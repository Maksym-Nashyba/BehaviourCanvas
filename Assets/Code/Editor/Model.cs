using UnityEngine;

namespace Code.Editor
{
    public class Model
    {
        public readonly int ID;
        public readonly (string, string)[] Parameters;
    
        public Model(int id, (string, string)[] parameters, Rect newPos)
        {
            ID = id;
            Parameters = parameters;
        }
    }
}