using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charmable : MonoBehaviour
{
    public enum CharmStatus
    {
        Hostile,
        Neutral,
        Charmed,
        Asleep
    }

    public float willpower = 120;
    public float asleepThreshold = 30;
    public float charmedThreshold = 60;
    public float neutralThreshold = 90;
    public float maxWillpower = 120;
    public float asleepRegen = 1;
    public float charmedRegen = 1;
    public float neutralRegen = 0;
    public float hostileRegen = 0;

    public float regenDelayDuration = 5;
    private float regenDelayTime = 0;

    public float immunityDuration = 0;
    private float immunityTime = 0;

    public float resistaceDuration = 1;
    private float resistanceTime = 0;

    public float healthbarOffset = 2;

    public CharmStatus Status { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (asleepThreshold > charmedThreshold || charmedThreshold > neutralThreshold) Debug.LogError("ERROR IN ASSIGNING CHARM THRESHOLDS");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (regenDelayTime <= 0 && willpower < maxWillpower)
        {
            switch (Status)
            {
                case CharmStatus.Asleep:
                    willpower += asleepRegen * Time.deltaTime;
                    if (willpower > asleepThreshold && charmedRegen == 0) willpower = asleepThreshold;
                    break;
                case CharmStatus.Charmed:
                    willpower += charmedRegen * Time.deltaTime;
                    if (willpower > charmedThreshold && neutralRegen == 0) willpower = charmedThreshold;
                    break;
                case CharmStatus.Neutral:
                    willpower += neutralRegen * Time.deltaTime;
                    if (willpower > neutralThreshold && hostileRegen ==0) willpower = neutralThreshold;
                    break;
                case CharmStatus.Hostile:
                    willpower += hostileRegen * Time.deltaTime;
                    if (willpower > maxWillpower) willpower = maxWillpower;
                    break;
            }
        }
        if (regenDelayTime > 0)
        {
            regenDelayTime -= Time.deltaTime;
            if (regenDelayTime < 0) regenDelayTime = 0;
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

    public void ApplySongOfCharms(float charmAmount)
    {
        regenDelayTime = regenDelayDuration;
        if (immunityTime <= 0)
        {
            if (resistaceDuration > 0)
            {
                willpower -= charmAmount * (1 - (resistanceTime / resistaceDuration));
            }
            else
            {
                willpower -= charmAmount;
            }
            if (willpower < 0) willpower = 0;
        }
    }

    private void UpdateStatus()
    {
        CharmStatus newStatus;
        if (willpower <= asleepThreshold) newStatus = CharmStatus.Asleep;
        else if (willpower <= charmedThreshold) newStatus = CharmStatus.Charmed;
        else if (willpower <= neutralThreshold) newStatus = CharmStatus.Neutral;
        else newStatus = CharmStatus.Hostile;
        if (Status != newStatus)
        {
            Status = newStatus;
            immunityTime = immunityDuration;
            resistanceTime = resistaceDuration;
            switch (Status)
            {
                case CharmStatus.Hostile: OnHostile(); break;
                case CharmStatus.Neutral: OnNeutral(); break;
                case CharmStatus.Charmed: OnCharmed(); break;
                case CharmStatus.Asleep: OnAsleep(); break;
            }
        }
    }


    protected virtual void OnHostile()
    {
        Debug.Log("OnHostile: This method should be overwritten");
    }

    protected virtual void OnNeutral()
    {
        Debug.Log("OnNeutral: This method should be overwritten");
    }

    protected virtual void OnCharmed()
    {
        Debug.Log("OnCharmed: This method should be overwritten");
    }

    protected virtual void OnAsleep()
    {
        Debug.Log("OnAsleep: This method should be overwritten");
    }
}
