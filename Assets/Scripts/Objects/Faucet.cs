using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faucet : MonoBehaviour

{

    public GameObject water;
    public float waterLevel = 0.8f;
    public bool draining = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (draining && waterLevel > 0 ){
            waterLevel -= Time.deltaTime;
            water.transform.position -= Vector3.up * Time.deltaTime;
        }
    }
    void OnTriggerStay(Collider Other){
        if(Other.gameObject.CompareTag("Player") && !Other.isTrigger){
            if (Input.GetKey(KeyCode.E)){
                gameObject.GetComponent<Animation>().Play();
                draining = true;
            }
        }
    }
}
