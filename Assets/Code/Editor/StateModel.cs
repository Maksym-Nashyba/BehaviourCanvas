using UnityEngine;

namespace Code.Editor
{
    public class StateModel : Model
    {
        public StateModel(int id, (string, string)[] parameters, Rect newPos) : base(id, parameters, newPos)
        {
        }
    }
}