using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLost_SOC : Charmable
{
    [Header("Song of Charms")]
    private TheLost_SOC soc;
    [SerializeField]
    private float followSpeed;
    private bool follow;
    private GameObject player;
    [SerializeField]
    private float circleDistance;
    [SerializeField]
    private float circleSpeed;
    private float circleLoc;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Update()
    {
        base.Update();

        if(follow)
        {
            followPlayer();
        }

    }

    protected override void OnCharmed()
    {
        follow = true;
    }


    private void followPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, makeCircleAround(player.transform.position + Vector3.up * 2f, circleDistance, circleSpeed * Time.deltaTime), followSpeed * Time.deltaTime);
    }

    private Vector3 makeCircleAround(Vector3 pos, float dist, float speed)
    {
        circleLoc += speed;
        return new Vector3(
            Mathf.Cos(circleLoc) * dist + pos.x,
            pos.y,
            Mathf.Sin(circleLoc) * dist + pos.z
            );

    }

}
