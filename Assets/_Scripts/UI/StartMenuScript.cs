using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour
{
    public List<SlideInButtonScript> buttons;
    public Image tintImage;

    public GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void StartGame()
    {
        foreach(var button in buttons)
        {
            button.MoveOut();
        }

        tintImage.DOFade(0, 0.5f)
            .SetDelay(0.5f)
            .OnComplete(gm.StartGame);
    }
}
