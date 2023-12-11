using UnityEngine;
using UnityEngine.AI;

public class Ram : MonoBehaviour
{
    private NavMeshAgent agent;
    private float ramSpeed = 12f;
    private GameObject player;
    private GameObject goat;

    public void setup(NavMeshAgent agent, GameObject player, GameObject goat)
    {
        this.agent = agent;
        this.goat = goat;
        this.player = player;
    }
    public bool goRam()
    {
        // go to player
        agent.speed = ramSpeed;
        agent.SetDestination(player.transform.position);
        
        float distance = Vector3.Distance(goat.transform.position, player.transform.position);
        if (distance < 1)
        {
            // hit em
            player.GetComponent<PlayerHealth>().TakeDamage();
            agent.ResetPath();
            return true;
        }

        return false;

    }

}