using UnityEngine;

public class StartPanel : UIPanel
{
    [SerializeField] private SessionManager _sessionManager;

    public void Fight()
    {
        _sessionManager.StartSession();
        Disable();
    }
}
