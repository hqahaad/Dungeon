using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateMachine
{
    private StateNode currentNode;

    private Dictionary<Type, StateNode> nodes = new();
    private HashSet<ITransition> anyTransitions = new();

    public void Update()
    {
        var transitions = GetTransition();

        if (transitions != null)
        {
            ChangeState(transitions.To);
        }

        currentNode?.State?.Update();
    }

    public void FixedUpdate()
    {
        currentNode?.State?.FixedUpdate();
    }

    public void SetState(IState state)
    {
        currentNode = nodes[state.GetType()];
        currentNode.State?.OnEnter();
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
    }

    private void ChangeState(IState state)
    {
        if (state == currentNode.State) return;

        var prevState = currentNode.State;
        var nextState = nodes[state.GetType()].State;

        prevState?.OnExit();
        nextState?.OnEnter();

        currentNode = nodes[state.GetType()];
    }

    private ITransition GetTransition()
    {
        foreach (var iter in anyTransitions)
        {
            if (iter.Condition.Evaluate())
            {
                return iter;
            }
        }

        foreach (var iter in currentNode.Transitions)
        {
            if (iter.Condition.Evaluate())
            {
                return iter;
            }
        }

        return null;
    }

    private StateNode GetOrAddNode(IState state)
    {
        var node = nodes.GetValueOrDefault(state.GetType());

        if (node == null)
        {
            node = new StateNode(state);
            nodes.Add(state.GetType(), node);
        }

        return node;
    }
}

public class StateNode
{
    public IState State { get; }
    public HashSet<ITransition> Transitions { get; }

    public StateNode(IState state)
    {
        State = state;
        Transitions = new();
    }

    public void AddTransition(IState to, IPredicate condition)
    {
        Transitions.Add(new Transition(to, condition));
    }
}
