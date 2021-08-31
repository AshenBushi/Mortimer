using UnityEngine;

public class StopScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = Time.timeScale == 1f ? 0f : 1f;
        }
    }
}
