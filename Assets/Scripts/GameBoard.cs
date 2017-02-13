using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance;
    enum GameState
    {
        Ready,
        Play,
        Finished,
    };

    Game_Playing playing = new Game_Playing();
    StateMachine<GameState> sm = new StateMachine<GameState>();
    Game_Playing.Result result = new Game_Playing.Result();
    public Upgrade upgrade = new Upgrade();

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
                upgrade.Reset();
            },
            null,
            (next) =>
            {

            });

        sm.AddState(GameState.Play,
            (prev, param) =>
            {
                Game_UI.Instance.EnterPlaying();
                playing.Start(100, 10);
            },
            () =>
            {
                playing.OnUpdate();
                if (playing.CheckFinish(ref result))
                {
                    sm.Enter(GameState.Finished);
                }
            },
            (next) =>
            {
                ProjectileManager.Instance.Clear();
            });

        sm.AddState(GameState.Finished,
            (prev, param) =>
            {
                Game_UI.Instance.EnterFinished();
            },
            null,
            (next) =>
            {

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

    public void StartReady()
    {
        sm.Enter(GameState.Ready);
    }
}
