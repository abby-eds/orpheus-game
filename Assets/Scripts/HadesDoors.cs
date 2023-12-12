using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesDoors : Sculptable
{
    // Start is called before the first frame update
    private bool activated = false;

    public override void ApplySongOfSculpting(int level)
    {
        Debug.Log("Song of skulpting activaed");
        if (level > 1 && activated == false){
            gameObject.GetComponent<Animation>().Play();
            activated = true;
        }

    }
}
