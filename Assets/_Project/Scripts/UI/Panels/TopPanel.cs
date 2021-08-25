using UnityEngine;

public class TopPanel : UIPanel
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
