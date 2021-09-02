using UnityEngine.SceneManagement;

public class LoseScreen : UIPanel
{
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
