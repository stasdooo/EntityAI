namespace GoalAI.Core


{
    public class Entity
    {
        private List<IComponent> components = new List<IComponent>();
        public string Name { get; private set; }

        public Entity(string Name) 
        {
            this.Name = Name;
        }

        public void AddComponent(IComponent component)
        {
            components.Add(component);
            
        }

        public T? GetComponent<T>() where T: IComponent
        {
            return components.OfType<T>().FirstOrDefault();
        }
    }
}
