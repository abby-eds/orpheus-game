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
        asleepBar.value = enemy.willpower;
        charmedBar.value = enemy.willpower;
        neutralBar.value = enemy.willpower;
        hostileBar.value = enemy.willpower;
    }

    public void assignEnemy(Enemy enemy)
    {
        this.enemy = enemy;
        asleepBar.minValue = 0;
        asleepBar.maxValue = enemy.sleepThreshold;
        charmedBar.minValue = enemy.sleepThreshold;
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
