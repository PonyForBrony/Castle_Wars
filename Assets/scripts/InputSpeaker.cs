using System;
using System.Collections.Generic;
using UnityEngine;

public class InputSpeaker : MonoBehaviour
{

    public static List<KeyCode> input;
    public static List<KeyCode> ignoreMouse;
    public static List<KeyCode> ignoreKeyboard;
    static List<InputListener> listeners;

    private void Awake()
    {
        input = new List<KeyCode>();
        ignoreMouse = new List<KeyCode>();
        ignoreKeyboard = new List<KeyCode>();
        listeners = new List<InputListener>();
    }


    void OnGUI()
    {

        foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
        {
            bool keyDown = Input.GetKey(key);

            if (keyDown && !input.Contains(key))
            {
                onKeyDown(key);
            }
            else if (!keyDown && input.Contains(key))
            {
                onKeyUp(key);
            }

        }

        /*string a = "";
        foreach(KeyCode k in input)
        {
            a += k.ToString() + "  ";
        }
        Debug.Log(a); //debugging pressed keys */
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            onMouseScroll(Input.mouseScrollDelta.magnitude);
    }

    bool isMouseCode(KeyCode key)
    {
        return ((int)key > 322 && (int)key < 330);
    }

    private void onKeyDown(KeyCode key)
    {
        addToInput(key);
        foreach (InputListener z in listeners)
            z.onKeyDown(key);

        //Debug.Log("Keyboard down");
    }

    private void onKeyUp(KeyCode key)
    {
        removeFromInput(key);
        foreach (InputListener z in listeners)
            z.onKeyUp(key);

        //Debug.Log("Keyboard up");
    }

    private void onMouseScroll(float delta)
    {
        foreach (InputListener z in listeners)
            z.onMouseScroll(delta);

        //Debug.Log("Mouse scroll");
    }

    public static void addToListeners(InputListener listener)
    {
        if (listener != null)
        {
            listeners.Add(listener);
            //Debug.Log("listener added");
        }
    }

    void addToInput(KeyCode key)
    {
        input.Add(key);
        if (isMouseCode(key))
            ignoreKeyboard.Add(key);
        else
            ignoreMouse.Add(key);
    }

    void removeFromInput(KeyCode key)
    {
        input.Remove(key);
        if (isMouseCode(key))
            ignoreKeyboard.Remove(key);
        else
            ignoreMouse.Remove(key);
    }
}

public interface InputListener
{
    void onKeyDown(KeyCode key);
    void onKeyUp(KeyCode key);
    void onMouseScroll(float delta);
}