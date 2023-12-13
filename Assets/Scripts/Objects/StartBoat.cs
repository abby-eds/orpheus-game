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
    private CharonRowing charonRowing;
    public bool sailing = false;
    private bool hasRowed = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boat = this.gameObject;
        charon = GameObject.Find("SitCharon");
        anim = charon.GetComponent<Animator>();
        chatterCharon = charon.GetComponent<Chatter>();
        chatterPlayer = player.GetComponent<Chatter>();
        charonRowing = charon.GetComponent<CharonRowing>();
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
            player.GetComponent<PlayerMovement>().jumpHeight = 0;


            // begin sailing
            Invoke("StartSailing", 0);

            // speak to player
            Invoke("Chat1", 2);

            Invoke("Chat2", 4);

            Invoke("Chat3", 8);

            Invoke("Chat4", 12);

            // teach song
            Invoke("TeachSong", 16);

            Invoke("Chat5", 20);

            Invoke("Chat6", 24);

            Invoke("Chat7", 28);

            Invoke("Chat8", 36);

            Invoke("Chat9", 40);


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
        chatterCharon.ModifyChatter(BubbleType.Speech, "", true);
        chatterPlayer.ModifyChatter(BubbleType.Speech, "My wife died unexpectedly. I have to go save her.", false);
    }

    void Chat4()
    {
        chatterPlayer.ModifyChatter(BubbleType.Speech, "", true);
        chatterCharon.ModifyChatter(BubbleType.Speech, "Well, you'll<br>need a different<br>tune to survive<br>down here...", true);
    }

    void Chat5() 
    {
        chatterCharon.ModifyChatter(BubbleType.Speech, "Try something a little more... spiritual...", true);
    }

    void Chat6()
    {
        chatterCharon.ModifyChatter(BubbleType.Speech, "It might just bring you light in a dark time.", true);
    }

    void Chat7()
    {
        chatterCharon.ModifyChatter(BubbleType.Speech, "", true);
    }

    void Chat8()
    {
        chatterPlayer.ModifyChatter(BubbleType.Speech, "Euridice... I'm on my way... hang in there.", false);
    }
    void Chat9()
    {
        chatterPlayer.ModifyChatter(BubbleType.Speech, "", true);
    }

    void TeachSong()
    {
        // teach player new song on way
        chatterCharon.ModifyChatter(BubbleType.Speech, "", true);
        player.GetComponent<RingMusic>().LearnSong();
    }

    void FixedUpdate()
    {
        if (sailing)
        {
            if (hasRowed)
            {
                boat.transform.Translate(0.05f, 0, 0);
            }
            
            if (charonRowing.rowing)
            {
                hasRowed = true;
                boat.transform.Translate(0.05f, 0, 0);
            }
        }


    }
}
