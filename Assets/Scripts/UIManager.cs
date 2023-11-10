using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UI { get; private set; }
    private List<Healthbar> healthbars = new List<Healthbar>();
    public GameObject healthbarParent;
    public GameObject healthbarPrefab;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject controlsMenu;
    public GameObject settingsMenu;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (backMenu) BackToPauseMenu();
            else if (paused) Unpause();
            else Pause();
        }
        foreach(Healthbar h in healthbars)
        {
            h.transform.localPosition = (Camera.main.WorldToScreenPoint(h.charmable.transform.position + Vector3.up * 1.5f) - new Vector3(Screen.width / 2, Screen.height / 2, 0)) * 1920 / Screen.width;
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
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BackToPauseMenu()
    {
        backMenu = false;
        pauseMenu.SetActive(true);
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void ToControlsMenu()
    {
        backMenu = true;
        controlsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void ToSettingsMenu()
    {
        backMenu = true;
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
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
