using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NoteData;

public class Note
{
    public NoteType noteType;
    public bool loop;

    public float loopTime;
    public float currentTime;
    public float delay;

    public GameObject visual;
    public Image icon;
    public Color color;

    public Note(NoteData note, float loopTime)
    {
        noteType = note.noteType;
        this.loopTime = loopTime;
        loop = note.loop;
        delay = note.delay;
    }
}
