using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ScreenFader
    : Singleton<ScreenFader>
{
    public Image Fader;
    public PostProcessVolume PostProcess;
    private DepthOfField _dof;

    public bool IsFading => _isFading;
    private bool _isFading;

    private void Start()
    {
        _dof = PostProcess.profile.GetSetting<DepthOfField>();
    }

    public void SetFocusDistance(float amount = 6, float duration = 1f)
    {
        DOTween.To(() => _dof.focusDistance.value, x => _dof.focusDistance.value = x, amount, duration);
    }

    public void Fade(Color color, float duration = 1f)
    {
        _isFading = true;
        Fader.DOComplete();
        Fader.DOColor(color, duration).OnComplete(() => _isFading = false);
    }
}
