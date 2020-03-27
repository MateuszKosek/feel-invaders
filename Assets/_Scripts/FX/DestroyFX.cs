using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFX : MonoBehaviour
{
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        Tweens.Squeeze(gameObject, 0.5f, 1.5f);

        sprite.DOFade(0, 0.2f)
            .SetDelay(0.05f)
            .SetEase(Ease.InSine);

        sprite.flipX = RandomBool();
        sprite.flipY = RandomBool();

        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));
        
        sprite.transform.DORotate(new Vector3(0, 0, 180 * RandomSign()), 5f * Random.Range(0.8f, 1f))
            .SetEase(Ease.InSine);
    }

    public bool RandomBool()
    {
        return Random.value >= 0.5f;
    }

    public int RandomSign()
    {
        return Random.Range(0, 1) * 2 - 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
