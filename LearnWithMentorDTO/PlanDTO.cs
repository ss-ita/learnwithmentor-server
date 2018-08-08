using System;
using System.ComponentModel.DataAnnotations;
using LearnWithMentorDTO.Infrastructure;

namespace LearnWithMentorDTO
{
    public class PlanDTO
    {
        public PlanDTO() { }
        public PlanDTO(int id,
            string name,
            string description,
            bool published,
            int creatorid,
            string creatorfirstname,
            string creatorlastname,
            int? modid,
            string modfirstname,
            string modlastname,
            DateTime? createdate,
            DateTime? moddate

            )
        {
            Id = id;
            Name = name;
            Description = description;
            Published = published;
            CreateDate = createdate;
            ModDate = moddate;
            Modid = modid;
            CreatorId = creatorid;
            CreatorFirstName = creatorfirstname;
            CreatorLastName = creatorlastname;
            ModFirstName= modfirstname;
            ModLastName= modlastname;

        }
        public int Id { get; set; }
        [Required]
        [StringLength(ValidationRules.MAX_PLAN_NAME_LENGTH,
            ErrorMessage = "Plan name too long")]
        public string Name { get; set; }
        [Required]
        [StringLength(ValidationRules.MAX_PLAN_DESCRIPTION_LENGTH,
            ErrorMessage = "Plan description too long")]
        public string Description { get; set; }
        [Required]
        public bool Published { get; set; }
        public int CreatorId { get; set; }
        public string CreatorFirstName { get; set; }
        public string CreatorLastName { get; set; }
        public int? Modid { get; set; }
        public string ModFirstName { get; set; }
        public string ModLastName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModDate { get; set; }
    }
}