using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager UI { get; private set; }
    public List<Healthbar> healthbars;
    public GameObject healthbarPrefab;

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
        foreach(Healthbar h in healthbars)
        {
            h.transform.localPosition = (Camera.main.WorldToScreenPoint(h.enemy.transform.position + Vector3.up * 1.5f) - new Vector3(Screen.width / 2, Screen.height / 2, 0)) * 1920 / Screen.width;
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        GameObject hbObject = Instantiate(healthbarPrefab, transform);
        Healthbar hb = hbObject.GetComponent<Healthbar>();
        hb.AssignEnemy(enemy.GetComponent<Enemy>());
        healthbars.Add(hb);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        foreach(Healthbar h in healthbars)
        {
            if (h.enemy == enemy.GetComponent<Enemy>())
            {
                healthbars.Remove(h);
                Destroy(h.gameObject);
                break;
            }
        }
    }
}
