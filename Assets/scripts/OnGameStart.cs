// Create a non-MonoBehaviour class which displays
// messages when a game is loaded.
using UnityEngine;

class MyClass
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        GameObject tmp = Object.Instantiate(new GameObject());
        tmp.name = "lalala";
        tmp.AddComponent<InputSpeaker>();
        GameObject.Destroy(GameObject.Find("New Game Object")); //instantiate generates two objects instead one
    }
}