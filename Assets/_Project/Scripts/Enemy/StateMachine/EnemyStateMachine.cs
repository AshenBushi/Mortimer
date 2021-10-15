using System;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private State _firstState;
    
    private State _currentState;

    public State CurrentState => _currentState;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (_currentState == null) return;

        var next = _currentState.GetNextState();
    }

    private void Reset(State startState)
    {
        _currentState = startState;

        if (_currentState != null)
        {
            _currentState.Enter();
        }
    }
}
