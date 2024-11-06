namespace SignalRLab3.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
