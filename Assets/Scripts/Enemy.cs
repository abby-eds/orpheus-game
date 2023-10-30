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

    public float willpower = 100;
    public float sleepThreshold = 25;
    public float charmedThreshold = 50;
    public float neutralThreshold = 75;
    public float maxWillpower = 100;
    public float willpowerRegen;

    private EnemyStatus status;

    // Start is called before the first frame update
    void Start()
    {
        if (sleepThreshold > charmedThreshold || charmedThreshold > neutralThreshold) Debug.LogError("ERROR IN ASSIGNING CHARM THRESHOLDS");
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
        if (willpower < sleepThreshold) status = EnemyStatus.Asleep;
        else if (willpower < charmedThreshold) status = EnemyStatus.Charmed;
        else if (willpower < neutralThreshold) status = EnemyStatus.Neutral;
        else status = EnemyStatus.Hostile;
    }
}
