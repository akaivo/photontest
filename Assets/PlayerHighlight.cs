using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHighlight : MonoBehaviour
{
    public bool isHighlighted;

    public void Set(bool value)
    {
        isHighlighted = value;
        GetComponent<Renderer>().material.color = value ? Color.red : Color.white;
    }
}
