using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Transition { get; private set; }
    public Image fadePanel;
    private float fadePercent;
    private bool fadeIn;
    private int sceneIndex;

    private void Awake()
    {
        if (Transition != null && Transition != this)
        {
            Destroy(this);
        }
        else
        {
            Transition = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fadePercent = 1;
        sceneIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn && fadePercent < 1)
        {
            fadePercent += Time.deltaTime;
            if (fadePercent > 1)
            {
                fadePercent = 1;
                SceneManager.LoadScene(sceneIndex);
            }
        }
        else if (!fadeIn && fadePercent > 0)
        {
            fadePercent -= Time.deltaTime;
            if (fadePercent < 0)
            {
                fadePercent = 0;
                fadePanel.gameObject.SetActive(false);
            }
        }
        fadePanel.color = new Color(0, 0, 0, fadePercent);
    }

    public void TransitionToScene(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
        fadePanel.gameObject.SetActive(true);
        fadeIn = true;
    }

    public void TransitionToScene(string sceneName)
    {
        sceneIndex = SceneManager.GetSceneByName(sceneName).buildIndex;
        fadePanel.gameObject.SetActive(true);
        fadeIn = true;
    }
}