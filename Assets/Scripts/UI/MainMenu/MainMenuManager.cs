using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject loadingscreen;
    public GameObject playPanel;
    public GameObject title;
    public GameObject entryPanel;
    public GameObject settingsPanel;
    public GameObject upgradesPanel;
    public GameObject lootboxScreen;

    private void Start()
    {
        GameObject.Find( "MusicPlayer" ).GetComponent<MusicPlayer>().Initialize();
    }

    public void PlayGame()
    {
        loadingscreen.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void PlayPlayground()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void LootBoxOpen()
    {
        title.SetActive( false );
        entryPanel.SetActive( false );
        lootboxScreen.SetActive( true );
    }
    public void LootBoxClose()
    {
        title.SetActive( true );
        entryPanel.SetActive( true );
        lootboxScreen.SetActive( false );
    }
    public void SettingsOpen()
    {
        entryPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void PlayPanelOpen()
    {
        entryPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    public void SettingsBack()
    {
        entryPanel.SetActive(true);
        settingsPanel.SetActive(false);
        settingsPanel.GetComponentInChildren<UISettingsManager>().SaveValues();
    }
    public void PlayPanelBack()
    {
        entryPanel.SetActive(true);
        playPanel.SetActive(false);
    }
    public void OpenUpgradesShop()
    {
        upgradesPanel.SetActive(true);
        entryPanel.SetActive(false);
    }
    public void CloseUpgradesShop()
    {
        upgradesPanel.SetActive(false);
        entryPanel.SetActive(true);
    }
}
