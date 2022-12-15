using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    private static int damage;
    private TMPro.TextMeshProUGUI damageTitle;
    private static int souls;
    private TMPro.TextMeshProUGUI soulsTitle;


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


    void Start()
    {
        damageTitle = GameObject.Find("Damage").GetComponent<TMPro.TextMeshProUGUI>();
        Debug.Log("Damage: " + damageTitle.text);
        soulsTitle = GameObject.Find("Souls").GetComponent<TMPro.TextMeshProUGUI>();
        Debug.Log("Souls: " + soulsTitle.text);
    }

    // Update is called once per frame
    void Update()
    {
        soulsTitle.text = souls.ToString();

    }



}
