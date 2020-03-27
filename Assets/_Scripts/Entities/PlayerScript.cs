using DG.Tweening;
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
    public SpriteRenderer gfx;
    public GameObject destroyFx;
    public SpriteRenderer muzzleFlash;

    private Camera mainCamera;
    private float shootTimer;
    private float cadencyMul = 1f;

    public Action onDeath;

    public Func<bool> isStarted;

    private Vector3 cachedPosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        cachedPosition = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y - 5, transform.position.z);
    }

    public void MoveIn()
    {
        transform.DOMoveY(cachedPosition.y, 1f)
            .SetEase(Ease.OutBack)
            .SetDelay(1f);
    }
    
    void Update()
    {
        UpdatePosition();

        if (!isStarted?.Invoke() ?? true)
            return;

        UpdateShooting();

        transform.rotation = Quaternion.Euler(0, 0, movementSide * 10);
    }

    public void Upgrade()
    {
        cadencyMul *= 0.5f;
    }

    float movementSide = 0;

    void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

        movementSide = transform.position.x - worldPos.x;

        transform.position = Vector3.Lerp(transform.position, 
            new Vector3(worldPos.x, transform.position.y, transform.position.z), 30 * Time.deltaTime);

        //transform.position = new Vector3(worldPos.x, transform.position.y, transform.position.z);

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

            SoundManager.instance.PlayShoot();

            PushBack();
            MuzzleFlash();
        }

    }

    void PushBack()
    {
        DOTween.Kill(gfx.gameObject);
        DOTween.Sequence()
            .Append(gfx.transform.DOLocalMoveY(-0.05f, 0.03f).SetEase(Ease.OutSine))
            .Append(gfx.transform.DOLocalMoveY(0, 0.1f).SetEase(Ease.InSine));
    }

    void Shoot(float shiftX)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = shootingPosition.transform.position + new Vector3(shiftX, 0, 0);
    }

    void MuzzleFlash()
    {
        muzzleFlash.DOKill();

        muzzleFlash.color = new Color(1, 1, 1, 1);
        muzzleFlash.DOFade(0, 0.1f).SetEase(Ease.InSine);
    }

    public void Hit()
    {
        Destroy(gameObject);

        onDeath.Invoke();

        GameObject fx = Instantiate(destroyFx);
        fx.transform.position = transform.position;
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
