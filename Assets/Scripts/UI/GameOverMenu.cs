using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;


public class GameOverMenu : MonoBehaviour
{
    //public PauseMenu pauseMenu; // for the 'GameIsPaused' bool
    public int LostPlayerNumber = -1;
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
        if (LostPlayerNumber != -1)
        {
            GameObject.Find("Camera").GetComponent<Animator>().SetInteger("player lost num", LostPlayerNumber);
            StartCoroutine(GameOver());
        }
    }


    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.3f);
        TextMeshProUGUI textMeshPro = GameOverMenuUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        int winningPlayerNumber = LostPlayerNumber == 1 ? 2 : 1;  
        string newText = textMeshPro.text.Replace('$', (char)(winningPlayerNumber + '0'));
        textMeshPro.SetText(newText);
        GameOverMenuUI.SetActive(true);
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * this.fixedDeltaTime;
    }

}
