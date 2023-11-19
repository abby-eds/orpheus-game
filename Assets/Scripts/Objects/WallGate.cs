using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGate : MonoBehaviour
{
    public float gateAngleChange = 0;
    public float currentGateAngle = 60;
    public GameObject gate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gate.transform.Rotate(new Vector3(0, gateAngleChange, 0));
        currentGateAngle = gate.transform.localRotation.eulerAngles.y;
    }
}
