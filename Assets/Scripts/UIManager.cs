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
    private bool paused;

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
            paused = !paused;
            if (paused) Time.timeScale = 0;
            else Time.timeScale = 1;
            pauseMenu.SetActive(paused);
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
}
