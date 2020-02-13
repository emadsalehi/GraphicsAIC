using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void BackToMenuFunction()
    {
        if (GameObject.Find("SoundController") != null)
        {
            AudioManager _audioManager = GameObject.Find("SoundController").GetComponent<AudioManager>();
            _audioManager.Stop("Game");
            _audioManager.Play("Menu");
        }
        SceneManager.LoadScene(0);
    }
}
