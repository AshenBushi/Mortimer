using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] private List<Transition> _transitions;

    public void Enter()
    {
        if (!enabled)
        {
            enabled = true;
            
            foreach (var transition in _transitions)
            {
                transition.enabled = true;
                transition.Init();
            }
        }
    }

    public State GetNextState()
    {
        return (from transition in _transitions where transition.NeedToTransit select transition.TargetState).FirstOrDefault();
    }
}
