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
    public float willpowerRegen = 0;

    public float immunityDuration = 3;
    private float immunityTime = 0;

    public float resistaceDuration = 3;
    public float resistanceTime = 0;

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
            if (willpower > maxWillpower)  willpower = maxWillpower;
        }
        if (immunityTime > 0)
        {
            immunityTime -= Time.deltaTime;
            if (immunityTime < 0) immunityTime = 0;
        }
        else if (resistanceTime > 0)
        {
            resistanceTime -= Time.deltaTime;
            if (resistanceTime < 0) resistanceTime = 0;
        }
        UpdateStatus();
    }

    public void ApplyCharm(float charmAmount)
    {
        if (immunityTime <= 0)
        {
            willpower -= charmAmount * (1 - (resistanceTime / resistaceDuration));
        }
    }

    private void UpdateStatus()
    {
        EnemyStatus newStatus;
        if (willpower < sleepThreshold) newStatus = EnemyStatus.Asleep;
        else if (willpower < charmedThreshold) newStatus = EnemyStatus.Charmed;
        else if (willpower < neutralThreshold) newStatus = EnemyStatus.Neutral;
        else newStatus = EnemyStatus.Hostile;
        if (status != newStatus)
        {
            status = newStatus;
            immunityTime = immunityDuration;
            resistanceTime = resistaceDuration;
        }
    }
}
