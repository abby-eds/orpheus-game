using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBubble : MonoBehaviour
{
    public Chatter chatter;
    public TextMeshProUGUI text;
    public Image bubbleImage;
    public Sprite speechBubble;
    public Sprite thoughtBubble;
    public float xOffset;
    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AssignChatter(Chatter chatter)
    {
        this.chatter = chatter;
        if (chatter.bubbleType == Chatter.BubbleType.Speech)
        {
            bubbleImage.sprite = speechBubble;
            text.text = chatter.text;
        }
        else
        {
            bubbleImage.sprite = thoughtBubble;
            text.text = "<i>" + chatter.text + "</i>";
        }
        if (chatter.right) xOffset = chatter.textBubbleXOffset * 100;
        else
        {
            xOffset = chatter.textBubbleXOffset * -100;
            transform.localScale = new Vector3(-1, 1, 1);
            text.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void UpdatePosition()
    {
        Vector3 enemyPosition = chatter.transform.position + Vector3.up * chatter.textBubbleYOffset;
        transform.localPosition = (Camera.main.WorldToScreenPoint(enemyPosition) - new Vector3(Screen.width / 2, Screen.height / 2, 0)) * 1920 / Screen.width + Vector3.right * xOffset;
        if (Vector3.Dot(Camera.main.transform.forward, (enemyPosition - Camera.main.transform.position).normalized) > 0) canvasGroup.alpha = 1;
        else canvasGroup.alpha = 0;
    }
}
