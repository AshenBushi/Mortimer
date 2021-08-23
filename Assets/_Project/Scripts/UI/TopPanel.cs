using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopPanel : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    public void PauseGame()
    {
        _pausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Play()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
