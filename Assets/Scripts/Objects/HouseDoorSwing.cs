using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HouseDoorSwing : MonoBehaviour
{
    public bool nearby = false;
    private bool open = false;
    private Animator anim;
    private GameObject player;
    private AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (nearby && Input.GetKeyDown(KeyCode.E))
        {
            open = !open;
            anim.SetBool("Open", open);
            audioSource.Play();
            if (player != null) player.GetComponent<Animator>().SetTrigger("Interact");
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

