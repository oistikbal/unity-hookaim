using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager
{
    protected EventManager() { }
    static Dictionary<string, List<Action>> m_actions = new Dictionary<string, List<Action>>();

    public static void CreateEvent(string eventName)
    {
        m_actions[eventName] = new List<Action>();
    }

    public static void DeleteEvent(string eventName) 
    {
        m_actions[eventName].Clear();
        m_actions[eventName] = null;
    }

    public static void RegisterEvent(string eventName, Action action) 
    {
        if(!m_actions[eventName].Contains(action))
            m_actions[eventName].Add(action);
    }

    public static void Invoke(string eventName) 
    {
        foreach (var func in m_actions[eventName])
            func();
    }
}
