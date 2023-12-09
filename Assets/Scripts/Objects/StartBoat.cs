using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoat : MonoBehaviour
{
    // This script should be applied to the most parentest boat
    
    private GameObject player;
    private GameObject boat;
    private GameObject charon;
    private Animator anim;
    private Chatter chatter;
    public bool sailing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boat = this.gameObject;
        charon = GameObject.Find("SitCharon");
        anim = charon.GetComponent<Animator>();
        chatter = charon.GetComponent<Chatter>();
    }

    // when we get on boat
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger)
        {
            // make player child of boat & disable movement
            player.transform.parent = boat.transform;
            player.GetComponent<PlayerMovement>().enabled = false;

            // start rowing
            anim.SetBool("Rowing", true);

            // begin sailing
            sailing = true;

            Invoke("TeachSong", 3);
        }
    }

    void TeachSong()
    {
        chatter.ModifyChatter(BubbleType.Speech, "Heed my voice, and learn my song...", true);
        player.GetComponent<RingMusic>().LearnSong();
    }

    void FixedUpdate()
    {
        if (sailing)
        {
            boat.transform.Translate(0.05f, 0, 0);
        }
    }
}
