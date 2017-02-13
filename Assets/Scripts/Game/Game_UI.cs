using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_UI : MonoBehaviour
{
    public static Game_UI Instance;
    public GameObject uiReady;
    public GameObject uiPlaying;

    public Text textHP;
    public Text textScore;

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

    public void EnterReady()
    {
        uiReady.SetActive(true);
        uiPlaying.SetActive(false);
    }

    public void EnterPlaying()
    {
        uiPlaying.SetActive(true);
        uiReady.SetActive(false);
    }
}
