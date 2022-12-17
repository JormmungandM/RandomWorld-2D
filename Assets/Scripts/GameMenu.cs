using System;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    public static bool IsSoundEnabled { get; private set; }
    public static float SoundsVolume { get; private set; }

    private static bool isBackgroundMusicEnabled;
    private static float backgroundMusicVolume;

    public static bool isTimerEnabled { get; private set; }

    private GameObject pauseContent;
    private GameObject settingContent;
    private GameObject upgradeStats;
    private GameObject gameEnd;
    private GameObject gameWin;

    private TMPro.TextMeshProUGUI damagePriceTitle;
    private TMPro.TextMeshProUGUI hpPriceTitle;


    [SerializeField]
    private GameObject buyError;



    private float errorTime = 0f;
    private int DamagePrice = 5;
    private int HP_Price = 5;

    private int HeroHP = 50;

    private Slider musicSlider;
    private Slider soundsSlider;
    private Toggle musicToggle;
    private Toggle soundsToggle;
    private Toggle timerToggle;
    private AudioSource backgroundAudio;

    private static bool gameMenuIs = false;
    private const string settingsFilename = "settings.txt";

    // Start is called before the first frame update
    void Start()
    {
        pauseContent = GameObject.Find("PauseContent");
        settingContent = GameObject.Find("SettingContent");
        upgradeStats = GameObject.Find("StatsUpgrade");
        gameEnd = GameObject.Find("GameEnd");
        gameWin = GameObject.Find("GameWin");



        backgroundAudio = GetComponent<AudioSource>();
        musicSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        soundsSlider = GameObject.Find("SoundSlider").GetComponent<Slider>();
        musicToggle = GameObject.Find("MusicToggle").GetComponent<Toggle>();
        soundsToggle = GameObject.Find("SoundToggle").GetComponent<Toggle>();
        timerToggle = GameObject.Find("TimerToggle").GetComponent<Toggle>();

        damagePriceTitle = GameObject.Find("UpgradeDamage").gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        hpPriceTitle = GameObject.Find("UpgradeHP").gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();

        LoadAndApplySettingsFromFile(settingsFilename);

        damagePriceTitle.text = DamagePrice.ToString();
        hpPriceTitle.text = HP_Price.ToString();

        HidePause();
        gameEnd.SetActive(false);
        gameWin.SetActive(false);
        upgradeStats.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(errorTime > 0)
        {
            errorTime -= Time.deltaTime;
            if(!upgradeStats.activeInHierarchy)
            {
                errorTime = 0f;
            }
        }
        else
        {
            buyError.SetActive(false);
        }
    }

    private void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.K) && pauseContent.activeInHierarchy == false && gameEnd.activeInHierarchy == false)
        {

            if (upgradeStats.activeInHierarchy)
            {
                gameMenuIs = false;
                HideUpgradeStats();
          
            }
            else
            {
                Time.timeScale = 0f;
                gameMenuIs = true;
                upgradeStats.SetActive(true);
                
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (pauseContent.activeInHierarchy)
            {
                gameMenuIs = false;
                HidePause();
            }
            else
            {
                gameMenuIs = true;
                ShowPause();
            }

        }


    }


    public void ShowPause()
    {
        Time.timeScale = 0f;

        pauseContent.SetActive(true);
    }

    public void HidePause()
    {
        Time.timeScale = 1f;
        pauseContent.SetActive(false);
        gameMenuIs = false;
        HideSetting();
       
    }

    public void ShowSetting()
    {
        settingContent.SetActive(true);
    }

    public void HideSetting()
    {
        settingContent.SetActive(false);
    }

    public void HideUpgradeStats()
    {
        Time.timeScale = 1f;
        gameMenuIs = false;
        upgradeStats.SetActive(false);
    }

    public static bool GameMenuIs
    {
        get { return gameMenuIs; }
    }


    public void BuyDamage(GameObject price)
    {
        TMPro.TextMeshProUGUI priceText = price.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        int priveValue = Convert.ToInt32(priceText.text);
        if (Stats.Damage == 15)
        {
            errorTime = 0.5f;
            buyError.GetComponent<TMPro.TextMeshProUGUI>().text = "Max Damage";
            buyError.SetActive(true);
            Destroy(price);
        }
        else
        {
            if (priveValue <= Stats.Souls)
            {
                Stats.Damage += 1;
                Stats.Souls -= priveValue;
                if (Stats.Damage == 15)
                {
                    Destroy(price);
                }
                else
                {
                    priveValue += 3;
                    priceText.text = priveValue.ToString();
                    DamagePrice = priveValue;
                }

            }
            else
            {
                errorTime = 1f;
                buyError.GetComponent<TMPro.TextMeshProUGUI>().text = "Not enough souls";
                buyError.SetActive(true);
            }
        }
        
    }

    public void EditBuyDamageButton(GameObject button)
    {
         if (Stats.Damage == 15)
         {
            button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "MAX";
            button.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 0);
            button.GetComponent<Button>().enabled = false;
         }
    }

    public void BuyHP(GameObject price)
    {
        TMPro.TextMeshProUGUI priceText = price.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        int priveValue = Convert.ToInt32(priceText.text);
        if(Stats.HP == 100)
        {
            errorTime = 0.5f;
            buyError.GetComponent<TMPro.TextMeshProUGUI>().text = "Max HP";
            buyError.SetActive(true);
            Destroy(price);
        }
        else
        {
            if (priveValue <= Stats.Souls)
            {
                Stats.HP += 10;
                HeroHP = Stats.HP;
                Stats.Souls -= priveValue;
                if (Stats.HP == 100)
                {
                    Destroy(price);
                }
                else
                {
                    priveValue += 3;
                    priceText.text = priveValue.ToString();
                    HP_Price = priveValue;
                }
            }
            else
            {
                errorTime = 1f;
                buyError.GetComponent<TMPro.TextMeshProUGUI>().text = "Not enough souls";
                buyError.SetActive(true);
            }
        }
 
    }

    public void EditBuyHPButton(GameObject button)
    {
        if (Stats.HP == 100)
        {
            button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "MAX";
            button.GetComponent<Image>().color = new UnityEngine.Color(255, 255, 255, 0);
            button.GetComponent<Button>().enabled = false;
        }
    }

    public void BuyFullHP(TMPro.TextMeshProUGUI price)
    {
        int priveValue = Convert.ToInt32(price.text);
        if(HeroHP > Stats.HP && HeroHP <= Stats.maxHP)
        {
            if (priveValue <= Stats.Souls)
            {
                Stats.HP = HeroHP;
                Stats.Souls -= priveValue;
            }
            else
            {
                errorTime = 1f;
                buyError.GetComponent<TMPro.TextMeshProUGUI>().text = "Not enough souls";
                buyError.SetActive(true);
            }
        }
        else
        {
            errorTime = 1f;
            buyError.GetComponent<TMPro.TextMeshProUGUI>().text = "Health points are full";
            buyError.SetActive(true);
        }
    }



    public void OnMusicToggleChanged()
    {
        isBackgroundMusicEnabled = musicToggle.isOn;
        MucisPause();
    }

    public void OnMusicVolumeSliderChanged()
    {
        backgroundMusicVolume = musicSlider.value;
        backgroundAudio.volume = backgroundMusicVolume;
        MucisPause();
    }

    private void MucisPause()
    {
        if (backgroundAudio != null)
        {
            if (isBackgroundMusicEnabled && musicSlider.value > 0) backgroundAudio.UnPause();
            else backgroundAudio.Pause();
        }
    }

    public void OnSoundsToggleChanged()
    {
        IsSoundEnabled = soundsToggle.isOn;
    }

    public void OnSoundsVolumeSliderChanged()
    {
        SoundsVolume = soundsSlider.value;
    }

    public void OnTimerToggleChanged()
    {
        isTimerEnabled = timerToggle.isOn;
    }

    private void SaveSettingsToFile(string file)
    {
        string settings = $"{isBackgroundMusicEnabled};{backgroundMusicVolume};{IsSoundEnabled};{SoundsVolume};{isTimerEnabled};{DamagePrice};{HP_Price};{HeroHP}";
        System.IO.File.WriteAllText(file, settings);
    }

    private void LoadAndApplySettingsFromFile(string file)
    {
        string[] settings = System.IO.File.ReadAllText(file).Split(';');
        try
        {
            musicToggle.isOn = isBackgroundMusicEnabled = Convert.ToBoolean(settings[0]);
            musicSlider.value = backgroundMusicVolume = Convert.ToSingle(settings[1]);
            soundsToggle.isOn = IsSoundEnabled = Convert.ToBoolean(settings[2]);
            soundsSlider.value = SoundsVolume = Convert.ToSingle(settings[3]);
            timerToggle.isOn = isTimerEnabled = Convert.ToBoolean(settings[4]);
            DamagePrice = Convert.ToInt32(settings[5]);
            HP_Price = Convert.ToInt32(settings[6]);
            
            HeroHP = Convert.ToInt32(settings[7]);
            if (HeroHP < 50)
            {
                HeroHP = 50;
            }
            MucisPause();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }





    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (gameEnd.activeInHierarchy) gameEnd.SetActive(false);
        else gameWin.SetActive(false);

        Stats.HeroColor = UnityEngine.Color.white;
        Stats.HP = HeroHP = 50;
        Stats.Damage = 5;
        Stats.Souls = 0;
        DamagePrice = 5;
        HP_Price = 5;
        GameStat.GameTime = 0f;
        GameStat.EnemyDeadNameList = new List<string>();
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-2.48f, -6.82f, -2.1f);
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        SaveSettingsToFile(settingsFilename);
    }
}
