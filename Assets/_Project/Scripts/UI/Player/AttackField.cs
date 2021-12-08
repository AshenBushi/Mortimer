using UnityEngine;
using UnityEngine.EventSystems;

public class AttackField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerStateMachine _playerStateMachine;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerStateMachine.ToAttack();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _playerStateMachine.ToIdle();
    }
}
