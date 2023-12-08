using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGuardAI : Charmable
{
    public WallGate gate;
    private GameObject player;
    private Chatter chatter;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        chatter = GetComponent<Chatter>();
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
        if (gate.gateAngleChange != -1) GetComponent<Animator>().SetTrigger("Interact");
        gate.gateAngleChange = -1;
        chatter.ModifyChatter(BubbleType.Speech, "Halt!", true);
    }

    protected override void OnCharmed()
    {
        if (gate.gateAngleChange != 1) GetComponent<Animator>().SetTrigger("Interact");
        gate.gateAngleChange = 1;
        chatter.ModifyChatter(BubbleType.Speech, "Oh, okay, you can pass...", true);
    }

    protected override void OnAsleep()
    {
        gate.gateAngleChange = 0;

        chatter.ModifyChatter(BubbleType.Speech, "Zzz...", true);
    }
}
