using System;

namespace Code.Runtime.Triggers
{
    public class TestTrigger : Trigger<Single>
    {
        private State<Single> _targetState;
        private Single _number;

        public override bool IsHit()
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }
    
        public override IState PrepareTarget()
        {
            _targetState.ResetStateParameters(_number);
            return _targetState;
        }
    
        public static (string,Type)[] GetParameterList()
        {
            return new(string, Type)[]
            {
                ("number", typeof(Single)),
            };
        }
    }
}