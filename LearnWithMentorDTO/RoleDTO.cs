namespace LearnWithMentorDTO
{
    public class RoleDTO
    {
        public RoleDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
