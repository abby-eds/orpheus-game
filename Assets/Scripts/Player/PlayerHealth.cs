using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 3;
    public bool dead { get; private set; } = false;

    private Animator anim;
    public Image health1;
    public Image health2;
    public Image health3;
    public float healthDisplayDuration = 1;
    private float healthDisplayTimer;

    public Color fullHeart;
    public Color emptyHeart;

    public bool takeDamage;
    public bool heal;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    private void Update()
    {
        if (takeDamage)
        {
            takeDamage = false;
            TakeDamage();
        }
        if (heal)
        {
            heal = false;
            Heal();
        }
        if (health >= maxHealth && healthDisplayTimer > 0)
        {
            healthDisplayTimer -= Time.deltaTime;
            if (healthDisplayTimer < 0) healthDisplayTimer = 0;
            float healthDisplayPercent = healthDisplayTimer / healthDisplayDuration;
            health1.color = fullHeart * new Color(1, 1, 1, healthDisplayPercent);
            health2.color = fullHeart * new Color(1, 1, 1, healthDisplayPercent);
            health3.color = fullHeart * new Color(1, 1, 1, healthDisplayPercent);
        }
    }

    public void UpdateHealth()
    {
        health1.color = health >= 1 ? fullHeart : emptyHeart;
        health2.color = health >= 2 ? fullHeart : emptyHeart;
        health3.color = health >= 3 ? fullHeart : emptyHeart;
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            health--;
            healthDisplayTimer = healthDisplayDuration;
            UpdateHealth();
            if (health <= 0)
            {
                dead = true;
                anim.SetBool("Die", true);
                UIManager.UI.Invoke("ToGameOverMenu", 2);
            }
        }
    }

    public void Heal()
    {
        if (health < maxHealth)
        {
            health++;
            healthDisplayTimer = healthDisplayDuration;
            UpdateHealth();
        }
    }
}
