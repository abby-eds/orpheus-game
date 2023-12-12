using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonSwing : MonoBehaviour
{
    private NavMeshAgent agent;
    private float runSpeed = 4f;
    private GameObject player;
    private GameObject skeleton;
    private bool swinging = false;

    public void setup(NavMeshAgent agent, GameObject player, GameObject skeleton)
    {
        this.agent = agent;
        this.skeleton = skeleton;
        this.player = player;
    }
    public void goHit()
    {
        // go to player
        agent.speed = runSpeed;
        agent.SetDestination(player.transform.position);

        float distance = Vector3.Distance(skeleton.transform.position, player.transform.position);
        if (distance < 1 && !swinging)
        {
            // stop moving, swing
            swinging = true;
            agent.ResetPath();
            skeleton.GetComponent<Animator>().SetTrigger("Swing");
            Invoke("Swing", 1);
            Invoke("StopSwinging", 2);
        }

    }

    private void Swing()
    {
        float distance = Vector3.Distance(skeleton.transform.position, player.transform.position);
        if (distance < 2)
        {
            player.GetComponent<PlayerHealth>().TakeDamage();
        }
            
    }

    private void StopSwinging()
    {
        swinging = false;
    }

}