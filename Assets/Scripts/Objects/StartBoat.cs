using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoat : MonoBehaviour
{
    // This script should be applied to the most parentest boat
    
    private GameObject player;
    private GameObject boat;
    public bool sailing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boat = this.gameObject;
    }

    // when we get on boat
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger)
        {
            // make player child of boat & disable movement
            player.transform.parent = boat.transform;
            player.GetComponent<PlayerMovement>().enabled = false;

            // begin sailing
            sailing = true;
        }
    }

    void FixedUpdate()
    {
        if (sailing)
        {
            boat.transform.Translate(0.1f, 0, 0);
        }
    }
}
