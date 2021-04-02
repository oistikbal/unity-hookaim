using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Singleton<PlayerController>
{
    protected PlayerController() { }
    Vector3 m_velocityBuffer;

    public GameObject playerModel 
    {
        get 
        {
            return transform.GetChild(0).gameObject;
        }
    }

    void Update()
    {
        PlayerInput();
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
        if (GameManager.Instance.IsAim() && Physics.Raycast(transform.position, Arrow.Instance.transform.forward, out hit, 100f, mask))
        {
            if (Mathf.Abs(transform.position.z - hit.point.z) > 2f)
                StartCoroutine(Move(hit));
        }
    }

    IEnumerator Move(RaycastHit hit) 
    {
        GameManager.Instance.SetRun();
        transform.rotation = Quaternion.LookRotation(hit.transform.position);
        while (transform.position != hit.point)
        {
            if (Mathf.Abs(transform.position.z - hit.point.z) < 2f)
                break;

            transform.position = Vector3.SmoothDamp(transform.position, hit.point, ref m_velocityBuffer, Time.fixedDeltaTime, 10f);
            yield return null;
        }
        GameManager.Instance.SetFight(hit);
    }
}
