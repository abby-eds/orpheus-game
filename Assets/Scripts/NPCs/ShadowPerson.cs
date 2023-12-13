using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : Charmable
{
    private enum State
    {
        WORK,
        CHASE,
        ATTACK
    };

    private NavMeshAgent nav;
    private State state;
    private GameObject player;
    private Vector3 workZone;
    private Animator anim;
    [SerializeField]
    private float attackDistance = 1;
    [SerializeField]
    private float chaseDistance = 1;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
       
        workZone = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {

        base.Update();

        switch(Status)
        {
            case CharmStatus.Hostile:
                state = State.CHASE;
                break;
            case CharmStatus.Neutral:
                state = State.WORK;
                break;
            case CharmStatus.Charmed:
                state = State.WORK;
                break;
        }




        switch(state)
        {
            case State.WORK:
                working();
                break;
            case State.CHASE:
                chasingPlayer();
                break;
            default:
                break;
        }
        
    }

    private void working()
    {
        if (Vector3.Distance(transform.position, workZone) > nav.stoppingDistance + 1)
        {
            nav.SetDestination(workZone);
        }
        else
        {
            anim.SetTrigger("Work");
            if(Status == CharmStatus.Neutral)
            {
                if (playerInSight(chaseDistance))
                {
                    willpower = maxWillpower;
                }
            }
        }
    }

    private void chasingPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            nav.SetDestination(transform.position);
            attack();
        }
        else
        {
            anim.SetTrigger("Chase");
            nav.SetDestination(player.transform.position);
        }
    }

    private void attack()
    {
        
        anim.SetTrigger("Attack");
    }

    private void dealDamage()
    {
        if(playerInSight(attackDistance))
        {
            player.GetComponent<PlayerHealth>().TakeDamage();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    private bool playerInSight(float distance)
    {
        /*Vector3 origin = transform.position + Vector3.up;
        Vector3 dir = player.transform.position - origin;
        Ray ray = new Ray(origin, dir);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, distance))
        {
            return hit.transform.gameObject.tag == "Player";
        }

        return false;
        */

        return Vector3.Distance(transform.position, player.transform.position) <= distance;
    }
}
