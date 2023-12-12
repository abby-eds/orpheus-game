using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTransitionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player") && !other.isTrigger){
            gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
