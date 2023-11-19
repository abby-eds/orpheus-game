using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goat : Charmable
{
    private RandomWander wander;
    private NavMeshAgent agent;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        wander = GetComponent<RandomWander>();
        agent = GetComponent<NavMeshAgent>();
        wander.setAgent(agent);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        switch(Status)
        {
            case CharmStatus.Hostile:
                break;

            case CharmStatus.Neutral:
                // wander around
                wander.Wander();
                break;
            case CharmStatus.Charmed:
                // this should never happen
                break;
            case CharmStatus.Asleep:
                // don't move at all
                break;
        }
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
    }

    protected override void OnHostile()
    {

    }

    protected override void OnNeutral()
    {
    }

    protected override void OnCharmed()
    {


    }

    protected override void OnAsleep()
    {

    }
}
