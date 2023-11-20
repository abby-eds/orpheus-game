using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool nearby = false;
    private bool open = false;
    private Animator anim;
    private GameObject player;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (nearby && Input.GetKeyDown(KeyCode.E))
        {
            open = !open;
            anim.SetBool("Open", open);
            if (player != null) player.GetComponent<Animator>().SetTrigger("Interact");
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

