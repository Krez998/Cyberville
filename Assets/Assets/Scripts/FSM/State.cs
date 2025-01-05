using UnityEngine;

public abstract class State
{
    protected readonly StateMachine StateMachine;

    protected State(StateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void HandleInput() { }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }

    public virtual void OnTriggerEnter(Collider other) { }

    public virtual void OnTriggerExit(Collider other) { }

    public virtual void Exit() { }
}