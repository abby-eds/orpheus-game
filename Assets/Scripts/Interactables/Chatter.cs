using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BubbleType
{
    Speech,
    Thought,
}
public class Chatter : MonoBehaviour
{

    public BubbleType bubbleType;
    public string text;
    public bool right;
    public bool modified;
    public float textBubbleXOffset;
    public float textBubbleYOffset;

    public void ModifyChatter(BubbleType bubbleType, string text, bool right)
    {
        this.bubbleType = bubbleType;
        this.text = text;
        this.right = right;
        modified = true;
    }
}
