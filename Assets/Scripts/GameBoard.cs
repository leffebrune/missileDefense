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
    public int day = 0;
    public float HP;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        sm.AddState(GameState.Ready,
            (prev, param) =>
            {
                day = 0;
                HP = 100;
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
                ProjectileManager.Instance.Clear();
            });

        sm.AddState(GameState.Finished,
            (prev, param) =>
            {
                if (result.isWin)
                {
                    upgrade.pointsRemain += 1;
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
        day++;
        sm.Enter(GameState.Play);
    }

    public void DoUpgrade(int idx)
    {
        Upgrade.CannonType u1 = Upgrade.CannonType.Invalid;
        Upgrade.CannonType u2 = Upgrade.CannonType.Invalid;
        if (upgrade.UpgradeAvailable(ref u1, ref u2))
        {
            if ((idx == 0) && (u1 != Upgrade.CannonType.Invalid))
                upgrade.cType = u1;
            else if ((idx == 1) && (u2 != Upgrade.CannonType.Invalid))
                upgrade.cType = u2;
        }
        Game_UI.Instance.RefreshUpgrades();
    }


    public void StartReady()
    {
        sm.Enter(GameState.Ready);
    }
}
