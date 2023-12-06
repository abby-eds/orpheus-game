using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unseenbox : Spectral{

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0){
            timer -= Time.deltaTime;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else{
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        
    }
    public override void ApplySongOfDead(int level){
        timer = 1;
    }
}
