using System;
using Code.Runtime;
using UnityEngine;

public class AbobaState : State<Vector3, Single, Rigidbody>
{
    private Vector3 _positionA;
    private Single _duration;
    private Rigidbody _rigidbody;

    public override void ResetStateParameters(Vector3 param0, Single param1, Rigidbody param2)
    {
        _positionA = param0;
        _duration = param1;
        _rigidbody = param2;
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
        return new(string, Type)[]
        {
            ("positionA", typeof(Vector3)),
            ("duration", typeof(Single)),
            ("rigidbody", typeof(Rigidbody)),
        };
    }
}