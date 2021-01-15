using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int ScoreAmount;
    public static int EffectivScore;

    private int Bonus;

    public GameObject Berserk;
    private Text scoreText;
    public Slider Weapon;

    public GameObject GunBonus;
    public GameObject BaseGun;

    public GameObject GunBonus1;
    public GameObject BaseGun1;

    public GameObject Player1;
    public GameObject Player2;

    private void Start()
    {
        scoreText = GetComponent<Text>();
        ScoreAmount = 0;
    }

    private void FixedUpdate()
    {
        GunBonus = GameObject.Find("/Player1/SuperGun/GunBonus");
        BaseGun = GameObject.Find("/Player1/SuperGun/BaseGun");

        GunBonus1 = GameObject.Find("/Player2/SuperGun/GunBonus");
        BaseGun1 = GameObject.Find("/Player2/SuperGun/BaseGun");

        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");

        scoreText.text = "SCORE: " + EffectivScore.ToString("00000000");

        if (Score.ScoreAmount < 1)
        {
            return;
        }

        Score.ScoreAmount += -Bonus;

        Weapon.value = ScoreAmount;

    }

    private void Update()
    {
        if (Score.ScoreAmount > 250)
        {
            if (Player1 == true)
            {
                GunBonus.SetActive(true);
                BaseGun.SetActive(false);
            }

            if (Player2 == true)
            {
                GunBonus1.SetActive(true);
                BaseGun1.SetActive(false);
            }

            Bonus = 6;

            Berserk.SetActive(true);

        }
        else if (Score.ScoreAmount < 250)
        {

            if (Player1 == true)
            {
                GunBonus.SetActive(false);
                BaseGun.SetActive(true);
            }

            if (Player2 == true)
            {
                GunBonus1.SetActive(false);
                BaseGun1.SetActive(true);
            }

            Bonus = 4;

            Berserk.SetActive(false);
        }
    }
} 
