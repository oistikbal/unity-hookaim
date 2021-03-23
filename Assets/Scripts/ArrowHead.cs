using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHead : Singleton<ArrowHead>
{
    protected ArrowHead() { }

    Vector3 m_positionDiff;

    Vector4 m_col1;
    Vector4 m_col2;
    Vector4 m_col3;
    Vector4 m_col4;
    Matrix4x4 m_rotMatrix;

    [SerializeField] float m_turnSpeed = 5f;

    void Start()
    {
        m_positionDiff = transform.position - PlayerController.Instance.transform.position;
    }

    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate() 
    {
        m_col1 = new Vector4(Mathf.Cos(Time.time * m_turnSpeed), 0, -Mathf.Sin(Time.time * m_turnSpeed), 0);
        m_col2 = new Vector4(0, 1f, 0, 0);
        m_col3 = new Vector4(Mathf.Sin(Time.time * m_turnSpeed), 0, Mathf.Cos(Time.time * m_turnSpeed), 0);
        m_col4 = new Vector4(0, 0, 0, 1);
        m_rotMatrix = new Matrix4x4(m_col1, m_col2, m_col3, m_col4);


        transform.position = m_rotMatrix.MultiplyPoint(m_positionDiff) + PlayerController.Instance.transform.position;
        transform.rotation = Quaternion.LookRotation(transform.position - PlayerController.Instance.transform.position);
    }

}
