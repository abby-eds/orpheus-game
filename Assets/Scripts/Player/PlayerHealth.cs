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
    public CanvasGroup healthCanvas;
    public Image health1;
    public Image health2;
    public Image health3;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public float healthDisplayDuration = 1;
    private float healthDisplayTimer;

    public bool takeDamage;
    public bool heal;
    private bool regen;

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
            healthCanvas.alpha = healthDisplayPercent;
        }
    }

    public void UpdateHealth()
    {
        health1.sprite = health >= 1 ? fullHeart : emptyHeart;
        health2.sprite = health >= 2 ? fullHeart : emptyHeart;
        health3.sprite = health >= 3 ? fullHeart : emptyHeart;
        healthCanvas.alpha = 1;
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

    public void Regen()
    {
        if (!regen)
        {
            regen = true;
            InvokeRepeating("Heal", 5, 5);
        }
    }

    public void CancelRegen()
    {
        if (regen)
        {
            regen = false;
            CancelInvoke("Heal");
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
