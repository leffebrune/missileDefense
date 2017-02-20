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
    public GameObject finished_clear;

    public Button buttonPrev;
    public Button buttonNext;
    public Button buttonLevelUp;
    public Text textDayStat;

    public Text textDay;

    public Text textHP;
    public Text textScore;
    public Text textRemain;

    public Text textPointRemain;
    public Text textCannonType;
    public Text textCannonStat;

    public GameObject upgradeAvailable;
    public Text textUpgrade1;
    public Text textUpgrade2;
    public Text textUpgrade3;

    public Text textRepair;

    public RectTransform cursor;

    int currentCannon = 1;

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

    public void UpdateHP()
    {
        textHP.text = "HP : " + Session.Instance.HP.ToString();
    }

    public void UpdateRemain()
    {
        textRemain.text = EnemySpawner.Instance.GetText();
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
            if (Session.Instance.day >= 15)
            {
                finished_win.SetActive(false);
                finished_lose.SetActive(false);
                finished_clear.SetActive(true);
            }
            else
            {
                finished_win.SetActive(true);
                finished_lose.SetActive(false);
                finished_clear.SetActive(false);
                RefreshInfos();
            }            
        }
        else
        {
            finished_win.SetActive(false);
            finished_clear.SetActive(false);
            finished_lose.SetActive(true);
        }
    }

    public void RefreshSessionStat()
    {
        textDayStat.text = "Day " + Session.Instance.day;
        textRepair.text = string.Format("HP Remain : {0} - Do Repair?", Session.Instance.HP);
        var remain = (Session.Instance.points > 0);
        textRepair.GetComponent<Button>().interactable = Session.Instance.HP != Session.Instance.MaxHP && remain;

        textPointRemain.text = string.Format("Points Remain : {0}", Session.Instance.points);
    }

    public void RefreshUpgradePanel()
    {
        var u = Session.Instance.cInfo[currentCannon - 1];
        var uu = GameData.Instance.cannonInfo[u._type].upgrades;

        var arr = new Text[]{ textUpgrade1, textUpgrade2, textUpgrade3 };

        for (var i = 0; i < 3; i++)
        {
            arr[i].gameObject.SetActive(i < uu.Count);
            if (i < uu.Count)
            {
                arr[i].text = "Upgrade to : " + uu[i].next.ToString();
                arr[i].GetComponent<Button>().interactable = Session.Instance.CanUpgrade(currentCannon - 1, i);
            }
        }
    }


    public void RefreshCannonStats()
    {
        var s = Session.Instance.cInfo[currentCannon - 1];

        textCannonType.text = "Cannon #" + currentCannon.ToString() + " : " + s._type.ToString() + " " + s.level + " lv";

        var _info = GameData.Instance.cannonInfo[s._type].info[s.level - 1];
        var stat = "Damage : " + _info.damage.ToString() + System.Environment.NewLine +
        "Speed : " + _info.speed.ToString() + System.Environment.NewLine +
        "Explosion : " + _info.explosionRadius.ToString() + System.Environment.NewLine;

        textCannonStat.text = stat;

        var interactable = Session.Instance.CanLevelUp(currentCannon - 1);
        buttonLevelUp.interactable = interactable;
        if (interactable)
            buttonLevelUp.GetComponent<Text>().text = "<Level UP! (cost : " + GameData.Instance.cannonInfo[s._type].info[s.level].cost.ToString() + ")>";
        else
            buttonLevelUp.GetComponent<Text>().text = "< MAX LEVEL! >";

        float[] xPos = { 640, 1000, 270 };
        cursor.anchoredPosition = new Vector2(xPos[currentCannon - 1], 150);
    }


    public void RefreshInfos()
    {
        RefreshCannonStats();
        RefreshUpgradePanel();
        RefreshSessionStat();
    }

    public void CannonPrev()
    {
        currentCannon--;
        if (currentCannon == 0)
            currentCannon = 3;
        RefreshInfos();
    }

    public void CannonNext()
    {
        if (currentCannon == 3)
            currentCannon = 0;
        currentCannon++;
        RefreshInfos();
    }

    public void LevelUp()
    {
        GameBoard.Instance.DoLevelUp(currentCannon - 1);
    }

    public void DoUpgrade(int uidx)
    {
        GameBoard.Instance.DoUpgrade(currentCannon - 1, uidx);
    }
}
