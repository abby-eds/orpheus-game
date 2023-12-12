using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UI { get; private set; }
    private List<Healthbar> healthbars = new List<Healthbar>();
    private List<TextBubble> textBubbles = new List<TextBubble>();
    public PlayerHealth playerHealth;
    public GameObject healthbarParent;
    public GameObject healthbarPrefab;
    public GameObject textBubbleParent;
    public GameObject textBubblePrefab;
    public AudioSource[] songs;

    [Header("Menus")]
    public GameObject startScreen;
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject controlsMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    public GameObject EToInteract;
    public GameObject toBeContinued;
    public bool onStartScreen;
    private bool paused;
    private bool backMenu;
    private bool winGame;

    private void Awake()
    {
        if (UI != null && UI != this)
        {
            Destroy(this);
        }
        else
        {
            UI = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        if (onStartScreen)
        {
            Pause();
            startScreen.SetActive(true);
            startMenu.SetActive(true);
        }
        else
        {
            startScreen.SetActive(false);
            startMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !playerHealth.dead && !winGame)
        {
            if (backMenu) BackToPauseMenu();
            else if (paused && !onStartScreen) Unpause();
            else Pause();
        }
    }

    private void LateUpdate()
    {
        if (Time.timeScale > 0)
        {
            foreach (Healthbar h in healthbars)
            {
                h.UpdatePosition();
            }
            foreach (TextBubble t in textBubbles)
            {
                t.UpdatePosition();
            }
        }
    }

    public void AddHealthbar(Charmable charmable)
    {
        GameObject hbObject = Instantiate(healthbarPrefab, healthbarParent.transform);
        Healthbar hb = hbObject.GetComponent<Healthbar>();
        hb.AssignEnemy(charmable);
        healthbars.Add(hb);
    }

    public void RemoveHealthbar(Charmable charmable)
    {
        foreach(Healthbar h in healthbars)
        {
            if (h.charmable == charmable)
            {
                healthbars.Remove(h);
                Destroy(h.gameObject);
                break;
            }
        }
    }

    public void AddTextBubble(Chatter chatter)
    {
        if (chatter.text != "")
        {
            GameObject tbObject = Instantiate(textBubblePrefab, textBubbleParent.transform);
            TextBubble tb = tbObject.GetComponent<TextBubble>();
            tb.AssignChatter(chatter);
            textBubbles.Add(tb);
        }
    }

    public void RemoveTextBubble(Chatter chatter)
    {
        foreach(TextBubble t in textBubbles)
        {
            if (t.chatter == chatter)
            {
                textBubbles.Remove(t);
                Destroy(t.gameObject);
                break;
            }
        }
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        foreach(AudioSource s in songs) s.Pause();
        if (onStartScreen) startMenu.SetActive(true);
        else pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
        foreach (AudioSource s in songs) s.UnPause();
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartGame()
    {
        startMenu.SetActive(false);
        onStartScreen = false;
        startScreen.SetActive(false);
        Unpause();
    }

    public void BackToPauseMenu()
    {
        backMenu = false;
        if (onStartScreen) startMenu.SetActive(true);
        else pauseMenu.SetActive(true);
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void ToControlsMenu()
    {
        backMenu = true;
        controlsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        startMenu.SetActive(false);
    }

    public void ToSettingsMenu()
    {
        backMenu = true;
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        startMenu.SetActive(false);
    }

    public void ToGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void InteractActive(bool active)
    {
        EToInteract.SetActive(active);
    }

    public void Retry()
    {
        SceneTransition.Transition.TransitionToScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToBeContinued()
    {
        SceneTransition.Transition.ToBeContinued();
        winGame = true;
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
