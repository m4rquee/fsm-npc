using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using States;

public class FiniteStateMachine
{
    private State _currentState;

    private class Transition
    {
        public readonly State To;
        public readonly Func<bool> Condition;

        public Transition(State to, [NotNull] Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();

    private List<Transition> _currentTransitions = new List<Transition>();
    private readonly List<Transition> _anyTransitions = new List<Transition>();
    private readonly List<Transition> _emptyTransitions = new List<Transition>(0);

    public void Tick(float deltaTime)
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.To);

        _currentState?.Tick(deltaTime);
    }

    public void SetState(State state)
    {
        if (state == _currentState) // Will stay in the current state:
            return;

        // Will enter a new state:
        _currentState?.OnExit();
        _currentState = state;

        // Get the transitions from this new state:
        _currentTransitions = null;
        if (_currentState != null)
            if (!_transitions.TryGetValue(_currentState.GetType(), out _currentTransitions))
                _currentTransitions = _emptyTransitions;

        // Enter the new state:
        _currentState?.ResetTimer();
        _currentState?.OnEnter();
    }

    public void AddTransition([NotNull] State from, State to, [NotNull] Func<bool> predicate)
    {
        if (!_transitions.TryGetValue(from.GetType(), out var transitions))
            _transitions[from.GetType()] = transitions = new List<Transition>();
        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(State to, [NotNull] Func<bool> predicate) =>
        _anyTransitions.Add(new Transition(to, predicate));

    private Transition GetTransition()
    {
        // Try to find a satisfied transition for the current state. List order matters and any-transitions
        // have higher priority:
        var transition = _anyTransitions.Find(t => t.Condition());
        return transition ?? _currentTransitions.Find(t => t.Condition());
    }

    public string CurrentStateName() => _currentState.GetType().Name;

    public float CurrentStopWatch() => _currentState.StopWatch;
}