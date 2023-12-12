using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveOverworld : MonoBehaviour
{
    private GameObject player;
    private GameObject boat;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boat = GameObject.Find("Boat");

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger && boat.GetComponent<StartBoat>().sailing)
        {
            SceneTransition.Transition.TransitionToNextScene();
        }
    }
}
