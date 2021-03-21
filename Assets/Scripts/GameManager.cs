using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    public enum GameState { MENU, STOP, AIM, RUN, FIGHT }
    private static GameState gameState = new GameState();

    void Start()
    {
        gameState = GameState.MENU;
    }

    static public GameState CurrentState() { return gameState; }

    static public void SetRun() { gameState = GameState.RUN; }
        
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
