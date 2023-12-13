using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Charmable
{
    private RandomWander wander;
    private NavMeshAgent agent;
    private Animator anim;
    private Vision vision;
    private SkeletonSwing SkeletonSwing;
    private GameObject player;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        wander = GetComponent<RandomWander>();
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<Vision>();
        wander.setAgent(agent);
        SkeletonSwing = GetComponent<SkeletonSwing>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        SkeletonSwing.setup(agent, player, this.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        switch (Status)
        {
            case CharmStatus.Hostile:
                if (vision.isPlayerInSight())
                {
                    // approach, then hit
                    SkeletonSwing.goHit();
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
        float speed = agent.velocity.sqrMagnitude;
        anim.SetFloat("Speed", speed);
        if (speed > 0 && !audioSource.isPlaying && Time.timeScale > 0)
        {
            audioSource.Play();
        } 
        else if (speed <= 0 || Time.timeScale == 0)
        {
            audioSource.Stop();
        }
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
