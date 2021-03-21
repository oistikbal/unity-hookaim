using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Singleton<PlayerController>
{
    protected PlayerController() { }

    GameObject m_arrow;

    void Start()
    {
        m_arrow = GameObject.Find("Arrow");
    }

    void FixedUpdate()
    {
        RotateArrow();        
    }

    void RotateArrow() 
    {
        m_arrow.transform.Rotate(Vector3.up, 5);
    }
}
