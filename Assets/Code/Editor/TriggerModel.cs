using UnityEngine;

namespace Code.Editor
{
    public class TriggerModel : Model
    {
        public bool ResetTarget;
    
        public TriggerModel(int id, (string, string)[] parameters, bool resetTarget, Rect newPos): base(id, parameters, newPos)
        {
            ResetTarget = resetTarget;
        }
    }
}