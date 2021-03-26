using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Angle
{
    private const float m_ClampMaximum = Mathf.PI / 2f;
    private float m_Clamp;
    private bool m_reverse;

    public float Clamp
    {
        get
        {
            if (m_reverse)
                return m_ClampMaximum - m_Clamp;
            else
                return m_Clamp;
        }
        set
        {
            if ((int)(value / m_ClampMaximum ) % 2 >= 1f )
            {
                m_reverse = true;
            }
            else 
            {
                m_reverse = false;
            }

            m_Clamp = value % m_ClampMaximum;
        }
    }
}


public class Arrow : Singleton<Arrow>
{
    protected Arrow() { }

    Vector3 m_relativePosition;

    Vector4 m_col1;
    Vector4 m_col2;
    Vector4 m_col3;
    Vector4 m_col4;
    Matrix4x4 m_rotMatrix;

    Angle m_angle;

    [SerializeField] float m_turnSpeed = 5f;

    void Start()
    {
        m_relativePosition = transform.position - PlayerController.Instance.transform.position;
    }

    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate() 
    {

        m_angle.Clamp = Time.time * m_turnSpeed;

        m_col1 = new Vector4(Mathf.Cos(m_angle.Clamp), 0, -Mathf.Sin(m_angle.Clamp), 0);
        m_col2 = new Vector4(0, 1f, 0, 0);
        m_col3 = new Vector4(Mathf.Sin(m_angle.Clamp), 0, Mathf.Cos(m_angle.Clamp), 0);
        m_col4 = new Vector4(0, 0, 0, 1);
        m_rotMatrix = new Matrix4x4(m_col1, m_col2, m_col3, m_col4);


        transform.position = m_rotMatrix.MultiplyPoint(m_relativePosition) + PlayerController.Instance.transform.position;
        transform.rotation = Quaternion.LookRotation(transform.position - PlayerController.Instance.transform.position);
    }

}
