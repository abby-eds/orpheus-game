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

    protected override void OnNeutral()
    {
        state = State.WORK;
    }

    protected override void OnHostile()
    {
        state = State.CHASE;
    }

    private void working()
    {
        if(transform.position != workZone)
        {
            nav.SetDestination(workZone);
        }
        else
        {
            anim.SetTrigger("Work");
        }
    }

    private void chasingPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= attackDistance)
        {
            
        }
        else if(playerInSight(chaseDistance))
        {
            anim.SetTrigger("Chase");
            nav.SetDestination(player.transform.position);
        }
        else
        {
            state = State.WORK;
        }
    }

    private void attack()
    {

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
        Vector3 origin = transform.position + Vector3.up;
        Vector3 dir = player.transform.position - origin;
        Ray ray = new Ray(origin, dir);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, distance))
        {
            return hit.transform.gameObject.tag == "Player";
        }

        return false;
    }
}
