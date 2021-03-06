using System;
using UnityEngine;

public enum Property
{
    DeltaTime,
    StopWatch
}

public abstract class State
{
    public float DeltaTime { get; private set; }
    public float StopWatch { get; private set; }
    protected readonly Animator Animator;

    public Func<bool> Eq(Property property, float value)
    {
        switch (property)
        {
            case Property.DeltaTime:
                return () => Math.Abs(DeltaTime - value) < Mathf.Epsilon;
            case Property.StopWatch:
                return () => Math.Abs(StopWatch - value) < Mathf.Epsilon;
            default:
                return () => true;
        }
    }

    public Func<bool> Gt(Property property, float value)
    {
        switch (property)
        {
            case Property.DeltaTime:
                return () => DeltaTime > value;
            case Property.StopWatch:
                return () => StopWatch > value;
            default:
                return () => true;
        }
    }

    public Func<bool> Lt(Property property, float value)
    {
        switch (property)
        {
            case Property.DeltaTime:
                return () => DeltaTime < value;
            case Property.StopWatch:
                return () => StopWatch < value;
            default:
                return () => true;
        }
    }

    protected State(Animator animator)
    {
        DeltaTime = StopWatch = 0;
        Animator = animator;
    }

    protected abstract void Tick();
    public abstract void OnEnter();
    public abstract void OnExit();

    public void Tick(float deltaTime)
    {
        // Update the StopWatch and then execute the specific actions:
        StopWatch += DeltaTime = deltaTime;
        Tick();
    }

    public void ResetTimer() => StopWatch = 0;
}