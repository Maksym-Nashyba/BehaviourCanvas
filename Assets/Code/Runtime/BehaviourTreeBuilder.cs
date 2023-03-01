using System;
using UnityEngine;

namespace Code.Runtime
{
    public sealed class BehaviourTreeBuilder
    {
        public BehaviourTree BuildTree(BehaviourTreeAsset blueprint)
        {
            TextAsset textAsset = blueprint.BehaviourTreeXML;
            throw new NotImplementedException();
        }
    }
}