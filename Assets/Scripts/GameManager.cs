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
    private static GameState gameState;

    static GameObject m_camAim;
    static GameObject m_camRun;
    static GameObject m_camFight;

    void Start()
    {
        m_camAim = GameObject.Find("CamAim");
        m_camRun = GameObject.Find("CamRun");
        m_camFight = GameObject.Find("CamFight");
    }

    static public GameState CurrentState() { return gameState; }

    static public void SetAim() 
    {
        m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
        m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 4;
        Arrow.Instance.gameObject.SetActive(true);
        gameState = GameState.AIM;
    }

    static public void SetMenu() { gameState = GameState.MENU; }

    static public void SetStop() { gameState = GameState.STOP; }

    static public void SetRun()
    {
        m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
        m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 3;
        Arrow.Instance.gameObject.SetActive(false);
        gameState = GameState.RUN;
    }

    static public void SetFight(RaycastHit hit) 
    {
        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("enemy"))
        {
            m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
            m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
            gameState = GameState.FIGHT;
        }
        else 
        {
            SetAim();
            Destroy(hit.transform.gameObject);
        }
    }
   
    static public bool IsAim()
    {
        if (gameState == GameState.AIM)
            return true;

        return false;
    }

    static public bool IsRun()
    {
        if (gameState == GameState.RUN)
            return true;

        return false;
    }

    static public bool IsFight()
    {
        if (gameState == GameState.FIGHT)
            return true;

        return false;
    }

}
