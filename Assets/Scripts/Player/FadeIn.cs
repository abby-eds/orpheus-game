using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadeTime;
    private float fadeValue;

    // Update is called once per frame
    void Update()
    {
        if (fadeValue < 1)
        {
            fadeValue += Time.deltaTime / fadeTime;
            if (fadeValue > 1) fadeValue = 1;
            gameObject.GetComponent<CanvasGroup>().alpha = fadeValue;
        }
    }
}
