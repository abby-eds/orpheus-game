using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigate : Charmable
{
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    public Transform[] waypoints;
    private int i = 0;
    private Ray ray;
    public float rayDistance = 10f;
    public float fieldOfViewAngle;
    private bool shouldChasePlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        agent =  GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        //agent.autoBraking = false;
    }

    // Update is called once per frame
    void GotoNextPoint(){
        if (waypoints.Length == 0)
            return;
        agent.destination = waypoints[i].position;
        i = (i+1) % waypoints.Length;
        agent.speed = 2.5f;
        fieldOfViewAngle =  140;
        rayDistance = 10;
        
    }
    protected override void Update()
    {

       base.Update();

       if(Status == CharmStatus.Asleep ){
            shouldChasePlayer = false;
            anim.SetFloat("Forwards", 0);
            agent.ResetPath();
            return;

       }
        else if (shouldChasePlayer){
                return;
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.1f){
                GotoNextPoint();
        }
        anim.SetFloat("Forwards", agent.velocity.sqrMagnitude);

        //ANIM UPDATE
       


        //RAY
        //ray = new Ray(transform.position + new Vector3(0f, 1.5f ,0f), transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction*rayDistance, Color.red);

        //if(Physics.Raycast(ray, out RaycastHit hit, rayDistance)){
        //    Debug.Log(hit.collider.gameObject.name);    
        //}
    }

    void OnTriggerStay(Collider Other){ // only cares if hostile
        if(Other.gameObject.CompareTag("Player") && (Status == CharmStatus.Hostile)){
            Debug.Log("Player in range..");
            RaycastHit hit;
            Vector3 enemyToPlayer = Other.gameObject.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(enemyToPlayer, transform.forward);
            bool isAngleUnderHalfView = angleToPlayer < fieldOfViewAngle*0.5f;

            if( isAngleUnderHalfView
                &&Physics.Raycast(transform.position + transform.up, enemyToPlayer.normalized, out hit, rayDistance)){
                    Debug.Log("player Seen!!");// set destination to player location
                    agent.destination = Other.gameObject.transform.position + enemyToPlayer.normalized*2;

                    // broden range on sight
                    fieldOfViewAngle = 200;
                    rayDistance = 12;
                    agent.speed =  4;
                    //punch it!!
                    if(Physics.Raycast(transform.position + transform.up, enemyToPlayer.normalized, out hit, 1)){
                        Debug.Log("player is close and seen");
                        anim.SetBool("Seen and Close", true);
                        agent.speed = 0;
                    }
            }
            else{
                anim.SetBool("Seen and Close",false);//stop running
            }
        }
    }

    protected override void OnHostile()
    {

    }

    protected override void OnNeutral()
    {
        agent.ResetPath();
    }

    protected override void OnCharmed()
    {


    }

    protected override void OnAsleep()
    {

    }
}