using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Config")]
    public float cadency = 0.5f;
    public float maxLineCadency = 0.05f;

    [Header("Referemces")]
    public GameObject shootingPosition;
    public GameObject bulletPrefab;

    public UpgradeSliderScript upgradeSlider;

    private Camera mainCamera;
    private float shootTimer;
    private float cadencyMul = 1f;

    public Action onDeath;

    public Func<bool> isStarted;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        if (!isStarted?.Invoke() ?? true)
            return;

        UpdatePosition();
        UpdateShooting();
    }

    public void Upgrade()
    {
        cadencyMul *= 0.5f;
    }

    void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(worldPos.x, transform.position.y, transform.position.z);

    }

    void UpdateShooting()
    {
        float currentCadency = Mathf.Max(cadency * cadencyMul, 0.001f);

        int minimumLines = Mathf.CeilToInt(maxLineCadency / currentCadency);
        float cadencyForLines = minimumLines * currentCadency;
      
        if (Input.GetMouseButtonDown(0))
        {
            shootTimer = cadencyForLines;
        }
        else if (Input.GetMouseButton(0))
        {
            shootTimer += Time.deltaTime;
        }

        if(shootTimer >= cadencyForLines)
        {
            shootTimer = 0;

            for (int i = 0; i < minimumLines; i++)
            {
                float shift = GetBulletShift(0.2f, minimumLines, i);
                
                Shoot(shift);
            }
        }

    }

    void Shoot(float shiftX)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = shootingPosition.transform.position + new Vector3(shiftX, 0, 0);
    }

    public void Hit()
    {
        Destroy(gameObject);

        onDeath.Invoke();
    }

    float GetBulletShift(float spaceBetweenLines, int lineCount, int lineIndex)
    {
        return GetLineStartShift(spaceBetweenLines, lineCount) + spaceBetweenLines * lineIndex;
    }

    float GetLineStartShift(float spaceBetweenLines, int lineCount)
    {
        float width = spaceBetweenLines * (lineCount - 1);
        return -(width / 2f);
    }

}
