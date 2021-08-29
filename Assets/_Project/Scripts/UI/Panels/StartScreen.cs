using UnityEngine;

public class StartScreen : UIPanel
{
    [SerializeField] private SessionManager _sessionManager;

    public void Fight()
    {
        _sessionManager.StartSession();
        Hide();
    }
}
