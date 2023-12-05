using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static NoteData;

public class RingMusic : MonoBehaviour
{
    [Header("Object References")]
    public GameObject ring;
    public GameObject indicator;
    public GameObject ringBurstOkPrefab;
    public GameObject ringBurstGreatPrefab;
    public GameObject ringBurstPerfectPrefab;
    public GameObject song1Orb;
    public GameObject song2Orb;
    public GameObject song3Orb;
    public GameObject song1Empty;
    public GameObject song2Empty;
    public GameObject song3Empty;
    public Image streakMeterFilled;
    public Image streakMeterEmpty;
    private float streakMeterFillAmount = 0.14f;
    public Image streakMask;
    public Image level1Orb;
    public Image level2Orb;
    public Image level3Orb;
    public Sprite levelOrbFilled;
    public Sprite levelOrbEmpty;
    public GameObject notePrefab;
    public AudioSource backgroundSong;
    public AudioSource instrumentSong;
    public AudioClip[] backgroundSongs;
    public AudioClip[] instrumentSongs;
    private Animator anim;
    public Animator lyreAnim;
    public GameObject lyre;
    private PlayerHealth playerHealth;
    private InteractableDetector interactions;
    public float ringRadius = 100;

    [Header("Song Settings")]
    public float[] songDurations;
    private float songDuration;
    private float songTime;
    private float delay;
    private float songVolume = 0;

    private List<Note> notes = new List<Note>();
    private Note finished = null;
    private int noteIndex;

    public int numSongs;
    private List<NoteData>[] songs;
    public List<Color> songColors;
    public List<Sprite> songIcons;
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
    private int level2Threshold;
    private int level3Threshold;
    private int streakMax;
    public int songLevel { get; private set; }

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
        songs[0] = new List<NoteData>()
        {
            new NoteData(NoteType.Any, 0f, true),
            new NoteData(NoteType.Any, 0.6f, true),
            new NoteData(NoteType.Any, 1.8f, true),
            new NoteData(NoteType.Any, 2.4f, true),
        };
        songDurations[0] = 3.6f;
        songs[1] = new List<NoteData>()
        {
            new NoteData(NoteType.Any, 0f, true),
            new NoteData(NoteType.Any, 0.25f, true),
            new NoteData(NoteType.Any, 0.5f, true),
            new NoteData(NoteType.Any, 1f, true),
            new NoteData(NoteType.Any, 1.5f, true),
            new NoteData(NoteType.Any, 2.0f, true),
            new NoteData(NoteType.Any, 2.25f, true),
            new NoteData(NoteType.Any, 2.5f, true),
            new NoteData(NoteType.Any, 3f, true),
            new NoteData(NoteType.Any, 3.5f, true),
        };
        songDurations[1] = 4.0f;
        level2Threshold = 8 * songs[songIndex].Count;
        level3Threshold = 16 * songs[songIndex].Count;
        streakMax = level3Threshold;
        songDuration = songDurations[songIndex];
        song1Orb.SetActive(songIndex == 0 && numSongs >= 1);
        song2Orb.SetActive(songIndex == 1 && numSongs >= 2);
        song3Orb.SetActive(songIndex == 2 && numSongs >= 3);
        song1Empty.SetActive(songIndex != 0 && numSongs >= 1);
        song2Empty.SetActive(songIndex != 1 && numSongs >= 2);
        song3Empty.SetActive(songIndex != 2 && numSongs >= 3);
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
        bool streakEnabled = (songIndex == 0 && interactions.CharmablesInRange())
                          || (songIndex == 1 && interactions.SpectralsInRange())
                          || (songIndex == 2 && interactions.SculptablesInRange());
        switch (quality)
        {
            case NoteQuality.Perfect: streak += streakPerfect; break;
            case NoteQuality.Great: streak += streakGreat; break;
            case NoteQuality.Ok: streak += streakOk; break;
            case NoteQuality.Early: streak += streakEarly; break;
            case NoteQuality.Late: streak += streakLate; break;
            case NoteQuality.Wrong: streak += streakWrong; break;
        }
        if (streak < 0) streak = 0;
        if (streak > streakMax) streak = streakMax;
        if (streak >= level3Threshold) songLevel = 3;
        else if (streak >= level2Threshold) songLevel = 2;
        else if (streak > 0) songLevel = 1;
        else songLevel = 0;
        if (songLevel == 3 && songIndex == 0) playerHealth.Regen();
        else playerHealth.CancelRegen();
        UpdateStreakMeter();

        if (songLevel > 0) songVolume = 0.4f + 0.2f * songLevel;
        else songVolume = 0;

