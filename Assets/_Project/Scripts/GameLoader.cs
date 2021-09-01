using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    private AsyncOperation _operation;

    private void Start()
    {
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        _operation = SceneManager.LoadSceneAsync(1);
        _operation.allowSceneActivation = false;

        yield return new WaitForSeconds(1f);

        _operation.allowSceneActivation = true;
    }
}
