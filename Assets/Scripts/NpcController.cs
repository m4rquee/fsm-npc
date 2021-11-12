using States;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
    public TextAsset fsmJson;
    public Text stateLabel;

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private FiniteStateMachine _finiteStateMachine;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _finiteStateMachine = new FiniteStateMachine();
        var idle = new IdleState(_animator, _navMeshAgent, transform);
        var move = new MoveState(_animator, _navMeshAgent, transform);
        var flee = new FleeState(_animator, _navMeshAgent, transform);

        FsmLoader.Load(_finiteStateMachine, fsmJson, new List<State> {idle, move, flee});
    }

    private void Update() => _finiteStateMachine.Tick(Time.deltaTime);

    private void OnGUI()
    {
        if (stateLabel == null) return;
        var stateName = _finiteStateMachine.CurrentStateName();
        var stateStopWatch = _finiteStateMachine.CurrentStopWatch().ToString("0.00");
        stateLabel.text = $"{name}: {stateName} - {stateStopWatch}s";
    }
}