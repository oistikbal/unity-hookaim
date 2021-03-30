using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Singleton<PlayerController>
{
    protected PlayerController() { }
    Vector3 m_velocityBuffer = new Vector3();

    void Awake()
    {
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
        if (GameManager.IsAim()  && Physics.Raycast(transform.position, Arrow.Instance.transform.forward, out hit, 100f, mask))
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
        transform.rotation = Quaternion.LookRotation(hit.point);
    }

}
