using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IPredicate
{
    bool Evaluate();
}

public class BoolPredicate : IPredicate
{
    private bool isTrue;

    public BoolPredicate(bool value)
    {
        isTrue = value;
    }

    public bool Evaluate() => isTrue;
}

public class FuncPredicate : IPredicate
{
    private readonly Func<bool> func;

    public FuncPredicate(Func<bool> func)
    {
        this.func = func;
    }

    public bool Evaluate() => func.Invoke();
}
