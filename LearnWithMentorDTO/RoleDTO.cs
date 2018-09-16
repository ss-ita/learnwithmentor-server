namespace LearnWithMentorDTO
{
    public class RoleDto
    {
        public RoleDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
