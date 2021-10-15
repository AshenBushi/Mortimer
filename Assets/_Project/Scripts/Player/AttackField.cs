using UnityEngine;
using UnityEngine.EventSystems;

public class AttackField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerStateHandler _playerStateHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerStateHandler.ToAttack();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _playerStateHandler.ToIdle();
    }
}
