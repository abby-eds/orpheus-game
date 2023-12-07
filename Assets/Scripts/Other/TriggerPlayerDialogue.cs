using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerDialogue : MonoBehaviour
{
    private GameObject player;
    private Chatter playerChat;
    public BubbleType bubbleType;
    public string text;
    public bool right;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerChat = player.GetComponent<Chatter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger)
        {
            playerChat.ModifyChatter(bubbleType, text, right);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger)
        {
            playerChat.ModifyChatter(bubbleType, "", right);
        }
    }
}
