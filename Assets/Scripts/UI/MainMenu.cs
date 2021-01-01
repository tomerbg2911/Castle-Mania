using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionsMenuUI;
    public GameObject MainMenuUI;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1 );
        
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && OptionsMenuUI.activeInHierarchy)
        {
            OptionsMenuUI.SetActive(false);
            MainMenuUI.SetActive(true);
        }
    }
}
