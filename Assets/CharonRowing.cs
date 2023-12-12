using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharonRowing : MonoBehaviour
{
    public bool rowing = false;
    void Row(int isRowing)
    {
        if (isRowing == 1)
        {
            rowing = true;
        } else
        {
            rowing = false;
        }
    }
}
