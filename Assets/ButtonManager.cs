using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    private Dictionary<string, Action> m_buttons = new Dictionary<string, Action>();
    private float m_xpos, m_ypos;
    private float m_height = 45;
    private float m_spacing = 5f;

    public void AddButton(string buttonText, Action callback)
    {
        m_buttons[buttonText] = callback;
    }

    private void OnGUI()
    {
        m_xpos = 10f;
        m_ypos = 10f;
        foreach (var button in m_buttons)
        {
            if (GUI.Button(GetNextRect(),button.Key) && button.Value != null)
            {
                button.Value.Invoke();
            };
        }
    }

    private Rect GetNextRect()
    {
        Rect r = new Rect(m_xpos, m_ypos, 200, m_height);
        m_ypos += m_height + m_spacing;
        return r;
    }
}