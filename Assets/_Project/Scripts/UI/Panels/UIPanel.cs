using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    [SerializeField] private float _animationSpeed;

    private CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    protected IEnumerator ChangeAlphaSlowly(float value)
    {
        while (_canvasGroup.alpha != value)
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, value, Time.deltaTime * _animationSpeed);
            yield return null;
        }
    }
    
    public virtual void Enable()
    {
        gameObject.SetActive(true);
    }

    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show(AnimationName animation)
    {
        switch (animation)
        {
            case AnimationName.Instantly:
                _canvasGroup.alpha = 1f;
                _canvasGroup.blocksRaycasts = true;
                break;
            case AnimationName.Slowly:
                StartCoroutine(ChangeAlphaSlowly(1));
                _canvasGroup.blocksRaycasts = true;
                break;
        }
    }
    
    public virtual void Hide(AnimationName animation)
    {
        switch (animation)
        {
            case AnimationName.Instantly:
                _canvasGroup.alpha = 0f;
                _canvasGroup.blocksRaycasts = false;
                break;
            case AnimationName.Slowly:
                StartCoroutine(ChangeAlphaSlowly(0));
                _canvasGroup.blocksRaycasts = false;
                break;
        }
    }
}

[Serializable]
public enum AnimationName
{
    Instantly,
    Slowly
}
