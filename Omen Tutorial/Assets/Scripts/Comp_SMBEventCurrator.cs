using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_SMBEventCurrator : MonoBehaviour
{
    [SerializeField] private bool debug = false;

    private List<IGameEventListener<string>> listeners = new List<IGameEventListener<string>>();

    public void Raise(string eventName)
    {
        if (debug)
            Debug.Log($"{eventName}");

        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(eventName);
    }
    public void RegisterListener(IGameEventListener<string> listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }
    public void UnregisterListener(IGameEventListener<string> listener)
    {
        listeners.Remove(listener);
    }      
}
