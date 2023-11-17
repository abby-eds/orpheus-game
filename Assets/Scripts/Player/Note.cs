using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoteData;

public class Note
{
    public NoteType noteType;
    public bool loop;

    public float loopTime;
    public float currentTime;

    public GameObject visual;
    public Color color;

    public Note(NoteType noteType, float loopTime, bool loop)
    {
        this.noteType = noteType;
        this.loopTime = loopTime;
        this.loop = loop;
    }
}
