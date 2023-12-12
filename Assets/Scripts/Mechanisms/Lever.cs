using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class In_Lever : Mechanism_in
{
    [SerializeField]
    private float range = 1;
    private GameObject player;
    private bool activated = false;
    private Animator anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    public override bool trigger()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= range
            && Input.GetAxisRaw("Interact") != 0
            && !activated)
        {
            activated = true;
            anim.SetTrigger("Pull");
            return true;
            
            
        }
        return false;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (showWires)
        {
            Gizmos.color = wireColor;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
