using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : UIPanel
{
    [SerializeField] private KillCounter _killCounter;
    [SerializeField] private TMP_Text _text;

    public override void Show()
    {
        base.Show();
        _text.text = _killCounter.KillCount.ToString();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
