using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitFallDmg : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) 
        {
            Invoke("Fall", 1.25f);
        }
    }

    void Fall()
    {
        player.GetComponent<PlayerHealth>().TakeDamage();
        player.GetComponent<PlayerHealth>().TakeDamage();
        player.GetComponent<PlayerHealth>().TakeDamage();
    }
}
