using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayPauseSounds : MonoBehaviour
{
    public Sprite muteImage;
    public Sprite playImage;

    private bool _isMuted;

    public void ChangeSoundState()
    {
        if (_isMuted)
        {
            GetComponent<Image>().sprite = muteImage;
            AudioListener.volume = 0;
        }
        else
        {
            GetComponent<Image>().sprite = playImage;
            AudioListener.volume = 1;
        }
        _isMuted = !_isMuted;
    }
}
