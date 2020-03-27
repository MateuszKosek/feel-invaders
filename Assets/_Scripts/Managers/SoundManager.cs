using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource shoot;
    public AudioSource enemyHit;
    
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private float lastShootTime;
    private float minShootTime = 0.075f;

    public void PlayShoot()
    {
        if (shoot == null || Time.time - lastShootTime < minShootTime)
            return;

        shoot.pitch = Random.Range(0.95f, 1.05f);
        shoot.Play();

        lastShootTime = Time.time;
    }

    private float lastHitTime;
    private float minHitTime = 0.1f;

    public void PlayEnemyHit()
    {
        if (enemyHit == null || Time.time - lastHitTime < minHitTime)
            return;

        enemyHit.pitch = Random.Range(0.9f, 1.1f);
        enemyHit.Play();

        lastHitTime = Time.time;
    }
}
