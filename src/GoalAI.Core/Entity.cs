namespace GoalAI.Core


{

    /// <summary>
    /// Container object that groups components together to form a unit in the game world
    /// </summary>
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

        // Finds the first component of the given type, or null if none exist
        public T? GetComponent<T>() where T: IComponent
        {
            return components.OfType<T>().FirstOrDefault();
        }
    }
}
