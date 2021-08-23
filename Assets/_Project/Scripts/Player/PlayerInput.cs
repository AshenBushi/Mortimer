using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Player _player;

    public void OnPointerDown(PointerEventData eventData)
    {
        _player.Attack();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _player.Idle();
    }
}
