using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private static int damage = 5;

    private static int souls = 900;

    private static int maxHealthPoint = 100;
    private static int healthPoint = 50;

    private static Color color;
    private static float hitedTime;

    [SerializeField]
    private GameObject gameEnd;

    [SerializeField]
    private GameObject gameWin;

    private const string statsFilename = "stats.txt";
    void Start()
    {
        LoadAndApplyStatsFromFile(statsFilename);
    }


    // Update is called once per frame
    void Update()
    {
        

        if(healthPoint <= 0)
        {
            Time.timeScale = 0f;
            gameEnd.SetActive(true);
        }

        if(GameStat.EnemyCount == 0)
        {
            Time.timeScale = 0f;
            gameWin.SetActive(true);
        }

        gameObject.transform.Find("View").gameObject.GetComponent<SpriteRenderer>().color = color;
        if (hitedTime <= 0f)
        {
            color = Color.white;
        }
        else
        {
            hitedTime -= Time.deltaTime;
        }
    }


    public static Color HeroColor
    {
        set
        {
            color = value;
            hitedTime = 0.6f;
        }
    }

    public static int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public static int Souls
    {
        get { return souls; }
        set { souls = value; }
    }

    public static int HP
    {
        get { return healthPoint; }
        set { healthPoint = value; }
    }

    public static int maxHP
    {
        get { return maxHealthPoint; }
    }


    private void SaveStatsToFile(string file)
    {
        string stats = $"{healthPoint};{damage};{souls};{GameStat.GameTime};{gameObject.transform.position.x};{gameObject.transform.position.y};{gameObject.transform.position.z}";
        System.IO.File.WriteAllText(file, stats);
    }

    private void LoadAndApplyStatsFromFile(string file)
    {
        string[] stats = System.IO.File.ReadAllText(file).Split(';');
        try
        {
            healthPoint = Convert.ToInt32(stats[0]);
            damage = Convert.ToInt32(stats[1]);
            souls = Convert.ToInt32(stats[2]);
            GameStat.GameTime = float.Parse(stats[3]);
            gameObject.transform.position = new Vector3(float.Parse(stats[4]), float.Parse(stats[5]), float.Parse(stats[6]));
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }


    private void OnDestroy()
    {
        SaveStatsToFile(statsFilename);
    }

}
