using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameStat : MonoBehaviour
{
    private static TMPro.TextMeshProUGUI timer;
    private TMPro.TextMeshProUGUI damageTitle;
    private TMPro.TextMeshProUGUI soulsTitle;
    private TMPro.TextMeshProUGUI slimeCountTitle;
    private int HeroHP;

    [SerializeField]
    private UnityEngine.UI.Image Heart;

    private GameObject Healths;
    private GameObject SecondHealths;
    private GameObject Enemy;

    private static int enemyCount;

    public static int EnemyCount
    {
        get { return enemyCount; }
        set { enemyCount = value; }
    }

    private static List<string> enemyDeadName;

    public static string addEnemyDeadName
    {
        set { enemyDeadName.Add(value); }
    }

    public static List<string> EnemyDeadNameList
    {
        get { return enemyDeadName; }
        set { enemyDeadName = value; }
    }

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

    private const string enemyFilename = "enemy dead list.txt";

    void Start()
    {
        enemyDeadName = new List<string>();
        timer = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
        damageTitle = GameObject.Find("Damage").GetComponent<TMPro.TextMeshProUGUI>();      
        soulsTitle = GameObject.Find("Souls").GetComponent<TMPro.TextMeshProUGUI>();

        Healths = GameObject.Find("Healths");
        SecondHealths = GameObject.Find("SecondHealths");

        slimeCountTitle = GameObject.Find("CountSlime").GetComponent<TMPro.TextMeshProUGUI>();
        Enemy = GameObject.Find("Enemy");

        LoadAndApplyEnemyFromFile(enemyFilename);


        HeroHP = Stats.HP;
        for (int i = 0; i < HeroHP / 10; i++)
        {
            Instantiate(Heart, Healths.transform);
        }
    }

   
    void Update()
    {
        enemyCount = Enemy.transform.childCount;
        slimeCountTitle.text = enemyCount.ToString();

        if (Stats.HP >= 0)
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


    private void SaveEnemyNameToFile(string file)
    {
        string stats = $"";
        foreach (string name in enemyDeadName)
        {
            stats += $"{name};";
        }
        
        System.IO.File.WriteAllText(file, stats);
    }

    private void LoadAndApplyEnemyFromFile(string file)
    {
        string[] enemyName = System.IO.File.ReadAllText(file).Split(';');
        try
        {
            for (int i = 0; i < enemyName.Length-1; i++)
            {
                Destroy(Enemy.transform.Find(enemyName[i]).gameObject);             
                enemyDeadName.Add(enemyName[i]);
            }

            enemyCount = Enemy.transform.childCount;
            slimeCountTitle.text = enemyCount.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }


    private void OnDestroy()
    {
        SaveEnemyNameToFile(enemyFilename);
    }

}
