using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public class GameOverMenu : MonoBehaviour
{
    //public PauseMenu pauseMenu; // for the 'GameIsPaused' bool
    public bool playerIsDead = true;
    public GameObject GameOverMenuUI;
    private float fixedDeltaTime;

    private void Awake()
    {
        //pauseMenu = GameObject.Find("Canvas").GetComponent<PauseMenu>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

        if (playerIsDead)
        {
            StartCoroutine(GameOver());
        }
    }


    IEnumerator  GameOver()
    {
        yield return new WaitForSeconds(0.3f);
        GameOverMenuUI.SetActive(true);
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * this.fixedDeltaTime;
    }

}
