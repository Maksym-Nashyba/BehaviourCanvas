namespace Code.Editor
{
    public class IdStore
    {
        public int ID => _id++;
        private int _id;
        
        public IdStore()
        {
            _id = 1;
        }
    }
}