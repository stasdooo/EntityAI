namespace GoalAI.Core


{
    public class Entity
    {
        private List<IComponent> components = new List<IComponent>();

        public IReadOnlyList<IComponent> Components 
        {
            get { return components; } 
        }
        public string Name { get; private set; }

        public Entity(string Name) 
        {
            this.Name = Name;
        }

        public T AddComponent<T>(T component)where T:IComponent
        {
            components.Add(component);
            return component;
        }

        public T? GetComponent<T>() where T: IComponent
        {
            return components.OfType<T>().FirstOrDefault();
        }
    }
}
