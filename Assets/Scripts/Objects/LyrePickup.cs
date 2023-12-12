using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyrePickup : MonoBehaviour
{
    public bool nearby = false;
    private GameObject player;
    public TriggerPlayerDialogue dialogueZone;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (nearby && Input.GetKeyDown(KeyCode.E))
        {
            if (player != null)
            {
                player.GetComponent<Animator>().SetTrigger("Interact");
                player.GetComponent<RingMusic>().LearnSong();
                player.GetComponent<RingMusic>().lyre.GetComponent<Lyre>().Dequip();
                dialogueZone.text = "Time to go find the entrance<br>to the Underworld...";
                dialogueZone.playerChat.ModifyChatter(dialogueZone.bubbleType, dialogueZone.text, true);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            UIManager.UI.InteractActive(true);
            player = other.gameObject;
            nearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            UIManager.UI.InteractActive(false);
            player = null;
            nearby = false;
        }
    }
}
