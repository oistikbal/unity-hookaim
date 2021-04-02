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

    public enum GameState {AIM, RUN, FIGHT,KICK, MENU, STOP }
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

    static public void SetMenu() { gameState = GameState.MENU; }

    static public void SetStop() { gameState = GameState.STOP; }

    static public void SetAim() 
    {
        m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
        m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 4;
        m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 3;
        Arrow.Instance.gameObject.SetActive(true);
        gameState = GameState.AIM;

        PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("run", false);
        PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("fight", false);
        PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("kick", false);
        PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("idle", true);
    }

    static public void SetRun()
    {
        m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
        m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
        m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 3;
        Arrow.Instance.gameObject.SetActive(false);
        gameState = GameState.RUN;

        PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("idle", false);
        PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("fight", false);
        PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("kick", false);
        PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("run", true);
    }

    public void SetFight(RaycastHit hit) 
    {
        if(hit.transform.GetComponent<Enemy>() && hit.transform.GetComponent<Enemy>().m_health == 1f)
        {
            m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 4;
            m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
            m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
            gameState = GameState.KICK;

            PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("idle", false);
            PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("run", false);
            PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("fight", false);
            PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("kick", true);

            StartCoroutine(GameManager.Instance.Kick(hit));
        }
        else if (hit.transform.GetComponent<Enemy>())
        {
            m_camFight.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 10;
            m_camRun.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 4;
            m_camAim.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = 5;
            gameState = GameState.FIGHT;

            PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("idle", false);
            PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("run", false);
            PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("kick", false);
            PlayerController.Instance.playerModel.GetComponent<Animator>().SetBool("fight", true);

            StartCoroutine(GameManager.Instance.Fight(hit));
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

    IEnumerator Kick(RaycastHit hit)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(hit.transform.gameObject);
        SetAim();
    }

    IEnumerator Fight(RaycastHit hit) 
    {
        while (hit.transform.GetComponent<Enemy>().m_health != 0)
        {
            yield return new WaitForSeconds(1.0f);
            hit.transform.GetComponent<Enemy>().m_health--;
        }
        Destroy(hit.transform.gameObject);
        SetAim();
    }
}