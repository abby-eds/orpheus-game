using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLost : Spectral
{
    private ParticleSystem.MainModule ptcl;
    private float maxPtclLifetime;
    [SerializeField]
    private float fadeInSpeed;
    [SerializeField]
    private float fadeOutSpeed;
    [SerializeField]
    private float fadeDelay;
    private float delaytimer;
    [SerializeField]
    private float maxLight;
    [SerializeField]
    private float range;
    private Light light;
    private AudioSource snd;
    [SerializeField]
    private AudioClip[] sighs;
    [SerializeField]
    private AudioClip[] hushs;

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
    void Start()
    {
        //delaytimer = fadeDelay;
        light = GetComponentInChildren<Light>();
        if(maxLight <= 0)
        {
            maxLight = light.intensity;
        }
        light.intensity = 0;
        ptcl = GetComponent<ParticleSystem>().main;
        maxPtclLifetime = ptcl.startLifetime.constant;
        ptcl.startLifetime = new ParticleSystem.MinMaxCurve(0);

        snd = GetComponent<AudioSource>();
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

    private void fadeIn(float speed)
    {
        if (light.intensity <= 0)
        {
            snd.PlayOneShot(sighs[Random.Range(0, sighs.Length - 1)]);
        }

        if (light.intensity <= maxLight)
        {
            light.intensity += speed * Time.deltaTime;
        }

        if (ptcl.startLifetime.constant <= maxPtclLifetime)
        {
            ptcl.startLifetime = new ParticleSystem.MinMaxCurve(ptcl.startLifetime.constant + speed * Time.deltaTime);
        }
    }

    private void fadeOut(float speed)
    {

        if (light.intensity > 0)
        {
            light.intensity -= speed * Time.deltaTime;
        }


        if (ptcl.startLifetime.constant > 0)
        {
            ptcl.startLifetime = new ParticleSystem.MinMaxCurve(ptcl.startLifetime.constant - speed * Time.deltaTime);
        }
    }
}