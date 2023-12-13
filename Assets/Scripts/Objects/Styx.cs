using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Styx : MonoBehaviour
{
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger)
        {
            player.GetComponent<PlayerHealth>().TakeDamage();
            player.GetComponent<PlayerHealth>().TakeDamage();
            player.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}
