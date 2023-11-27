using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Charmable charmable;
    public Slider hostileBar;
    public Slider neutralBar;
    public Slider charmedBar;
    public Slider asleepBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hostileBar.gameObject.SetActive(charmable.Status == Charmable.CharmStatus.Hostile);
        neutralBar.gameObject.SetActive(charmable.Status == Charmable.CharmStatus.Neutral);
        charmedBar.gameObject.SetActive(charmable.Status == Charmable.CharmStatus.Charmed);
        asleepBar.gameObject.SetActive(charmable.Status == Charmable.CharmStatus.Asleep);
        asleepBar.value = charmable.willpower;
        charmedBar.value = charmable.willpower;
        neutralBar.value = charmable.willpower;
        hostileBar.value = charmable.willpower;
    }

    public void AssignEnemy(Charmable charmable)
    {
        this.charmable = charmable;
        asleepBar.minValue = 0;
        asleepBar.maxValue = charmable.asleepThreshold;
        charmedBar.minValue = charmable.asleepThreshold;
        charmedBar.maxValue = charmable.charmedThreshold;
        neutralBar.minValue = charmable.charmedThreshold;
        neutralBar.maxValue = charmable.neutralThreshold;
        hostileBar.minValue = charmable.neutralThreshold;
        hostileBar.maxValue = charmable.maxWillpower;
        asleepBar.value = charmable.willpower;
        charmedBar.value = charmable.willpower;
        neutralBar.value = charmable.willpower;
        hostileBar.value = charmable.willpower;
    }

    public void UpdatePosition()
    {
        Vector3 enemyPosition = charmable.transform.position + Vector3.up * charmable.healthbarOffset;
        transform.localPosition = (Camera.main.WorldToScreenPoint(enemyPosition) - new Vector3(Screen.width / 2, Screen.height / 2, 0)) * 1920 / Screen.width;
    }
}
