using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Enemy enemy;
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
        hostileBar.gameObject.SetActive(enemy.willpower >= enemy.neutralThreshold);
        neutralBar.gameObject.SetActive(enemy.willpower >= enemy.charmedThreshold);
        charmedBar.gameObject.SetActive(enemy.willpower >= enemy.asleepThreshold);
        asleepBar.value = enemy.willpower;
        charmedBar.value = enemy.willpower;
        neutralBar.value = enemy.willpower;
        hostileBar.value = enemy.willpower;
    }

    public void AssignEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        asleepBar.minValue = 0;
        asleepBar.maxValue = enemy.asleepThreshold;
        charmedBar.minValue = enemy.asleepThreshold;
        charmedBar.maxValue = enemy.charmedThreshold;
        neutralBar.minValue = enemy.charmedThreshold;
        neutralBar.maxValue = enemy.neutralThreshold;
        hostileBar.minValue = enemy.neutralThreshold;
        hostileBar.maxValue = enemy.maxWillpower;
        asleepBar.value = enemy.willpower;
        charmedBar.value = enemy.willpower;
        neutralBar.value = enemy.willpower;
        hostileBar.value = enemy.willpower;
    }
}
