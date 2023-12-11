// RandomPointOnNavMesh
using UnityEngine;
using UnityEngine.AI;

public class RandomWander : MonoBehaviour
{
    private NavMeshAgent agent;
    public float range = 10.0f;
    private float minWaitTime = 0;
    public float maxWaitTime = 4f;
    private bool waiting = true;
    private float waitTime = 0;
    public float walkSpeed = 3f;
    private float stopSpeed = 0f;

    public void setAgent(NavMeshAgent agent)
    {
        this.agent = agent;
    }
    public void Wander()
    {
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            // at point, determine if waiting or moving
            agent.speed = stopSpeed;
            if (waiting)
            {
                if (waitTime > 0)
                {
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    waiting = false;
                }
            }
            else
            {
                // at point & wait over, make new destination
                Vector3 point;
                if (RandomPoint(agent.transform.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                    agent.speed = walkSpeed;
                    waiting = true;
                    waitTime = Random.Range(minWaitTime, maxWaitTime);
                }
            }
        }
        
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
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