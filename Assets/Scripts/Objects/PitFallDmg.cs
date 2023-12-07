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
        if (other.gameObject == player && !other.isTrigger) 
        {
            Invoke("Fall", 1.0f);
        }
    }

    void Fall()
    {
        player.GetComponent<PlayerHealth>().TakeDamage();
        player.GetComponent<PlayerHealth>().TakeDamage();
        player.GetComponent<PlayerHealth>().TakeDamage();
    }
}
