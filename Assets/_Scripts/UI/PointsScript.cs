using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsScript : MonoBehaviour
{
    public TextMeshProUGUI pointsText;

    [HideInInspector]
    public int targetPoints;

    public float currentPoints;

    void Update()
    {
        if(targetPoints != currentPoints)
        {
            currentPoints = Mathf.Lerp(currentPoints, targetPoints, 20 * Time.deltaTime);

            if(Mathf.Abs(currentPoints - targetPoints) < 1)
            {
                currentPoints = targetPoints;
            }

            pointsText.text = ((int)currentPoints).ToString();
        }
    }

    public void UpdatePoints(int points)
    {
        //pointsText.text = points.ToString();

        targetPoints = points;

        transform.DOKill();

        DOTween.Sequence()
            .Append(transform.DOScale(1.2f, 0.1f))
            .Append(transform.DOScale(1f, 0.2f));

        DOTween.Sequence()
            .Append(transform.DORotate(new Vector3(0, 0, -5), 0.1f))
            .Append(transform.DORotate(new Vector3(0, 0, 0), 0.2f));

    }
}
