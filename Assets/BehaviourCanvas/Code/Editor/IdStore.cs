namespace Code.Editor
{
    public class IdStore
    {
        public int ID => _id++;
        private int _id;
        
        public IdStore(int id)
        {
            _id = id;
        }
    }
}