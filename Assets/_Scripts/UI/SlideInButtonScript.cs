using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideInButtonScript : MonoBehaviour
{
    public float delay;

    void OnEnable()
    {
        transform.DOLocalMoveX(-600, 1f).From(true)
            .SetDelay(delay)
            .SetEase(Ease.OutBack)
            .OnComplete(() => {
                StartUpDownTween();
            });
    }

    void StartUpDownTween()
    {
        DOTween.Sequence()
            .Append(transform.DOLocalMoveY(-10, 1f).SetRelative(true))
            .Append(transform.DOLocalMoveY(10, 1f).SetRelative(true))
            .SetLoops(-1);
    }
    
    public void MoveOut()
    {
        transform.DOLocalMoveX(-600, 1f).SetRelative(true)
            .SetDelay(delay)
            .SetEase(Ease.InSine);
    }
    
    public void OnPointerEnter()
    {
        //transform.localScale = new Vector3(1.2f, 1.2f, 1);
        transform.DOKill(true);

        if(!GetComponent<Button>().interactable)
        {
            transform.DOPunchPosition(new Vector3(10, 0), 0.2f);
        }
        else
        {
            transform.DOScale(1.2f, 0.2f);
        }

    }

    public void OnPointerDown()
    {

        //transform.localScale = new Vector3(0.8f, 0.8f, 1);
        transform.DOScale(0.9f, 0.1f);
    }

    public void OnPointerExit()
    {
        //transform.localScale = new Vector3(1f, 1f, 1);
        transform.DOScale(1f, 0.2f);
    }
}
