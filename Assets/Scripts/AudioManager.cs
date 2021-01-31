using DG.Tweening;
using UnityEngine;

public class AudioManager
    : Singleton<AudioManager>
{
    public AudioSource MainMusic;
    public AudioSource GalleryMusic;
    public AudioSource Rain;

    public void Start()
    {
        SceneController.Instance.OnSceneExit += FadeOut;
        SceneController.Instance.OnSceneEnter += FadeIn;
    }

    private void FadeIn(string scene)
    {
        if (scene == "Gallery")
        {
            GalleryMusic.time = 0;
            GalleryMusic.Play();
            GalleryMusic.DOFade(0.2f, 0.5f);
        }
        else
        {
            MainMusic.time = 0;
            MainMusic.Play();
            MainMusic.DOFade(0.5f, 0.5f);
            Rain.DOFade(0.5f, 0.5f);
        }

    }

    private void FadeOut()
    {
        Rain.DOFade(0, 1f);
        MainMusic.DOFade(0, 0.5f);
        GalleryMusic.DOFade(0, 0.5f);
    }
}
