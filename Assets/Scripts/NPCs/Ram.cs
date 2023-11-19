// RandomPointOnNavMesh
using UnityEngine;
using UnityEngine.AI;

public class Ram : MonoBehaviour
{
    private NavMeshAgent agent;
    private float ramSpeed = 4f;
    private float stopSpeed = 0f;

    public void setAgent(NavMeshAgent agent)
    {
        this.agent = agent;
    }
    public bool goRam(GameObject player)
    {
        // go to player
        agent.speed = ramSpeed;
        agent.SetDestination(player.transform.position);
        
        if (agent.remainingDistance < 1)
        {
            // hit em
            player.GetComponent<PlayerHealth>().TakeDamage();
            return true;
        }

        return false;

    }

}