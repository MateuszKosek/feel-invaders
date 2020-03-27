using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowTimeScale = 0.1f;

    private float targetTimeScale = 1;
    
    void Start()
    {
        Time.timeScale = targetTimeScale;
    }
    
    void Update()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * 60 * 0.05f);

        if (Time.timeScale >= 0.99f) Time.timeScale = 1;
    }

    public void SetSlow(bool force = false)
    {
        targetTimeScale = slowTimeScale;

        if (force) Time.timeScale = targetTimeScale;
    }

    public void SetNormal(bool force = false)
    {
        targetTimeScale = 1;

        if (force) Time.timeScale = targetTimeScale;
    }

    public void TemporarySlowdown(float slowTime, bool forceIn, bool forceOut)
    {
        StartCoroutine(TempSlowdownCorutine(slowTime, forceIn, forceOut));
    }

    IEnumerator TempSlowdownCorutine(float slowTime, bool forceIn, bool forceOut)
    {
        SetSlow(forceIn);
        yield return new WaitForSecondsRealtime(slowTime);
        SetNormal(forceOut);
    }
}
