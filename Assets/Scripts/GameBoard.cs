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

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        sm.AddState(GameState.Ready,
            (prev, param) =>
            {
                Session.Instance.Reset();
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
                playing.Start(1);
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
                EnemyManager.Instance.Clear();
                EnemySpawner.Instance.Clear();
            });

        sm.AddState(GameState.Finished,
            (prev, param) =>
            {
                if (result.isWin)
                {
                    Session.Instance.points += 5;
                }
                Game_UI.Instance.EnterFinished(result.isWin);
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
        Session.Instance.day++;
        sm.Enter(GameState.Play);
    }

    public void StartReady()
    {
        sm.Enter(GameState.Ready);
    }

    public void DoLevelUp(int idx)
    {
        Session.Instance.LevelUp(idx);
        Game_UI.Instance.RefreshInfos();
    }

    public void DoUpgrade(int idx, int uIdx)
    {
        Session.Instance.DoUpgrade(idx, uIdx);
        Game_UI.Instance.RefreshInfos();
    }

    public void Repair()
    {
        Session.Instance.Repair();
        Game_UI.Instance.RefreshInfos();
    }
}
