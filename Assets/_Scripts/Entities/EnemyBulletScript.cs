using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed = -1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(0, speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletKiller"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);

            PlayerScript player = collision.GetComponent<PlayerScript>();

            player.Hit();
        }
    }
}
    