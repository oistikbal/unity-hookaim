using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    public enum GameState { MENU, STOP, PLAYING }
    private static GameState gameState = new GameState();

    void Start()
    {
        gameState = GameState.MENU;
    }

    static public GameState CurrentState() { return gameState; }

    static public void SetPlay() { gameState = GameState.PLAYING; }

    static public void SetMenu() { gameState = GameState.MENU; }

    static public void SetStop() { gameState = GameState.STOP; }
    static public bool IsPlaying()
    {
        if (gameState == GameState.PLAYING)
            return true;

        return false;
    }
}
