using System;
using Code.Runtime;
using UnityEngine;

public class TestState : State<Vector3, GameObject>
{
    private Vector3 _positionA;
    private GameObject _abobaGameObject;

    public override void ResetStateParameters(Vector3 param0, GameObject param1)
    {
        _positionA = param0;
        _abobaGameObject = param1;
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
            ("abobaGameObject", typeof(GameObject)),
        };
    }
}