using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    public float detectRadius = 5;
    private List<Charmable> charmables = new List<Charmable>();
    private List<Spectral> spectrals = new List<Spectral>();
    private List<Sculptable> sculptables = new List<Sculptable>();
    private List<Chatter> chatters = new List<Chatter>();

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = detectRadius;
        Chatter chatter = GetComponent<Chatter>();
        if (chatter != null)
        {
            chatters.Add(chatter);
            UIManager.UI.AddTextBubble(chatter);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Chatter c in chatters)
        {
            if (c.modified)
            {
                UIManager.UI.RemoveTextBubble(c);
                UIManager.UI.AddTextBubble(c);
                c.modified = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Charmable charmable = other.GetComponent<Charmable>();
        Spectral spectral = other.GetComponent<Spectral>();
        Sculptable sculptable = other.GetComponent<Sculptable>();
        Chatter chatter = other.GetComponent<Chatter>();
        if (charmable != null)
        {
            charmables.Add(charmable);
            UIManager.UI.AddHealthbar(charmable);
        }
        if (spectral != null) spectrals.Add(spectral);
        if (sculptable != null) sculptables.Add(sculptable);
        if (chatter != null)
        {
            chatters.Add(chatter);
            UIManager.UI.AddTextBubble(chatter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Charmable charmable = other.GetComponent<Charmable>();
        Spectral spectral = other.GetComponent<Spectral>();
        Sculptable sculptable = other.GetComponent<Sculptable>();
        Chatter chatter = other.GetComponent<Chatter>();
        if (charmable != null)
        {
            charmables.Remove(charmable);
            UIManager.UI.RemoveHealthbar(charmable);
        }
        if (spectral != null) spectrals.Remove(spectral);
        if (sculptable != null) sculptables.Remove(sculptable);
        if (chatter != null)
        {
            chatters.Remove(chatter);
            UIManager.UI.RemoveTextBubble(chatter);
        }
    }

    public bool CharmablesInRange()
    {
        return !(charmables.Count == 0);
    }

    public bool SpectralsInRange()
    {
        return !(spectrals.Count == 0);
    }

    public bool SculptablesInRange()
    {
        return !(sculptables.Count == 0);
    }

    public void SongOfCharms(float power)
    {
        foreach(Charmable c in charmables)
        {
            c.ApplySongOfCharms(power);
        }
    }

    public void SongOfDead(int level)
    {
        foreach(Spectral s in spectrals)
        {
            s.ApplySongOfDead(level);
        }
    }

    public void SongOfSculpting(int level)
    {
        foreach(Sculptable s in sculptables)
        {
            s.ApplySongOfSculpting(level);
        }
    }
}
