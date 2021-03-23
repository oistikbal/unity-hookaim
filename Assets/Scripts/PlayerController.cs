using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Singleton<PlayerController>
{
    protected PlayerController() { }
    Vector3 m_velocityBuffer = new Vector3();

    public static GameObject m_arrow;

    void Awake()
    {
        m_arrow = GameObject.Find("Arrow");
    }

    void Start()
    {

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
        int mask = 1 << LayerMask.NameToLayer("enemy") | 1 << LayerMask.NameToLayer("box");
        if (GameManager.IsAim()  && Physics.Raycast(transform.position, m_arrow.transform.forward, out hit, 100f, mask))
        {
            StartCoroutine(Move(hit));
        }
    }

    IEnumerator Move(RaycastHit hit) 
    {
        GameManager.SetRun();
        while (transform.position != hit.point)
        {
            transform.position = Vector3.SmoothDamp(transform.position, hit.point, ref m_velocityBuffer, Time.fixedDeltaTime, 10f);
            if (Mathf.Abs(transform.position.z - hit.point.z) < 2f)
                break;
            yield return null;
        }
        GameManager.SetAim();
        transform.rotation = hit.transform.rotation;
    }

    void RotateArrow() 
    {
        m_arrow.transform.Rotate(Vector3.up, 5);
    }
}
