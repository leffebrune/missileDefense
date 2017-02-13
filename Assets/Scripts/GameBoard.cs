using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance;
    enum GameState
    {
        Ready,
        Play
    };

    Game_Playing playing = new Game_Playing();
    StateMachine<GameState> sm = new StateMachine<GameState>();

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        sm.AddState(GameState.Ready,
            (prev, param) =>
            {
                Game_UI.Instance.EnterReady();
            },
            null,
            (next) =>
            {

            });

        sm.AddState(GameState.Play,
            (prev, param) =>
            {
                Game_UI.Instance.EnterPlaying();
                playing.Start(100);
            },
            () =>
            {
                if (playing.HP <= 0)
                {
                    sm.Enter(GameState.Ready);
                }
            },
            (next) =>
            {
                ProjectileManager.Instance.Clear();
            });

        sm.Enter(GameState.Ready);
    }

    void Update()
    {
        sm.OnUpdate();
    }

    public void StartPlay()
    {
        sm.Enter(GameState.Play);
    }
}
