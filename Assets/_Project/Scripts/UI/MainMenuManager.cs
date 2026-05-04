using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    [Header("Options")]
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    [Header("Scenes")]
    public string firstGameScene = "Mainroom";

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);

        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        volumeSlider.value = savedVolume;
        fullscreenToggle.isOn = savedFullscreen;

        AudioListener.volume = savedVolume;
        Screen.fullScreen = savedFullscreen;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(firstGameScene);
    }

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exit Game");
    }
}