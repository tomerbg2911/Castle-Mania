
using UnityEngine;


public class OptionsMenu : MonoBehaviour
{
    public GameObject SoundOnButton;
    public GameObject SoundOffButton;

    public void Mute()
    {
        AudioListener.volume = 0;
    }

    public void Unmute()
    {
        AudioListener.volume = 1;
        FindObjectOfType<AudioManager>().Play("Menu Buttons");
    }

    void Update()
    {
        SoundOnButton.SetActive(AudioListener.volume == 1);
        SoundOffButton.SetActive(AudioListener.volume == 0);
    }

}
