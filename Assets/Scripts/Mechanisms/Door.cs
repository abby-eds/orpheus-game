using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Out_Door : Mechanism_out
{
    private Animator anim;
    private ParticleSystem[] particles;
    private AudioSource snd;

    private void Start()
    {
        anim = GetComponent<Animator>();
        particles = GetComponentsInChildren<ParticleSystem>();
        snd = GetComponent<AudioSource>();
    }

    public override void activate()
    {
        Debug.Log("open");
        anim.SetTrigger("Open");
    }

    private void activateParticles()
    {
        foreach(ParticleSystem p in particles)
        {
            p.Play();
        }
    }

    private void playOpenSound()
    {
        snd.Play();
    }
}
