using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lyre : MonoBehaviour
{
    public GameObject mockLyre;

    public void Equip()
    {
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) r.enabled = true;
        foreach (MeshRenderer r in mockLyre.GetComponentsInChildren<MeshRenderer>()) r.enabled = false;
    }

    public void Dequip()
    {
        foreach (MeshRenderer r in mockLyre.GetComponentsInChildren<MeshRenderer>()) r.enabled = true;
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) r.enabled = false;
    }
}
