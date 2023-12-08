using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyrePickup : MonoBehaviour
{
    public bool nearby = false;
    private GameObject player;

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
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            player = other.gameObject;
            nearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            player = null;
            nearby = false;
        }
    }
}
