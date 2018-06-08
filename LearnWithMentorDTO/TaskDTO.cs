using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public class TaskDTO
    {
        public TaskDTO(int id,
                        string name,
                        string description,
                        bool _private,
                        int createId,
                        Nullable<int> modId,
                        Nullable<System.DateTime> createDate,
                        Nullable<System.DateTime> modDate,
                        Nullable<int> priority,
                        Nullable<int> sectionId)
        {
            Id = id;
            Name = name;
            Description = description;
            Private = _private;
            Create_Date = createDate;
            Mod_Date = modDate;
            Creator_Id = createId;
            Modifier_Id = modId;
            Priority = priority;
            Section_Id = sectionId;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public int Creator_Id { get; set; }
        public Nullable<int> Modifier_Id { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public Nullable<System.DateTime> Mod_Date { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> Section_Id { get; set; }
    }
}