        switch (quality)
        {
            case NoteQuality.Perfect: Debug.Log("<color=yellow><b>Perfect!</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Great: Debug.Log("<color=green><b>Great</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Ok: Debug.Log("<color=blue><b>Ok</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Early: Debug.Log("<color=red><b>Early</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Late: Debug.Log("<color=red><b>Late</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
            case NoteQuality.Wrong: Debug.Log("<color=red><b>Wrong Note</b></color>\nStreak: " + streak + " | Song Level: " + songLevel); break;
        }
        if (songLevel > 0 && !playerHealth.dead)
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

    private void UpdateStreakMeter()
    {
        if (streak < level2Threshold) streakMeterFillAmount = 0.14f * (1 + ((float)streak / (level2Threshold)));
        else if (streak <= level3Threshold) streakMeterFillAmount = 0.14f * (1 + ((float)(streak - (level2Threshold)) / ((level3Threshold) - (level2Threshold))));
        level1Orb.sprite = songLevel >= 1 ? levelOrbFilled : levelOrbEmpty;
        level2Orb.sprite = songLevel >= 2 ? levelOrbFilled : levelOrbEmpty;
        level3Orb.sprite = songLevel >= 3 ? levelOrbFilled : levelOrbEmpty;
    }

    private void RefreshSong()
    {
        songTime = 0;
        songLevel = 0;
        streak = 0;
        delay = 0f;
        noteIndex = 0;
        RemoveAllNotes();
        UpdateStreakMeter();
        backgroundSong.Stop();
        instrumentSong.Stop();
        backgroundSong.clip = backgroundSongs[songIndex];
        instrumentSong.clip = instrumentSongs[songIndex];
        backgroundSong.volume = 0;
        instrumentSong.volume = 0;
        songVolume = 0;
        backgroundSong.Play();
        instrumentSong.Play();
        indicator.GetComponent<Image>().color = songColors[songIndex];
        streakMeterEmpty.color = songColors[songIndex];
        streakMeterFilled.color = songColors[songIndex];
        level1Orb.color = songColors[songIndex];
        level2Orb.color = songColors[songIndex];
        level3Orb.color = songColors[songIndex];
        songDuration = songDurations[songIndex];
        level2Threshold = 8 * songs[songIndex].Count;
        level3Threshold = 16 * songs[songIndex].Count;
        streakMax = level3Threshold;
        song1Orb.SetActive(songIndex == 0 && numSongs >= 1);
        song2Orb.SetActive(songIndex == 1 && numSongs >= 2);
        song3Orb.SetActive(songIndex == 2 && numSongs >= 3);
        song1Empty.SetActive(songIndex != 0 && numSongs >= 1);
        song2Empty.SetActive(songIndex != 1 && numSongs >= 2);
        song3Empty.SetActive(songIndex != 2 && numSongs >= 3);
        anim.SetBool("Playing Song", false);
        lyreAnim.SetBool("IsPlaying", false);
    }

    public void LearnSong()
    {
        numSongs++;
        song1Orb.SetActive(songIndex == 0 && numSongs >= 1);
        song2Orb.SetActive(songIndex == 1 && numSongs >= 2);
        song3Orb.SetActive(songIndex == 2 && numSongs >= 3);
        song1Empty.SetActive(songIndex != 0 && numSongs >= 1);
        song2Empty.SetActive(songIndex != 1 && numSongs >= 2);
        song3Empty.SetActive(songIndex != 2 && numSongs >= 3);
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
            // Will be used to display a burst
            GameObject ringBurst = null;
            if (value < leewayPerfect)
            {
                HitNote(NoteQuality.Perfect);
                ringBurst = Instantiate(ringBurstPerfectPrefab, indicator.transform);
            }
            else if (value < leewayGreat)
            {
                HitNote(NoteQuality.Great);
                ringBurst = Instantiate(ringBurstGreatPrefab, indicator.transform);
            }
            else if (value < leewayOk)
            {
                HitNote(NoteQuality.Ok);
                ringBurst = Instantiate(ringBurstOkPrefab, indicator.transform);
            }
            if (hitNote.loop) RefreshNote(hitNote);
            else RemoveNote(hitNote);
            ringBurst.GetComponent<Image>().color = songColors[songIndex];
        }
        else HitNote(NoteQuality.Early);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0 && numSongs > 0)
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
                songTime = backgroundSong.time % songDuration;
                if (noteIndex < songs[songIndex].Count && songTime >= songs[songIndex][noteIndex].delay)
                {
                    Note p = new Note(songs[songIndex][noteIndex], songDuration);
                    notes.Add(p);
                    p.visual = Instantiate(notePrefab, ring.transform);
                    p.icon = p.visual.transform.GetChild(0).GetComponent<Image>();
                    p.visual.GetComponent<Image>().color = songColors[songIndex];
                    p.icon.sprite = songIcons[songIndex];
                    p.icon.color = songColors[songIndex];
                    noteIndex++;
                }
            }

            // Update time for all notes
            foreach (Note p in notes)
            {
                if (p.currentTime < songDuration / 2)
                {
                    p.currentTime = (songTime - p.delay + songDuration) % songDuration;
                    p.currentTime = (p.currentTime + songDuration / 4) % songDuration - songDuration / 4;
                }
                else
                {
                    p.currentTime = (songTime - p.delay + songDuration) % songDuration;
                    p.currentTime = (p.currentTime + songDuration * 3 / 4) % songDuration + songDuration / 4;
                }
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
                    if (Input.GetMouseButtonDown(0))
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
        if (backgroundSong.volume < 0.8f)
        {
            backgroundSong.volume += 0.1f * Time.deltaTime;
            if (backgroundSong.volume > 0.8f) backgroundSong.volume = 0.8f;
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
        if (streakMask.fillAmount > streakMeterFillAmount + 0.1f) streakMask.fillAmount = streakMeterFillAmount;
        if (streakMask.fillAmount < streakMeterFillAmount)
        {
            streakMask.fillAmount += 0.1f * Time.deltaTime;
            if (streakMask.fillAmount > streakMeterFillAmount) streakMask.fillAmount = streakMeterFillAmount;
        }
        if (streakMask.fillAmount > streakMeterFillAmount)
        {
            streakMask.fillAmount -= 0.1f * Time.deltaTime;
            if (streakMask.fillAmount < streakMeterFillAmount) streakMask.fillAmount = streakMeterFillAmount;
        }
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
