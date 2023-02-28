namespace Code.Editor
{
    public abstract class TreeModel
    {
        public int ID { get; set; }
        public Model Model { get; set; }
    
        protected TreeModel()
        {
            ID = 0;
            Model = new Model();
        }
    
        protected TreeModel(int id, string name, (string, string)[] parameters)
        {
            ID = id;
            Model = new Model(name, parameters);
        }
    
        protected TreeModel(int id, Model model) : this(id, model.Name, model.Parameters)
        {
        
        }
    }
}