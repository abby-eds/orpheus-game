// RandomPointOnNavMesh
using UnityEngine;
using UnityEngine.AI;

public class Goat : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range = 10.0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //print("speed" + agent.speed);
        //print("path" + agent.pathPending);
        //print("rem" + agent.remainingDistance);
        //print("stop" + agent.stoppingDistance);
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            // at point, make new destination
            Vector3 point;
            if (RandomPoint(agent.transform.position, range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                 agent.SetDestination(point);
               
            }
        }
        
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;

    }
}