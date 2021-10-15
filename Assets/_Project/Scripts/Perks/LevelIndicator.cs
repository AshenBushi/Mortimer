using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour
{
    [SerializeField] private Sprite _enabled;
    [SerializeField] private Sprite _disabled;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Enable()
    {
        _image.sprite = _enabled;
    }

    public void Disable()
    {
        _image.sprite = _disabled;
    }

    public void Hide()
    {
        _image.color = Color.clear;
    }

    public void Show()
    {
        _image.color = Color.white;
    }
}
