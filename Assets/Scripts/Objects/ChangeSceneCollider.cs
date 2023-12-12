using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneCollider : MonoBehaviour
{
    [SerializeField]
    private int scene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SceneTransition.Transition.TransitionToScene(scene);
        }
    }
}
