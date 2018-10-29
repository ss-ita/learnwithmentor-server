namespace LearnWithMentorDTO
{
    public class UserGroupDTO
    {
        public UserGroupDTO(int id, int userId, int groupId)
        {
            Id = id;
            UserId = userId;
            GroupId = groupId;
        }

        public int Id { get; set;} 
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}
