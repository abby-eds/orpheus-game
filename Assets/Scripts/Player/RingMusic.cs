using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NoteData;

public class RingMusic : MonoBehaviour
{
    [Header("Object References")]
    public GameObject ring;
    public GameObject indicator;
    public GameObject notePrefab;
    public AudioSource backgroundSong;
    public AudioSource instrumentSong;
    private Animator anim;
    public Animator lyreAnim;
    public GameObject lyre;
    public GameObject mockLyre;
    private PlayerHealth playerHealth;
    private InteractableDetector interactions;
    public float ringRadius = 100;

    [Header("Song Settings")]
    public float[] songDurations;
    private float songDuration;
    private float songTime;
    private float delay;
    private float songVolume = 0;

    public int numNotes;
    private List<Note> notes = new List<Note>();
    private Note finished = null;
    private int noteIndex;

    public int numSongs;
    private List<NoteData>[] songs;
    public List<Color> songColors;
    private int songIndex;

    [Header("Leeway/Difficulty Settings")]
    public float leewayPerfect;
    public float leewayGreat;
    public float leewayOk;

    [Header("Streak Settings")]
    private int streak;
    public int streakPerfect;
    public int streakGreat;
    public int streakOk;
    public int streakWrong;
    public int streakEarly;
    public int streakLate;
    public int level2Threshold;
    public int level3Threshold;
    public int streakMax;
    private int songLevel;

