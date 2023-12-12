using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism_in : MonoBehaviour
{
    [SerializeField]
    public bool showWires = true;
    [SerializeField]
    public Color wireColor = Color.white;
    [SerializeField]
    private Mechanism_out[] outputs;
    // Update is called once per frame
    void Update()
    {
        if(trigger())
        {
            foreach (Mechanism_out o in outputs)
            {
                o.activate();
            }
        }
    }

    public virtual bool trigger()
    {
        Debug.Log("default input (always on)");
        return true;
    }

    public virtual void OnDrawGizmos()
    {
        if (showWires)
        {
            Gizmos.color = wireColor;
            foreach (Mechanism_out o in outputs)
            {
                Gizmos.DrawLine(transform.position, o.transform.position);
            }
        }
    }
}
