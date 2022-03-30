using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScript : MonoBehaviour
{
    public bool IsAngry = false;

    public void ChangeBool()
    {
        IsAngry = !IsAngry;
    }
}
