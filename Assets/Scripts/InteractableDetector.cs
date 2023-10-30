using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour
{
    public float detectRadius = 5;
    private List<Enemy> enemies = new List<Enemy>();
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius = detectRadius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            enemies.Add(other.GetComponent<Enemy>());
            UIManager.UI.AddEnemy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemies.Remove(other.GetComponent<Enemy>());
            UIManager.UI.RemoveEnemy(other.gameObject);
        }
    }

    public void SongOfCharms(float power)
    {
        foreach(Enemy e in enemies)
        {
            e.ApplyCharm(power);
        }
    }
}
