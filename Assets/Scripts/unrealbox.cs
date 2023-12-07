using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unrealbox : Spectral
{
    // Start is called before the first frame update

    public float timer = 0;
    public Material opaqueMat;
    public Material transparentMat;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<MeshRenderer>().enabled = turnOn;
        if(timer > 0){
            timer -= Time.deltaTime;
            UpdateMaterial(true);
        }
        else{
            UpdateMaterial(false);
        }
        
    }

    public override void ApplySongOfDead(int level){
        timer = 1;

    }

    void UpdateMaterial(bool transparent){
        if (transparent) {
        rend.material = transparentMat;
    }
    else {
        rend.material = opaqueMat;
    }

    }

}
