using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnWithMentorDTO
{
    public class TaskDTO
    {
        public TaskDTO(int _id,
                        string _name,
                        string _description,
                        bool _private,
                        int _create_id,
                        string _creator_name,
                        Nullable<int> _mod_id,
                        string _modifier_name,
                        Nullable<System.DateTime> _create_date,
                        Nullable<System.DateTime> _mod_date,
                        Nullable<int> _priority,
                        Nullable<int> _section_id)
        {
            Id = _id;
            Name = _name;
            Description = _description;
            Private = _private;
            Create_Date = _create_date;
            Mod_Date = _mod_date;
            Creator_Id = _create_id;
            Creator_Name = _creator_name;
            Modifier_Name = _modifier_name;
            Modifier_Id = _mod_id;
            Priority = _priority;
            Section_Id = _section_id;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public int Creator_Id { get; set; }
        public string Creator_Name { get; set; }
        public Nullable<int> Modifier_Id { get; set; }
        public string Modifier_Name { get; set; }
        public Nullable<System.DateTime> Create_Date { get; set; }
        public Nullable<System.DateTime> Mod_Date { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> Section_Id { get; set; }
    }
}
