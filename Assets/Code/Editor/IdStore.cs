namespace Code.Editor
{
    public class IdStore
    {
        public int ID => _id++;
        private int _id;
        
        public IdStore(int id = 1)
        {
            _id = id;
        }
    }
}