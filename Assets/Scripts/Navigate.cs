using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigate : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    public Transform[] waypoints;
    private int i = 0;
    private Ray ray;
    public float rayDistance = 10f;
    public float fieldOfViewAngle;
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
        
    }
    void Update()
    {
       // if(agent.pathPending)
       //     return;
        

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
                GotoNextPoint();

        //ANIM UPDATE
        anim.SetFloat("Forwards", agent.velocity.sqrMagnitude);


        //RAY
        //ray = new Ray(transform.position + new Vector3(0f, 1.5f ,0f), transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction*rayDistance, Color.red);

        //if(Physics.Raycast(ray, out RaycastHit hit, rayDistance)){
        //    Debug.Log(hit.collider.gameObject.name);    
        //}
    }

    void OnTriggerStay(Collider Other){
        if(Other.gameObject.CompareTag("Player")){
            Debug.Log("Player in range..");
            RaycastHit hit;
            Vector3 enemyToPlayer = Other.gameObject.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(enemyToPlayer, transform.forward);
            bool isAngleUnderHalfView = angleToPlayer < fieldOfViewAngle*0.5f;
            if( isAngleUnderHalfView
                &&Physics.Raycast(transform.position + transform.up, enemyToPlayer.normalized, out hit, 8)){
                    Debug.Log("player Seen!!");
                    agent.destination = Other.gameObject.transform.position;
                }

        }
    }
}

