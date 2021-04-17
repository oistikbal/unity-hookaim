using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    public enum GameState {AIM, RUN, FIGHT, MENU, STOP }
    public enum PlayerAnimationState { IDLE,FLY,FIGHT,KICK}
    public enum EnemyAnimationState { IDLE,FIGHT,DIE}
    private GameState m_gameState;

    private List<GameObject> m_cameras = new List<GameObject>();

    GameObject m_camAim;
    GameObject m_camFly;
    GameObject m_camFight;

    void Start()
    {
        StoreCameras();
    }

    public GameState CurrentState() { return m_gameState; }

    public void SetMenu() { m_gameState = GameState.MENU; }

    public void SetStop() { m_gameState = GameState.STOP; }

    public void SetAim() 
    {
        m_gameState = GameState.AIM;
        Arrow.Instance.gameObject.SetActive(true);

        SetActiveCamera(m_camAim);
        SetPlayerActiveAnimation(PlayerAnimationState.IDLE);
    }

    public void SetRun()
    {
        m_gameState = GameState.RUN;
        
        Arrow.Instance.gameObject.SetActive(false);
        SetActiveCamera(m_camFly);
        SetPlayerActiveAnimation(PlayerAnimationState.FLY);
    }

    public void SetFight(GameObject enemy) 
    {
        Enemy enemyComponent = enemy.GetComponent<Enemy>();

        if(enemyComponent && enemyComponent.m_health == 1f) // 1hp enemy
        {
            m_gameState = GameState.FIGHT;

            SetActiveCamera(m_camFight);
            SetPlayerActiveAnimation(PlayerAnimationState.KICK);
            SetEnemyActiveAnimation(EnemyAnimationState.DIE, enemy);
           
            StartCoroutine(GameManager.Instance.Kick(enemy));
        }
        else if (enemyComponent)// more than 1 hp enemy
        {
            m_gameState = GameState.FIGHT;

            SetActiveCamera(m_camFight);
            SetPlayerActiveAnimation(PlayerAnimationState.FIGHT);
            SetEnemyActiveAnimation(EnemyAnimationState.FIGHT, enemy);

            StartCoroutine(GameManager.Instance.Fight(enemy));
        }
        else //box
        {
            StartCoroutine(GameManager.Instance.Kick(enemy));
            StartCoroutine(enemy.transform.GetComponent<Box>().Explode());
        }
    }
   
    IEnumerator Die(GameObject enemy) 
    {
        SetEnemyActiveAnimation(EnemyAnimationState.DIE, enemy);
        yield return new WaitForSeconds(1.5f);
        enemy.transform.GetComponent<BoxCollider>().enabled = false;
        Destroy(enemy, 5f);
        SetAim();
    }

    IEnumerator Kick(GameObject enemy)
    {
        yield return new WaitForSeconds(1.5f);
        enemy.transform.GetComponent<BoxCollider>().enabled = false;
        Destroy(enemy, 5f);
        SetAim();
    }

    IEnumerator Fight(GameObject enemy) 
    {
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        while (enemyComponent.m_health != 0)
        {
            yield return new WaitForSeconds(1.0f);
            enemyComponent.m_health--;
        }
        StartCoroutine(Die(enemy));
    }

    public void StoreCameras()
    {
        m_camAim = GameObject.Find("CamAim");
        m_camFly = GameObject.Find("CamFly");
        m_camFight = GameObject.Find("CamFight");

        m_cameras.Add(m_camAim);
        m_cameras.Add(m_camFight);
        m_cameras.Add(m_camFly);
    }

    public void SetActiveCamera(GameObject activeCamera)
    {
        int priority = m_cameras.Count;
        foreach (var camera in m_cameras)
        {
            if (camera == activeCamera)
            {
                camera.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = m_cameras.Count;
                priority--;
            }
            else
            {
                priority--;
                camera.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Priority = priority;
            }
        }
    }

    public void SetPlayerActiveAnimation(PlayerAnimationState animationState)
    {
        Animator playerAnimator = PlayerController.Instance.playerAnimator;
        string animationName = Enum.GetName(typeof(PlayerAnimationState), animationState);

        foreach (string animation in Enum.GetNames(typeof(PlayerAnimationState)))
        {
            if (animation == animationName)
            {
                playerAnimator.SetInteger(animation, 1);
            }
            else
            {
                playerAnimator.SetInteger(animation, 0);
            }
        }
    }

    public void SetEnemyActiveAnimation(EnemyAnimationState animationState, GameObject enemy)
    {
        Animator enemyAnimator = enemy.GetComponent<Animator>();
        string animationName = Enum.GetName(typeof(EnemyAnimationState), animationState);

        foreach (string animation in Enum.GetNames(typeof(EnemyAnimationState)))
        {
            if (animation == animationName)
            {
                enemyAnimator.SetInteger(animation, 1);
            }
            else
            {
                enemyAnimator.SetInteger(animation, 0);
            }
        }
    }

    public bool IsAim()
    {
        if (m_gameState == GameState.AIM)
            return true;

        return false;
    }

    public bool IsRun()
    {
        if (m_gameState == GameState.RUN)
            return true;

        return false;
    }

    public bool IsFight()
    {
        if (m_gameState == GameState.FIGHT)
            return true;

        return false;
    }
}