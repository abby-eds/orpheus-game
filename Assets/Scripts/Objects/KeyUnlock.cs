using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUnlock : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject door;

    void OnTriggerEnter(Collider other){
        Debug.Log("OnTriggerEnter");
        if(other.gameObject.CompareTag("Player") && !other.isTrigger){
            Debug.Log("player triggered key");
            door.GetComponent<Animation>().Play();
            gameObject.SetActive(false);
        }
    }
}
