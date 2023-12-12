using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOD_Reveal : Spectral
{
    [SerializeField]
    private float fadeInSpeed;
    [SerializeField]
    private float fadeOutSpeed;
    [SerializeField]
    private float fadeDelay;
    private float delaytimer;
    [SerializeField]
    private float range;



    private void OnValidate()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        if (!col)
        {
            col = gameObject.AddComponent<SphereCollider>();
            col.isTrigger = true;
        }

        col.radius = range;

    }

    // Update is called once per frame
    void Update()
    {
        if (delaytimer <= 0)
        {
            fadeOut(fadeOutSpeed);
        }
        else
        {
            delaytimer -= Time.deltaTime;
        }
    }

    public override void ApplySongOfDead(int level)
    {
        Debug.Log("reveal");
        delaytimer = fadeDelay;
        fadeIn(fadeInSpeed);
    }

    public virtual void fadeOut(float speed)
    {
        Debug.Log("fade out default");
    }

    public virtual void fadeIn(float speed)
    {
        Debug.Log("fade out default");
    }
}
