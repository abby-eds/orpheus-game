using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persephone : MonoBehaviour
{

    private GameObject Orpheus;
    private bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player") && (!other.isTrigger) && (activated == false)){
            Debug.Log("Persephone is triggered");
            Orpheus = other.gameObject;
            
            Invoke("Chat1", 0);
            Invoke("Chat2", 2);
            Invoke("Chat3", 5);
            Invoke("Chat4", 10);
            Invoke("Chat5", 14);
            Invoke("Chat6", 18);
            Invoke("Chat7", 22);
            Invoke("Chat8", 26);
            Invoke("Chat9", 28);
            Invoke("Chat10", 32);

            Invoke("songOfStone", 32);

            Invoke("Chat11", 42);
            Invoke("Chat12", 44);
            Invoke("Chat13", 50);
            
            activated = true;
        }
    }

    void Chat1(){
        Orpheus.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "Who are you?", false);
    }
    void Chat2(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "I am Persephone, Goddess of spring.", true);
    }

    void Chat3(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech,"I have heard of your travels. What do you seek?", true);
    }

    void Chat4(){
        Orpheus.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        Orpheus.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "I am looking for my Fiance Eurydice...", false);
    }

    void Chat5(){
        Orpheus.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "Ah, you wish to speak with my husband.", true);
    }
    void Chat6(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "Your love must be very strong to travel so far...", true);
    }
    void Chat7(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "...but my husband is not so easily swayed", true);
    }
    void Chat8(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "Follow my lead...", true);
    }
        void Chat9(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "The song I sing sways trees and moves mountains", true);
    }

    void Chat10(){
        Orpheus.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
    }
    void Chat11(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "Very Good!!", true);
    }
    void Chat12(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "My Husband is beyond the double doors outside.", true);
    }
    void Chat13(){
        gameObject.GetComponent<Chatter>().ModifyChatter(BubbleType.Speech, "", false);
    }

    void songOfStone(){
        Orpheus.GetComponent<RingMusic>().LearnSong();
    }
}