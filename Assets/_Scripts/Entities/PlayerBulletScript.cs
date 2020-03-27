using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletScript : MonoBehaviour
{
    public float speed = 10f;

    public Vector3 direction = new Vector3(0, 1, 0);

    public SpriteRenderer spr;
    public SpriteRenderer hitSpr;

    private bool destroyed = false;

    void Start()
    {
        //direction = new Vector3(direction.x + Random.Range(-0.02f, 0.02f), direction.y, direction.z).normalized;
    }
    
    void Update()
    {
        if (destroyed)
            return;

        transform.position = transform.position + direction * speed * Time.deltaTime;
        
        transform.rotation = Quaternion.Euler(0, 0, new Vector2(direction.x, direction.y).Angle() - 90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyed)
            return;

        if(collision.CompareTag("BulletKiller"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            Kill();

            EnemyScript enemy = collision.GetComponent<EnemyScript>();
            enemy.Hit();
        }
    }

    public void Kill()
    {
        if (destroyed)
            return;

        destroyed = true;

        StartCoroutine(KillInternal());
    }

    IEnumerator KillInternal()
    {
        spr.gameObject.SetActive(false);
        hitSpr.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.05f);
        
        Destroy(gameObject);
    }
}
