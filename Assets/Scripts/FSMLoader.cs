using System;
using System.Collections.Generic;
using States;
using UnityEngine;

public static class FsmLoader
{
    [Serializable]
    private struct JsonPredicate
    {
        public string property;
        public string op;
        public float value;
    }

    [Serializable]
    private struct JsonTransition
    {
        public string from;
        public string to;
        public JsonPredicate predicate;
    }

    [Serializable]
    private struct JsonFsm
    {
        public string startState;
        public JsonTransition[] anyTransitions;
        public JsonTransition[] transitions;
    }

    public static void Load(FiniteStateMachine finiteStateMachine, TextAsset fsmJson, List<State> states)
    {
        void At(State from, State to, Func<bool> condition) =>
            finiteStateMachine.AddTransition(from, to, condition);

        var stateMap = new Dictionary<string, State>();
        foreach (var state in states)
            stateMap[state.GetType().Name] = state;

        var fromJson = JsonUtility.FromJson<JsonFsm>(fsmJson.text);
        foreach (var anyTransition in fromJson.anyTransitions)
        {
            var to = stateMap[anyTransition.to];
            var propertyName = anyTransition.predicate.property;
            var op = anyTransition.predicate.op;
            var value = anyTransition.predicate.value;

            var found = Enum.TryParse(propertyName, out Property property);
            if (!found) continue;
            object[] parameters = {property, value};
            // TODO: Using the "to" state as the caller makes no sense:
            var condition = (Func<bool>) to.GetType().GetMethod(op)?.Invoke(to, parameters);
            finiteStateMachine.AddAnyTransition(to, condition);
        }

        foreach (var transition in fromJson.transitions)
        {
            var from = stateMap[transition.from];
            var to = stateMap[transition.to];
            var propertyName = transition.predicate.property;
            var op = transition.predicate.op;
            var value = transition.predicate.value;

            var found = Enum.TryParse(propertyName, out Property property);
            if (!found) continue;
            object[] parameters = {property, value};
            var condition = (Func<bool>) from.GetType().GetMethod(op)?.Invoke(from, parameters);
            At(from, to, condition);
        }

        stateMap.TryGetValue(fromJson.startState, out var startState);
        finiteStateMachine.SetState(startState);
    }
}