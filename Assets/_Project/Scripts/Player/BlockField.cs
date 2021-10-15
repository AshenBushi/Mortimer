using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerStateHandler _playerStateHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        _playerStateHandler.ToBlock();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _playerStateHandler.ToIdle();
    }
}
