
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    public GameObject settingPanel;
    public Toggle muteToggle;

    private void Start()
    {
        muteToggle.isOn = !AudioManager.Instance.IsMute;
    }

    public void SwitchMute(bool isOn)
    {
        AudioManager.Instance.SwitchMuteState(isOn);
    }

    public void OnBackButtonDown()
    {
        PlayerPrefs.SetInt("gold",GameController.Instance.gold);
        PlayerPrefs.SetInt("lv",GameController.Instance.lv);
        PlayerPrefs.SetFloat("scd",GameController.Instance.smallTimer);
        PlayerPrefs.SetFloat("bcd",GameController.Instance.bigTimer);
        PlayerPrefs.SetInt("exp",GameController.Instance.exp);
        int convertedMute = AudioManager.Instance.IsMute ? 1 : 0;
        PlayerPrefs.SetInt("isMute", convertedMute);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }

    public void OnSettingButtonDown()
    {
        //settingPanel.SetActive(true);

        //Mine
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    public void OnCloseButtonDown()
    {
        settingPanel.SetActive(false);
    }
}
