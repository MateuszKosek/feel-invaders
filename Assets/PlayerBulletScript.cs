using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    public float speed = 10f;
    
    void Start()
    {

    }
    
    void Update()
    {
        transform.position = transform.position + new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BulletKiller"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);

            EnemyScript enemy = collision.GetComponent<EnemyScript>();
            enemy.Hit();
        }
    }
}
