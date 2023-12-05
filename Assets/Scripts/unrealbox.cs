using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unrealbox : Spectral
{
    // Start is called before the first frame update

    public bool turnOn = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshRenderer>().enabled = turnOn;
        
    }

    public override void ApplySongOfDead(int level){
        if (level > 0){
            //change object to new visibility
            // do I want it to be only shadows??
            // do I want it to be a faint blue glow
            turnOn = false;
            
        }
        if (level == 0){
            //not playing
            // make objecs solid again.
            turnOn = true;

        }
    }

}
