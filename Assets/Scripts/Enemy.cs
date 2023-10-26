using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyStatus
    {
        Hostile,
        Neutral,
        Charmed,
        Asleep
    }

    private float willpower;
    public float maxWillpower;
    public float willpowerRegen;

    private EnemyStatus status;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (willpowerRegen > 0 && willpower < maxWillpower)
        {
            willpower += willpowerRegen * Time.deltaTime;
            if (willpower > maxWillpower)
            {
                willpower = maxWillpower;
            }
        }
    }

    public void ApplyCharm(float charmAmount)
    {
        willpower -= charmAmount;
    }

    private void UpdateStatus()
    {
        float willpowerPercent = willpower / maxWillpower;
    }
}
