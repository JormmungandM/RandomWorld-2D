using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{

    private GameObject pauseContent;
    private GameObject settingContent;

    private Slider musicSlider;
    private Slider soundsSlider;
    private Toggle musicToggle;
    private Toggle soundsToggle;

    // Start is called before the first frame update
    void Start()
    {
        pauseContent = GameObject.Find("PauseContent");
        settingContent = GameObject.Find("SettingContent");

        HidePause();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
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
        HideSetting();
    }

    public void Toggle()
    {
        if (pauseContent.activeInHierarchy) HidePause();
        else ShowPause();
    }



    public void ShowSetting()
    {
        Time.timeScale = 0f;
        settingContent.SetActive(true);
    }

    public void HideSetting()
    {
        Time.timeScale = 1f;
        settingContent.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
