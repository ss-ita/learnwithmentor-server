namespace LearnWithMentorDAL.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UERS_ROLES
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Roles_Name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Email { get; set; }
    }
}
