using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public event Action OnSceneExit;
    public event Action<string> OnSceneEnter;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Coro());

        IEnumerator Coro()
        {
            OnSceneExit?.Invoke();
            ScreenFader.Instance.Fade(/*new Color(0.2078431f, 0.564817f, 0.9803922f)*/Color.black, 0.4f);
            yield return new WaitWhile(() => ScreenFader.Instance.IsFading);
            SceneManager.LoadScene(sceneName);
            OnSceneEnter?.Invoke(sceneName);
            ScreenFader.Instance.Fade(Color.clear, 1f);
        }
    }
}
