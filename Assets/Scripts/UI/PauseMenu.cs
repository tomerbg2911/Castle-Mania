using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    

   


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<AudioManager>().Play("Menu Buttons");
            if (GameIsPaused)
            {
                if (!OptionsMenuUI.activeInHierarchy)
                {
                  Resume();
                }
                else
                {
                    OptionsMenuUI.SetActive(false);
                    Pause();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume() 
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
        GameIsPaused = false;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameIsPaused = false;
    }


}