    [Header("Song Multipliers")]
    public float charmMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        interactions = GetComponent<InteractableDetector>();
        delay = 1f;
        noteIndex = 0;
        songIndex = 0;
        songTime = -1;
        songs = new List<NoteData>[numSongs];
        songDurations = new float[numSongs];
        indicator.GetComponent<Image>().color = songColors[songIndex];
        indicator.transform.localPosition = new Vector2(0, ringRadius);
        indicator.SetActive(true);
        songs[0] = new List<NoteData>()
        {
            new NoteData(NoteType.Left, 0f, true),
            new NoteData(NoteType.Left, 0.6f, true),
            new NoteData(NoteType.Left, 1.8f, true),
            new NoteData(NoteType.Left, 2.4f, true),
        };
        songDurations[0] = 3.6f;
        songDuration = songDurations[songIndex];
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
        Destroy(note.visual);
        notes.Remove(note);
    }

    private void RemoveAllNotes()
    {
        foreach(Note note in notes) Destroy(note.visual);
        notes.Clear();
    }

    /// <summary>
    ///  Updates the player's streak based on note quality
    /// </summary>
    private void HitNote(NoteQuality quality)
    {
        int streakEnabled = (songIndex == 0 && interactions.CharmablesInRange())
                          || (songIndex == 1 && interactions.SpectralsInRange())
                          || (songIndex == 2 && interactions.SculptablesInRange())
                          || streak == 0 ? 1 : 0;
        switch (quality)
        {
            case NoteQuality.Perfect: streak += streakPerfect * streakEnabled; break;
            case NoteQuality.Great: streak += streakGreat * streakEnabled; break;
            case NoteQuality.Ok: streak += streakOk * streakEnabled; break;
            case NoteQuality.Early: streak += streakEarly; break;
            case NoteQuality.Late: streak += streakLate; break;
            case NoteQuality.Wrong: streak += streakWrong; break;
        }

        if (streak > streakMax) streak = streakMax;
        if (streak < 0) streak = 0;

        if (songLevel > 0) songVolume = 0.4f + 0.2f * songLevel;
        else songVolume = 0;
        

        if (streak >= level3Threshold) songLevel = 3;
        else if (streak >= level2Threshold) songLevel = 2;
        else if (streak > 0) songLevel = 1;
        else songLevel = 0;

        switch (quality)
        {
            case NoteQuality.Perfect: Debug.Log("<color=yellow><b>Perfect!</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Great: Debug.Log("<color=green><b>Great</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Ok: Debug.Log("<color=blue><b>Ok</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Early: Debug.Log("<color=red><b>Early</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Late: Debug.Log("<color=red><b>Late</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Wrong: Debug.Log("<color=red><b>Wrong Note</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
        }
        if (streak > 0 && !playerHealth.dead)
        {
            anim.SetBool("Playing Song", true);
            lyreAnim.SetBool("IsPlaying", true);
        }
        else
        {
            anim.SetBool("Playing Song", false);
            lyreAnim.SetBool("IsPlaying", false);
        }
    }

    private void RefreshSong()
    {
        songTime = 0;
        songLevel = 0;
        delay = 0f;
        noteIndex = 0;
        RemoveAllNotes();
        backgroundSong.Stop();
        instrumentSong.Stop();
        backgroundSong.Play();
        instrumentSong.Play();
        indicator.GetComponent<Image>().color = songColors[songIndex];
        songDuration = songDurations[songIndex];
    }

    /// <summary>
    ///  Moves a note in accordance with how much of it's loop time has passed
    /// </summary>
    private void MoveNote(Note note)
    {
        float radians = ((note.currentTime / note.loopTime) + 0.25f) * Mathf.PI * 2;
        note.visual.transform.localPosition = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * ringRadius;
        if (note.currentTime > note.loopTime + leewayOk) finished = note;
    }

    /// <summary>
    ///  Calculates what occurs when the player inputs a note
    /// </summary>
    private void PlayNote(Note hitNote)
    {
        // Calculate how close the player was to perfect timing
        float value = Mathf.Abs(hitNote.currentTime - hitNote.loopTime);
        if (value <= leewayOk)
        {
            if (value < leewayPerfect) HitNote(NoteQuality.Perfect);
            else if (value < leewayGreat) HitNote(NoteQuality.Great);
            else if (value < leewayOk) HitNote(NoteQuality.Ok);
            if (hitNote.loop) RefreshNote(hitNote);
            else RemoveNote(hitNote);
        }
        else HitNote(NoteQuality.Early);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
                if (delay <= 0)
                {
                    delay = 0;
                    backgroundSong.Play();
                    instrumentSong.Play();
                }
            }
            else
            {
                // Advance the overall song time and spawn notes as needed
                //songTime += Time.deltaTime;
                songTime = backgroundSong.time;
                //if (noteIndex >= 0) trackTime += Time.deltaTime;
                //if (songTime >= songDuration) songTime -= songDuration;
                //if (trackTime > songOfCharmsTrackDuration) trackTime -= songOfCharmsTrackDuration;
                //float offset = trackTime - backgroundSong.time;
                //Debug.Log(offset);
                if (noteIndex < songs[songIndex].Count && songTime >= songs[songIndex][noteIndex].delay)
                {
                    //delay -= Time.deltaTime;
                    //if (delay <= 0)
                    //{
                    //if (noteIndex == 0)
                    //{
                    //    backgroundSong.Play();
                    //    instrumentSong.Play();
                    //}
                    Note p = new Note(songs[songIndex][noteIndex].noteType, songDuration, songs[songIndex][noteIndex].loop);
                    notes.Add(p);
                    p.visual = Instantiate(notePrefab, ring.transform);
                    p.visual.GetComponent<FadeIn>().targetColor = songColors[songIndex];
                    noteIndex++;
                    //delay = songs[songIndex][noteIndex].delay;
                    //}
                }
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
                HitNote(NoteQuality.Late);
                finished = null;
            }

            if (!playerHealth.dead)
            {
                // Check for player input to switch songs
                if (Input.mouseScrollDelta.y != 0 && numSongs > 1)
                {
                    if (Input.mouseScrollDelta.y > 0)
                    {
                        songIndex++;
                        if (songIndex >= numSongs) songIndex = 0;
                    }
                    else if (Input.mouseScrollDelta.y < 0)
                    {
                        songIndex--;
                        if (songIndex < 0) songIndex = numSongs - 1;
                    }
                    RefreshSong();
                }

                // Check for player input and play a note
                if (notes.Count > 0)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Note hitNote = notes[0];
                        if (hitNote.noteType == NoteType.Space || hitNote.noteType == NoteType.Any) PlayNote(hitNote);
                        else HitNote(NoteQuality.Wrong);
                    }
                    else if (Input.GetMouseButtonDown(0))
                    {
                        Note hitNote = notes[0];
                        if (hitNote.noteType == NoteType.Left || hitNote.noteType == NoteType.Any) PlayNote(hitNote);
                        else HitNote(NoteQuality.Wrong);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        Note hitNote = notes[0];
                        if (hitNote.noteType == NoteType.Right || hitNote.noteType == NoteType.Any) PlayNote(hitNote);
                        else HitNote(NoteQuality.Wrong);
                    }
                }

                if (songLevel > 0)
                {
                    if (songIndex == 0) interactions.SongOfCharms((1 + songLevel) / 2f * charmMultiplier * Time.deltaTime);
                    else if (songIndex == 1) interactions.SongOfDead(songLevel);
                    else if (songIndex == 2) interactions.SongOfSculpting(songLevel);
                }
            }
        }
        if (instrumentSong.volume < songVolume)
        {
            instrumentSong.volume += Time.deltaTime;
            if (instrumentSong.volume > songVolume) instrumentSong.volume = songVolume;
        }
        if (instrumentSong.volume > songVolume)
        {
            instrumentSong.volume -= 0.2f * Time.deltaTime;
            if (instrumentSong.volume < songVolume) instrumentSong.volume = songVolume;
        }
        //instrumentSong.volume = Mathf.Lerp(instrumentSong.volume, songVolume, 0.02f);
    }

    enum NoteQuality
    {
        Late,
        Early,
        Wrong,
        Ok,
        Great,
        Perfect,
    }
}
