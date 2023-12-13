using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPerson_SOD : SOD_Reveal
{
    private Material mat;
    private float maxAmnt;
    private float maxSize;

    private void Start()
    {

        mat = GetComponent<Renderer>().material;
        maxAmnt = mat.GetFloat("amount");
        mat.SetFloat("amount", 0);
        
        
    }

    public override void fadeIn(float speed)
    {
        GetComponent<Renderer>().enabled = true;
        var amnt = mat.GetFloat("amount");
        if (amnt <= maxAmnt)
        {
            amnt += speed * Time.deltaTime;
            mat.SetFloat("amount", amnt);
        }
    }

    public override void fadeOut(float speed)
    {
        var amnt = mat.GetFloat("amount");
        if (amnt > 0)
        {
            amnt -= speed * Time.deltaTime;

        }
        else if (amnt != 0)
        {
            amnt = 0;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
        mat.SetFloat("amount", amnt);
    }
}
