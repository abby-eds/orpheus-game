using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGate : MonoBehaviour
{
    public float gateAngleChange = 0;
    public float currentGateAngle = 0;
    public GameObject gate;
    private bool move;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move = false;
      
        if (gateAngleChange > 0 && currentGateAngle < 90)
        {
            // opening & not all the way open
            move = true;
        } else if (gateAngleChange < 0 && currentGateAngle > 0)
        {
            // closing & not all the way closed
            move = true;
        }

        if (move)
        {
            gate.transform.Rotate(new Vector3(0, gateAngleChange, 0));
            currentGateAngle += gateAngleChange;
        }

        

    }
}
