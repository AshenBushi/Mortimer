using UnityEngine;
using UnityEngine.EventSystems;

public class AttackField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerAttackHandler _playerAttackHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerAttackHandler.ToAttack();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _playerAttackHandler.ToIdle();
    }
}
