using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [Header("Config")]
    public float hp = 2f;
    public float cadency = 0.5f;
    
    [Header("Referemces")]
    public GameObject shootingPosition;
    public GameObject bulletPrefab;
    
    private float timeToShoot;

    public Action<EnemyScript> onDied;

    public Func<bool> canShoot;

    // Start is called before the first frame update
    void Start()
    {
        timeToShoot = cadency * UnityEngine.Random.value + 2f;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateShooting();
    }

    void UpdateShooting()
    {
        if(!canShoot?.Invoke() ?? true)
        {
            return;
        }

        timeToShoot -= Time.deltaTime;

        if (timeToShoot <= 0)
        {
            timeToShoot = cadency;
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = shootingPosition.transform.position;
    }

    public void Hit()
    {
        if(--hp <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);

        onDied.Invoke(this);
    }
}
