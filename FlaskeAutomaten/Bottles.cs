namespace FlaskeAutomaten
{
    public class Bottles
    {
        public string Name { get; private set; }

        public int Id { get; private set; }

        public Bottles(string name, int id)
        {
            Name = name;
            Id = id;
        }
    }
}
