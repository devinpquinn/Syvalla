using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    public AudioMixerGroup masterMixer;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectsSlider;

    private void Start()
    {
        //set volume sliders to values stored in player prefs
        if (PlayerPrefs.HasKey("Volume_Master"))
        {
            masterSlider.value = PlayerPrefs.GetFloat("Volume_Master");
        }
        if (PlayerPrefs.HasKey("Volume_Music"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("Volume_Music");
        }
        if (PlayerPrefs.HasKey("Volume_Effects"))
        {
            effectsSlider.value = PlayerPrefs.GetFloat("Volume_Effects");
        }
    }

    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.audioMixer.SetFloat("volumeMaster", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Volume_Master", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        masterMixer.audioMixer.SetFloat("volumeMusic", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Volume_Music", sliderValue);
    }

    public void SetEffectsVolume(float sliderValue)
    {
        masterMixer.audioMixer.SetFloat("volumeEffects", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("Volume_Effects", sliderValue);
    }

    public void ResetVolume()
    {
        masterSlider.value = 1;
        musicSlider.value = 1;
        effectsSlider.value = 1;
    }

    public void Resume()
    {
        PlayerController.Instance.Unpause();
    }

    public void ExitToMenu()
    {
        StartCoroutine(DoExitToMenu());
    }

    IEnumerator DoExitToMenu()
    {
        Fade.FadeEffect();
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitToDesktop()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
}
