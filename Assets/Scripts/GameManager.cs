using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /*  Default Cam priorties
     *  Current = 10
     *  Aim = 5
     *  Run = 4
     *  fight = 3
     */
    protected GameManager() { }

    public enum GameState {AIM, RUN, FIGHT, MENU, STOP }
    private GameState gameState;

    public GameObject m_camAim;
    public GameObject m_camFly;
    public GameObject m_camFight;

    Animator m_playerAnimator;

    void Start()
    {
        m_camAim = GameObject.Find("CamAim");
        m_camFly = GameObject.Find("CamFly");
        m_camFight = GameObject.Find("CamFight");
        m_playerAnimator = PlayerController.Instance.playerAnimator;
    }

    public GameState CurrentState() { return gameState; }

    public void SetMenu() { gameState = GameState.MENU; }

    public void SetStop() { gameState = GameState.STOP; }

    public void SetAim() 
    {
        m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
        m_camFly.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 4;
        m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 3;
        Arrow.Instance.gameObject.SetActive(true);
        gameState = GameState.AIM;

        m_playerAnimator.SetBool("fly", false);
        m_playerAnimator.SetBool("fight", false);
        m_playerAnimator.SetBool("kick", false);
        m_playerAnimator.SetBool("idle", true);
    }

    public void SetRun()
    {
        m_camFly.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
        m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
        m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 3;
        Arrow.Instance.gameObject.SetActive(false);
        gameState = GameState.RUN;
        Animator playerAnimator = PlayerController.Instance.playerAnimator;

        m_playerAnimator.SetBool("idle", false);
        m_playerAnimator.SetBool("fight", false);
        m_playerAnimator.SetBool("kick", false);
        m_playerAnimator.SetBool("fly", true);

    }

    public void SetFight(RaycastHit hit) 
    {
        if(hit.transform.GetComponent<Enemy>() && hit.transform.GetComponent<Enemy>().m_health == 1f) // 1hp enemy
        {
            m_camFly.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 4;
            m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
            m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
            gameState = GameState.FIGHT;


            m_playerAnimator.SetBool("idle", false);
            m_playerAnimator.SetBool("fly", false);
            m_playerAnimator.SetBool("fight", false);
            m_playerAnimator.SetBool("kick", true);

            StartCoroutine(GameManager.Instance.Kick(hit));
            hit.transform.GetComponent<Animator>().SetBool("idle", false);
            hit.transform.GetComponent<Animator>().SetBool("die", true);

        }
        else if (hit.transform.GetComponent<Enemy>())// more than 1 hp enemy
        {
            m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
            m_camFly.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 4;
            m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
            gameState = GameState.FIGHT;

            m_playerAnimator.SetBool("idle", false);
            m_playerAnimator.SetBool("fly", false);
            m_playerAnimator.SetBool("kick", false);
            m_playerAnimator.SetBool("fight", true);

            hit.transform.GetComponent<Animator>().SetBool("idle", false);
            hit.transform.GetComponent<Animator>().SetBool("fight", true);

            StartCoroutine(GameManager.Instance.Fight(hit));
        }
        else //box
        {
            StartCoroutine(GameManager.Instance.Kick(hit));
            StartCoroutine(hit.transform.GetComponent<Box>().Explode());
        }
    }
   
    IEnumerator Die(RaycastHit hit) 
    {
        hit.transform.GetComponent<Animator>().SetBool("fight", false);
        hit.transform.GetComponent<Animator>().SetBool("idle", false);
        hit.transform.GetComponent<Animator>().SetBool("die", true);
        yield return new WaitForSeconds(1.5f);
        hit.transform.GetComponent<BoxCollider>().enabled = false;
        Destroy(hit.transform.gameObject, 5f);
        SetAim();
    }

    IEnumerator Kick(RaycastHit hit)
    {
        yield return new WaitForSeconds(1.5f);
        hit.transform.GetComponent<BoxCollider>().enabled = false;
        Destroy(hit.transform.gameObject, 5f);
        SetAim();
    }

    IEnumerator Fight(RaycastHit hit) 
    {
        while (hit.transform.GetComponent<Enemy>().m_health != 0)
        {
            yield return new WaitForSeconds(1.0f);
            hit.transform.GetComponent<Enemy>().m_health--;
        }
        StartCoroutine(Die(hit));
    }

    public bool IsAim()
    {
        if (gameState == GameState.AIM)
            return true;

        return false;
    }

    public bool IsRun()
    {
        if (gameState == GameState.RUN)
            return true;

        return false;
    }

    public bool IsFight()
    {
        if (gameState == GameState.FIGHT)
            return true;

        return false;
    }
}