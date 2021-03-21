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

    void Update()
    {
        PlayerInput();
    }

    void FixedUpdate()
    {
        RotateArrow();        
    }

    void PlayerInput() 
    {
        if (Input.touchCount > 0)
        {
            Shoot();
        }

    }

    void Shoot() 
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, m_arrow.transform.forward, out hit, 100f, 1 << LayerMask.NameToLayer("target")))
            StartCoroutine(Move(hit));
    }

    IEnumerator Move(RaycastHit hit) 
    {
        while (transform.position != hit.point)
        {
            transform.position = Vector3.MoveTowards(transform.position, hit.point, Time.fixedDeltaTime);
            yield return null;
        }
    }

    void RotateArrow() 
    {
        m_arrow.transform.Rotate(Vector3.up, 5);
    }
}
