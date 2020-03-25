using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSliderScript : MonoBehaviour
{

    public List<Image> sliderParts;

    public Sprite unactiveImage;
    public Sprite activeImage;

    // Start is called before the first frame update
    void Awake()
    {
        SetProgress(0);
    }
    
    public void SetProgress(float progress)
    {
        int litCount = Mathf.RoundToInt(sliderParts.Count * progress);

        for (int i = 0; i < sliderParts.Count; i++)
        {
            sliderParts[i].sprite = i < litCount ? activeImage : unactiveImage;
        }
    }
}
