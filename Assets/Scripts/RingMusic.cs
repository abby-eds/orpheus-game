using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NoteData;

public class RingMusic : MonoBehaviour
{
    public GameObject ring;
    public GameObject indicator;
    public GameObject notePrefab;

    public float songDuration;
    private float songTime;
    private float delay;

    public float leewayPerfect;
    public float leewayGreat;
    public float leewayOk;
    private int streak;
    public int streakMax;

    public int numNotes;
    private List<Note> notes = new List<Note>();
    private Note finished = null;
    private int noteIndex;

    public int numSongs;
    private List<NoteData>[] songs;
    public List<Color> songColors;
    private int songIndex;

    // Start is called before the first frame update
    void Start()
    {
        delay = 1;
        noteIndex = -1;
        songIndex = 0;
        songs = new List<NoteData>[numSongs];
        indicator.GetComponent<Image>().color = songColors[songIndex];
        songs[0] = new List<NoteData>();
        songs[1] = new List<NoteData>()
        {
            new NoteData(NoteType.Space, 0.5f, true),
            new NoteData(NoteType.Left, 0.25f, true),
            new NoteData(NoteType.Left, 0.25f, true),
            new NoteData(NoteType.Left, 0.5f, true),
            new NoteData(NoteType.Space, 0.5f, true),
            new NoteData(NoteType.Left, 0.25f, true),
            new NoteData(NoteType.Left, 0.25f, true),
            new NoteData(NoteType.Left, 0.5f, true)
        };
        songs[2] = new List<NoteData>()
        {
            new NoteData(NoteType.Space, 0.5f, true),
            new NoteData(NoteType.Left, 0.2f, true),
            new NoteData(NoteType.Left, 0.6f, true),
            new NoteData(NoteType.Space, 0.2f, true),
            new NoteData(NoteType.Space, 0.2f, true),
            new NoteData(NoteType.Space, 0.6f, true),
            new NoteData(NoteType.Right, 0.2f, true),
            new NoteData(NoteType.Right, 0.5f, true),
        };
        for (int i = 0; i < numNotes; i++)
        {
            songs[0].Add(new NoteData(NoteType.Space, songDuration / numNotes, true));
        }
    }

    /// <summary>
    ///  "Refreshes" a note, moving it to the back of the list and refreshing it's time
    /// </summary>
    private void RefreshNote(Note note)
    {
        notes.Remove(note);
        notes.Add(note);
        note.currentTime -= note.loopTime;
    }

    /// <summary>
    ///  Removes a note from the list of notes, and deletes its visual GameObject
    /// </summary>
    private void RemoveNote(Note note)
    {
        notes.Remove(note);
        Destroy(note.visual);
    }

    private void RemoveAllNotes()
    {
        foreach(Note note in notes) Destroy(note.visual);
        notes.Clear();
    }

    private void HitPerfect()
    {
        streak += 2;
        if (streak > streakMax) streak = streakMax;
        Debug.Log("<color=yellow><b>Perfect!</b></color>\nStreak: " + streak);
    }

    private void HitGreat()
    {
        streak += 1;
        if (streak > streakMax) streak = streakMax;
        Debug.Log("<color=green><b>Great</b></color>\nStreak: " + streak);
    }

    private void HitOk()
    {
        Debug.Log("<color=blue><b>Ok</b></color>\nStreak: " + streak);
    }

    private void MissEarly()
    {
        streak -= 5;
        if (streak < 0) streak = 0;
        Debug.Log("<color=red><b>Early</b></color>\nStreak: " + streak);
    }

    private void MissLate()
    {
        streak -= 5;
        if (streak < 0) streak = 0;
        Debug.Log("<color=red><b>Late</b></color>\nStreak: " + streak);
    }

    private void MissWrongNote()
    {
        streak -= 5;
        if (streak < 0) streak = 0;
        Debug.Log("<color=red><b>Wrong Note</b></color>\nStreak: " + streak);
    }

    private void RefreshSong()
    {
        songTime = 0;
        delay = 1;
        noteIndex = -1;
        RemoveAllNotes();
        indicator.GetComponent<Image>().color = songColors[songIndex];
    }

    /// <summary>
    ///  Moves a note in accordance with how much of it's loop time has passed
    /// </summary>
    private void MoveNote(Note note)
    {
        float radians = ((note.currentTime / note.loopTime) + 0.25f) * Mathf.PI * 2;
        note.visual.transform.localPosition = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * 100;
        if (note.currentTime > note.loopTime + leewayOk) finished = note;
    }

    private void PlayNote(Note hitNote)
    {
        // Calculate how close the player was to perfect timing
        float value = Mathf.Abs(hitNote.currentTime - hitNote.loopTime);
        if (value <= leewayOk)
        {
            if (value < leewayPerfect) HitPerfect();
            else if (value < leewayGreat) HitGreat();
            else if (value < leewayOk) HitOk();
            if (hitNote.loop) RefreshNote(hitNote);
            else RemoveNote(hitNote);
        }
        else MissEarly();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                songIndex++;
                if (songIndex >= numSongs) songIndex = 0;
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                songIndex--;
                if (songIndex < 0)songIndex = numSongs - 1;
            }
            RefreshSong();
        }

        // Update time for all notes
        foreach (Note p in notes)
        {
            p.currentTime += Time.deltaTime;
            MoveNote(p);
        }

        // If a note has "finished", handle it
        if (finished != null)
        {
            if (finished.loop) RefreshNote(finished);
            else RemoveNote(finished);
            MissLate();
            finished = null;
        }

        // Check for player input and play a note
        if (notes.Count > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Note hitNote = notes[0];
                if (hitNote.noteType == NoteType.Space) PlayNote(hitNote);
                else MissWrongNote();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Note hitNote = notes[0];
                if (hitNote.noteType == NoteType.Left) PlayNote(hitNote);
                else MissWrongNote();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Note hitNote = notes[0];
                if (hitNote.noteType == NoteType.Right) PlayNote(hitNote);
                else MissWrongNote();
            }
        }
        

        // Advance the overall song time and spawn notes as needed
        songTime += Time.deltaTime;
        if (songTime >= songDuration) songTime -= songDuration;
        if (noteIndex + 1 < songs[songIndex].Count)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                noteIndex++;
                Note p = new Note(songDuration, songs[songIndex][noteIndex].loop);
                notes.Add(p);
                p.visual = Instantiate(notePrefab, ring.transform);
                p.visual.GetComponent<FadeIn>().targetColor = songColors[songIndex];
                delay = songs[songIndex][noteIndex].delay;
            }
        }
    }
}
