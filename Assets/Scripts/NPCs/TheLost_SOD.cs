using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLost : SOD_Reveal
{
    private ParticleSystem.MainModule ptcl;
    private float maxPtclLifetime;
    [SerializeField]
    private float maxLight;
    private Light light;
    private AudioSource snd;
    [SerializeField]
    private AudioClip[] sighs;
    [SerializeField]
    private AudioClip[] hushs;

    void Start()
    {
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

    public override void fadeIn(float speed)
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

    public override void fadeOut(float speed)
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