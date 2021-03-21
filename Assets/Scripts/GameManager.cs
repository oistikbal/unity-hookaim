using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /*  Default Cam priorties
     *  Aim = 5
     *  Run = 4
     *  fight = 3
     */
    protected GameManager() { }

    public enum GameState { MENU, STOP, AIM, RUN, FIGHT }
    private static GameState gameState = new GameState();

    static GameObject m_camAim;
    static GameObject m_camRun;

    void Awake()
    {
    }

    void Start()
    {
        m_camAim = GameObject.Find("CamAim");
        m_camRun = GameObject.Find("CamRun");
        SetAim();
    }

    static public GameState CurrentState() { return gameState; }

    static public void SetAim() 
    {
        m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
        m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 4;
        PlayerController.m_arrow.SetActive(true);
        gameState = GameState.AIM;
    }

    static public void SetRun() 
    {
        m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
        PlayerController.m_arrow.SetActive(false);
        gameState = GameState.RUN;
    }
        
    static public void SetMenu() { gameState = GameState.MENU; }

    static public void SetStop() { gameState = GameState.STOP; }
   
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
