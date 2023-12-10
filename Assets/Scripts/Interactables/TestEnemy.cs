using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Charmable
{
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
        base.OnHostile();
    }

    protected override void OnNeutral()
    {
        base.OnNeutral();
    }

    protected override void OnCharmed()
    {
        base.OnCharmed();
    }

    protected override void OnAsleep()
    {
        base.OnAsleep();
    }
}
