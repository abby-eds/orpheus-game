using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UI { get; private set; }
    private List<Healthbar> healthbars = new List<Healthbar>();
    public PlayerHealth playerHealth;
    public GameObject healthbarParent;
    public GameObject healthbarPrefab;
    public AudioSource backgroundSong;
    public AudioSource instrumentSong;

    [Header("Menus")]
    public GameObject startScreen;
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject controlsMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    public bool onStartScreen;
    private bool paused;
    private bool backMenu;

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
        if (Input.GetKeyDown(KeyCode.Escape) && !playerHealth.dead)
        {
            if (backMenu) BackToPauseMenu();
            else if (paused && !onStartScreen) Unpause();
            else Pause();
        }
    }

    private void LateUpdate()
    {
        foreach (Healthbar h in healthbars)
        {
            h.UpdatePosition();
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

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        backgroundSong.Pause();
        instrumentSong.Pause();
        if (onStartScreen) startMenu.SetActive(true);
        else pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
        backgroundSong.UnPause();
        instrumentSong.UnPause();
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

    public void Retry()
    {
        SceneTransition.Transition.TransitionToScene(SceneManager.GetActiveScene().buildIndex);
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
