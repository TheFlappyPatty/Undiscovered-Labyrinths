using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Canvas MainMenu;
    public Canvas Settings;
    public Canvas LevelSelect;

    public void CloseMenus()
    {
        MainMenu.enabled = false;
        Settings.enabled = false;
        LevelSelect.enabled = false;
    }
    public void SettingsMenu()
    {
        CloseMenus();
        Settings.enabled = true;
    }
    public void MainMenuMenu()
    {
        CloseMenus();
        MainMenu.enabled = true;
    }
    public void LevelSelectMenu()
    {
        CloseMenus();
        LevelSelect.enabled = true;
    }
    public void quitGame()
    {
        Application.Quit();
    }
    public void LoadLevel(String LevelName)
    {
        SceneManager.LoadScene(LevelName);
    }
}
