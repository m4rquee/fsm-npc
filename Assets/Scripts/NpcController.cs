using System;
using States;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : MonoBehaviour
{
    // public TextAsset fsmJson;
    public Text stateLabel;

    private Animator _animator;
    private FiniteStateMachine _finiteStateMachine;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _finiteStateMachine = new FiniteStateMachine();
        var idle = new IdleState(_animator);
        var move = new MoveState(_animator);

        /*var states = new List<State>();
        states.Add(idle);
        states.Add(move);
        FSMLoader.Load(_finiteStateMachine, fsmJson, states);*/

        _finiteStateMachine.AddAnyTransition(idle, idle.Eq(Property.DeltaTime, 0));
        At(idle, move, idle.Gt(Property.StopWatch, 5));
        At(move, idle, move.Gt(Property.StopWatch, 8));

        void At(State from, State to, Func<bool> condition) =>
            _finiteStateMachine.AddTransition(from, to, condition);
    }

    private void Update() => _finiteStateMachine.Tick(Time.deltaTime);

    private void OnGUI()
    {
        var stateName = _finiteStateMachine.CurrentStateName();
        var stateStopWatch = _finiteStateMachine.CurrentStopWatch().ToString("0.00");
        stateLabel.text = $"{stateName} - {stateStopWatch}s";
    }
}