using States;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NpcController : MonoBehaviour
{
    public TextAsset fsmJson;
    public Text stateLabel;

    private Animator _animator;
    private FiniteStateMachine _finiteStateMachine;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _finiteStateMachine = new FiniteStateMachine();
        var idle = new IdleState(_animator);
        var move = new MoveState(_animator);

        FsmLoader.Load(_finiteStateMachine, fsmJson, new List<State> {idle, move});
    }

    private void Update() => _finiteStateMachine.Tick(Time.deltaTime);

    private void OnGUI()
    {
        var stateName = _finiteStateMachine.CurrentStateName();
        var stateStopWatch = _finiteStateMachine.CurrentStopWatch().ToString("0.00");
        stateLabel.text = $"{stateName} - {stateStopWatch}s";
    }
}