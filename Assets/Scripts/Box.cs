using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    Transform[] m_particles;

    void Start()
    {
        m_particles = GetComponentsInChildren<Transform>();
    }

    public IEnumerator Explode() 
    {
        GameManager.Instance.SetAim();
        yield return new WaitForSeconds(1.0f);
        foreach (var particle in m_particles) 
        {
            particle.gameObject.AddComponent<BoxCollider>();
            particle.gameObject.AddComponent<Rigidbody>().AddExplosionForce(2f, Vector3.up, 1f);
        }
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject, 2f);
    }
}
