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
    private Chatter chatterCharon;
    private Chatter chatterPlayer;
    public bool sailing = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boat = this.gameObject;
        charon = GameObject.Find("SitCharon");
        anim = charon.GetComponent<Animator>();
        chatterCharon = charon.GetComponent<Chatter>();
        chatterPlayer = player.GetComponent<Chatter>();
    }

    // when we get on boat
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !other.isTrigger)
        {
            // make player child of boat & disable movement
            player.transform.parent = boat.transform;
            //player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerMovement>().movementSpeed = 0;
            player.GetComponent<PlayerMovement>().rotateSpeed = 0;


            // begin sailing
            Invoke("StartSailing", 0);

            // speak to player
            Invoke("Chat1", 2);

            Invoke("Chat2", 4);

            Invoke("Chat3", 8);

            Invoke("Chat4", 12);

            // teach song
            Invoke("TeachSong", 16);




        }
    }

    void StartSailing()
    {
        // start rowing
        anim.SetBool("Rowing", true);

        // begin sailing
        sailing = true;
    }

    void Chat1()
    {
        chatterCharon.ModifyChatter(BubbleType.Speech, "A close one there, huh.", true);
    }

    void Chat2()
    {
        chatterCharon.ModifyChatter(BubbleType.Speech, "What brings you down to the Styx?", true);
    }

    void Chat3()
    {
        chatterPlayer.ModifyChatter(BubbleType.Speech, "My wife died unexpectedly. I have to go save her.", true);
    }

    void Chat4()
    {
        chatterPlayer.ModifyChatter(BubbleType.Speech, "", true);
        chatterCharon.ModifyChatter(BubbleType.Speech, "Well, you'll need a different tune to survive down here...", true);
    }

    void TeachSong()
    {
        // teach player new song on way
        
        
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
