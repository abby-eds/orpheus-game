using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goat : Charmable
{
    private RandomWander wander;
    private NavMeshAgent agent;
    private Animator anim;
    private Vision vision;
    private Ram ram;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        wander = GetComponent<RandomWander>();
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<Vision>();
        wander.setAgent(agent);
        ram = GetComponent<Ram>();
        ram.setAgent(agent);
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        switch(Status)
        {
            case CharmStatus.Hostile:
                if (vision.isPlayerInSight())
                {
                    // go ram the player
                    // on success, become netural for a bit
                    if (ram.goRam(player))
                    {
                        willpower = 60;
                    }
                }
                else
                {
                    // no player, wander hostile-ly
                    wander.Wander();
                }
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
                agent.ResetPath();
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