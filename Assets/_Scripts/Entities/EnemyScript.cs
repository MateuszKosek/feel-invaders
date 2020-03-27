using DG.Tweening;
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
    public GameObject destroyedFxPrefab;
    public ParticleSystem hitFx;
    public SpriteRenderer gfx;

    public SpriteMask spriteMask;
    public SpriteRenderer maskGfx;
    
    private float timeToShoot;

    public Action<EnemyScript> onDied;

    public Func<bool> canShoot;

    private bool dead = false;
    
    // Start is called before the first frame update
    void Start()
    {
        timeToShoot = cadency * UnityEngine.Random.value + 2f;

        gfx.color = new Color(UnityEngine.Random.Range(0.8f, 1f), 
            UnityEngine.Random.Range(0.8f, 1f), 
            UnityEngine.Random.Range(0.8f, 1f), 
            UnityEngine.Random.Range(0.9f, 1f));

        spriteMask.sprite = gfx.sprite;
        spriteMask.enabled = false;
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
        SoundManager.instance.PlayEnemyHit();

        FlashWhite();

        hitFx.Play();

        DOTween.Kill(gameObject);
        Tweens.Squeeze(gameObject, 0.5f);

        if (--hp <= 0)
        {
            Kill();
        }

        gfx.DOKill();
        DOTween.Sequence()
            .Append(gfx.transform.DOLocalMoveY(0.15f, 0.03f).SetEase(Ease.OutSine))
            .Append(gfx.transform.DOLocalMoveY(0, 0.1f).SetEase(Ease.InSine));

    }


    public void Kill()
    {
        if (dead)
            return;

        dead = true;

        StartCoroutine(KillIntertnal());
    }

    private IEnumerator KillIntertnal()
    {
        yield return new WaitForSeconds(0.05f);

        Destroy(gameObject);

        onDied.Invoke(this);

        GameObject fx = Instantiate(destroyedFxPrefab);
        fx.transform.position = transform.position;
    }

    public void FlashWhite()
    {
        spriteMask.enabled = true;
        maskGfx.color = new Color(1, 1, 1, 1);

        maskGfx.DOKill();
        maskGfx.DOFade(0, 0.1f).SetEase(Ease.InSine)
            .OnComplete(() => { spriteMask.enabled = false; });


    }

    public void TweenMoveIn(float delay, Action callback)
    {
        transform.DOMoveY(5, 2f + UnityEngine.Random.Range(-0.3f, 0.3f))
                .From(true)
                .SetEase(Ease.OutBack)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                });
    }

    public void TweenMoveOut(float delay, Action callback)
    {
        transform.DOMoveY(-100, 2f + UnityEngine.Random.Range(-0.3f, 0.3f))
                .SetRelative (true)
                .SetEase(Ease.InSine)
                .SetDelay(delay)
                .OnComplete(() =>
                {
                    callback?.Invoke();
                });
    }
}
