using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGuardAI : Charmable
{
    public WallGate gate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
    }

    protected override void OnHostile()
    {
        
    }

    protected override void OnNeutral()
    {
        gate.gateAngleChange = -1;
    }

    protected override void OnCharmed()
    {
        gate.gateAngleChange = 1;
        
    }

    protected override void OnAsleep()
    {
        gate.gateAngleChange = 0;
    }
}
