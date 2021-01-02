using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionsMenuUI;
    public GameObject MainMenuUI;
    public GameObject SoundOnButton;
    public GameObject SoundOffButton;
    
    public void PlayButtonSound()
    {
        FindObjectOfType<AudioManager>().Play("Menu Buttons");
    }

    public void PlayGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
        
    }

    public void QuitGame()
    {
        PlayButtonSound();
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void MuteSound(bool mute)
    {
        AudioListener.volume = mute ? 0f : 1f;
    }

    public void Update()
    {
        SoundOnButton.SetActive(AudioListener.volume == 1);
        SoundOffButton.SetActive(AudioListener.volume == 0);

        if (Input.GetKeyDown(KeyCode.Escape) && OptionsMenuUI.activeInHierarchy)
        {
            OptionsMenuUI.SetActive(false);
            MainMenuUI.SetActive(true);
        }
    }
}
