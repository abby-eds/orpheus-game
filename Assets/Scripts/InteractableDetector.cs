using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    public float detectRadius = 5;
    private List<Charmable> charmables = new List<Charmable>();
    private List<Spectral> spectrals = new List<Spectral>();
    private List<Sculptable> sculptables = new List<Sculptable>();

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = detectRadius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Charmable charmable = other.GetComponent<Charmable>();
        Spectral spectral = other.GetComponent<Spectral>();
        Sculptable sculptable = other.GetComponent<Sculptable>();
        if (charmable != null)
        {
            charmables.Add(charmable);
            UIManager.UI.AddHealthbar(charmable);
        }
        if (spectral != null) spectrals.Add(spectral);
        if (sculptable != null) sculptables.Add(sculptable);
    }

    private void OnTriggerExit(Collider other)
    {
        Charmable charmable = other.GetComponent<Charmable>();
        Spectral spectral = other.GetComponent<Spectral>();
        Sculptable sculptable = other.GetComponent<Sculptable>();
        if (charmable != null)
        {
            charmables.Remove(charmable);
            UIManager.UI.RemoveHealthbar(charmable);
        }
        if (spectral != null) spectrals.Add(spectral);
        if (sculptable != null) sculptables.Add(sculptable);
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
