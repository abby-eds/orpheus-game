using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    [Range(0, 360)]
    public float fieldOfViewAngle = 110f;
    private bool playerInSight = false;
    private GameObject player;
    private float sightTimeStep = 0.5f;

    public bool isPlayerInSight()
    {
        return playerInSight;
    }
    private void CheckSight()
    {
        playerInSight = false;
        RaycastHit hit;
        Vector3 enemyToPlayer = player.gameObject.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(enemyToPlayer, transform.forward);
        bool isAngleUnderHalfAngleOfView = angleToPlayer < fieldOfViewAngle * 0.5f;

        if (isAngleUnderHalfAngleOfView &&
            Physics.Raycast(transform.position + transform.up,
            enemyToPlayer, out hit, 8))
        {
            playerInSight = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            InvokeRepeating("CheckSight", sightTimeStep, sightTimeStep);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CancelInvoke("CheckSight");
            playerInSight = false;
        }
    }
}
