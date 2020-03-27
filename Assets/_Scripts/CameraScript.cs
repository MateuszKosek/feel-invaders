using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera camera;

    private float shakeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            camera.transform.position = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), camera.transform.position.z);
        }
        else
        {
            camera.transform.position = new Vector3(0, 0, camera.transform.position.z);
        }

        

    }

    public void ShakeScreen(float time)
    {
        shakeTime = time;
    }
    

}
