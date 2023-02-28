using System;
using Code.Runtime;
using UnityEngine;

public class TestState : State
{
    

    public override void ResetStateParameters()
    {
        
    }

    public override void Start()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public override void End()
    {
        throw new NotImplementedException();
    }
    
    public static (string,Type)[] GetParameterList()
    {
        return Array.Empty<(string, Type)>();
    }
}