using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerStateMachine _playerStateMachine;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerStateMachine.ToBlock();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _playerStateMachine.ToIdle();
    }
}
