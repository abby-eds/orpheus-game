using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unseenbox : Spectral{

    private float timer = 0;
    private GameObject Orpheus;
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

    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Player") && (gameObject.GetComponent<MeshRenderer>().enabled == false)){
            Orpheus = other.gameObject;
            Orpheus.gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Thought, "Ouch! There's something there...?", true);
            Invoke("turnOffChatter", 2);
        }
    }
    public override void ApplySongOfDead(int level){
        timer = 1;
    }

    private void turnOffChatter(){
        Orpheus.gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Thought, "",true);
    }


}
