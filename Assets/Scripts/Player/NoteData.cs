using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData
{
    public enum NoteType
    {
        Space,
        Left,
        Right,
        Any
    }

    public NoteType noteType;
    public float delay;
    public bool loop;

    public NoteData(NoteType noteType, float delay, bool loop)
    {
        this.noteType = noteType;
        this.delay = delay;
        this.loop = loop;
    }
}
