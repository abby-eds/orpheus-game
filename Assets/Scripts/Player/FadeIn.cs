using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Color targetColor;
    public float fadeTime;
    private float fadeValue;

    // Update is called once per frame
    void Update()
    {
        if (fadeValue < 1)
        {
            fadeValue += Time.deltaTime / fadeTime;
            if (fadeValue > 1) fadeValue = 1;
            gameObject.GetComponent<Image>().color = new Color(targetColor.r, targetColor.g, targetColor.b, targetColor.a * fadeValue);
        }
    }
}
