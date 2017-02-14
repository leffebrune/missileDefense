using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UI : MonoBehaviour
{
    public static Game_UI Instance;
    public GameObject uiReady;
    public GameObject uiPlaying;
    public GameObject uifinished;

    public GameObject finished_win;
    public GameObject finished_lose;

    public Text textUpgradeSpeed;
    public Text textUpgradeExplosionRadius;

    public Button buttonUpgradeSpeed;
    public Button buttonUpgradeExplosionRadius;

    public Text textDay;

    public Text textHP;
    public Text textScore;
    public Text textRemain;

    public Text textPointRemain;
    public Text textCannonType;

    public GameObject upgradeAvailable;
    public Text textUpgrade1;
    public Text textUpgrade2;

    void Awake()
    {
        Instance = this;
    }

	void Start ()
    {
        
	}
	
	void Update ()
    {
		
	}

    public void UpdateHP(float hp)
    {
        textHP.text = "HP : " + hp.ToString();
    }

    public void UpdateScore(float score)
    {
        textScore.text = "Score : " + score.ToString();
    }

    public void UpdateRemain(int remain)
    {
        textRemain.text = "Remain : " + remain.ToString();
    }

    public void EnterReady()
    {
        uiReady.SetActive(true);
        uiPlaying.SetActive(false);
        uifinished.SetActive(false);
    }

    public void EnterPlaying()
    {
        uiPlaying.SetActive(true);
        uiReady.SetActive(false);
        uifinished.SetActive(false);
    }

    public void EnterFinished(bool isWin)
    {
        uiPlaying.SetActive(false);
        uiReady.SetActive(false);
        uifinished.SetActive(true);

        if (isWin)
        {
            finished_win.SetActive(true);
            finished_lose.SetActive(false);
            RefreshUpgrades();
        }
        else
        {
            finished_win.SetActive(false);
            finished_lose.SetActive(true);
        }
    }

    public void UpgradeSpeed()
    {
        GameBoard.Instance.upgrade.DoUpgrade(1, 0, 0);
        RefreshUpgrades();
    }

    public void UpgradeExplosionRadius()
    {
        GameBoard.Instance.upgrade.DoUpgrade(0, 0, 1);
        RefreshUpgrades();
    }

    public void RefreshUpgrades()
    {
        textCannonType.text = "Cannon Type : " + GameBoard.Instance.upgrade.cType.ToString();
        textUpgradeSpeed.text = string.Format("Speed : {0} / 9", GameBoard.Instance.upgrade.speed);
        textUpgradeExplosionRadius.text = string.Format("Explosion Rad : {0} / 9", GameBoard.Instance.upgrade.explosionRadius);
        textPointRemain.text = string.Format("Points Remain : {0}", GameBoard.Instance.upgrade.pointsRemain);

        var remain = (GameBoard.Instance.upgrade.pointsRemain > 0);

        buttonUpgradeSpeed.interactable = GameBoard.Instance.upgrade.speed <= 10 && remain;
        buttonUpgradeExplosionRadius.interactable = GameBoard.Instance.upgrade.explosionRadius <= 10 && remain;

        Upgrade.CannonType u1 = Upgrade.CannonType.Invalid;
        Upgrade.CannonType u2 = Upgrade.CannonType.Invalid;

        if (GameBoard.Instance.upgrade.UpgradeAvailable(ref u1, ref u2))
        {
            upgradeAvailable.SetActive(true);
            textUpgrade1.text = "Upgrade to : " + u1.ToString();
            if (u2 != Upgrade.CannonType.Invalid)
            {
                textUpgrade2.gameObject.SetActive(true);
                textUpgrade2.text = "Upgrade to : " + u2.ToString();
            }
            else
            {
                textUpgrade2.gameObject.SetActive(false);
            }
        }
        else
        {
            upgradeAvailable.SetActive(false);
        }
    }
}
