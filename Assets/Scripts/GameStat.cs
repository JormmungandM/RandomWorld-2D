using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class GameStat : MonoBehaviour
{
    private static TMPro.TextMeshProUGUI timer;
    private TMPro.TextMeshProUGUI damageTitle;
    private TMPro.TextMeshProUGUI soulsTitle;
    private int HeroHP;

    [SerializeField]
    private Image Heart;

    private GameObject Healths;
    private GameObject SecondHealths;

    private static float gameTime = 60f;
    public static float GameTime
    {
        get => gameTime;
        set
        {
            gameTime = value;
            UpdateTime();
        }
    }

    private static void UpdateTime()
    {
        timer.transform.gameObject.SetActive(GameMenu.isTimerEnabled);
        TimeSpan sp = TimeSpan.FromSeconds(GameTime);
        timer.text = sp.ToString("mm':'ss'.'f");
    }


    void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
        damageTitle = GameObject.Find("Damage").GetComponent<TMPro.TextMeshProUGUI>();      
        soulsTitle = GameObject.Find("Souls").GetComponent<TMPro.TextMeshProUGUI>();
        Healths = GameObject.Find("Healths");
        SecondHealths = GameObject.Find("SecondHealths");

        HeroHP = Stats.HP;
        for (int i = 0; i < HeroHP / 10; i++)
        {
            Instantiate(Heart, Healths.transform);
        }
    }

   
    void Update()
    {
        if(Stats.HP >= 0)
        {
            HP_Update();
        }
        
        soulsTitle.text = Stats.Souls.ToString();
        damageTitle.text = Stats.Damage.ToString();
    }

    private void LateUpdate()
    {
        GameTime += Time.deltaTime;
    }

    void HP_Update()
    {
        
        if(Stats.HP >= 50 && HeroHP >= 50)
        {
            if (HeroHP > Stats.HP)
            {
                for (int i = 0; i < ((HeroHP - Stats.HP) / 10); i++)
                {
                    GameObject child = SecondHealths.transform.GetChild(i).gameObject;
                    Destroy(child);
                }
            }
            else if (HeroHP < Stats.HP)
            {
                for (int i = 0; i < ((Stats.HP - HeroHP) / 10); i++)
                {
                    Instantiate(Heart, SecondHealths.transform);
                }
            }
        }
        else
        {
            if (HeroHP > Stats.HP)
            {
                for (int i = 0; i < ((HeroHP - Stats.HP) / 10); i++)
                {
                    GameObject child = Healths.transform.GetChild(i).gameObject;
                    Destroy(child);
                }
            }
            else if (HeroHP < Stats.HP)
            {
                if(Stats.HP > 50)
                {
                    for (int i = 0; i < ((Stats.HP - 50 - HeroHP) / 10); i++)
                    {
                        Instantiate(Heart, Healths.transform);
                    }
                    HeroHP = Stats.HP - 50;
                    return;
                }
                else
                {
                    for (int i = 0; i < ((Stats.HP - HeroHP) / 10); i++)
                    {
                        Instantiate(Heart, Healths.transform);
                    }
                }

            }
        }

        HeroHP = Stats.HP;
    }

}
