using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Singleton<PlayerController>
{
    protected PlayerController() { }

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
        if (Physics.Raycast(transform.position, m_arrow.transform.forward, out hit, 100f, 1 << LayerMask.NameToLayer("target")))
        {
            GameManager.SetRun();
            StartCoroutine(Move(hit));
        }
    }

    IEnumerator Move(RaycastHit hit) 
    {
        while (transform.position != hit.point)
        {
            transform.position = Vector3.Lerp(transform.position, hit.point, Time.fixedDeltaTime * 0.3f);
            if (Mathf.Abs(transform.position.z - hit.point.z) < 2f)
                break;
            yield return null;
        }
        GameManager.SetAim();
    }

    void RotateArrow() 
    {
        m_arrow.transform.Rotate(Vector3.up, 5);
    }
}
