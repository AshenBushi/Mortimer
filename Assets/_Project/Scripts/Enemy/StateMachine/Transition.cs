using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;

    public State TargetState => _targetState;
    
    public bool NeedToTransit { get; protected set; }

    public void Init()
    {
        
    }

    private void OnEnable()
    {
        NeedToTransit = false;
    }
}
