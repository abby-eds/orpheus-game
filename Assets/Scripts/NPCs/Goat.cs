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
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        ram.setup(agent, player, this.gameObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();


        int baaChance = Random.Range(0, 100);
        if (baaChance == 0 && Status != CharmStatus.Asleep && Time.timeScale > 0)
        {
            GetComponent<AudioSource>().Play();
        }



        switch(Status)
        {
            case CharmStatus.Hostile:
                if (vision.isPlayerInSight())
                {
                    // go ram the player
                    // on success, become netural for a bit
                    if (ram.goRam())
                    {
                        willpower = (asleepThreshold + neutralThreshold) / 2;
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
        base.OnHostile();
    }

    protected override void OnNeutral()
    {
        base.OnNeutral();
        agent.ResetPath();
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
