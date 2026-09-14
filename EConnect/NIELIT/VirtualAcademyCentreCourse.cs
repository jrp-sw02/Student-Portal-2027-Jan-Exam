using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class VirtualAcademyCentreCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("centreID", TypeName = "bigint")]
        public Int64 centreID { get; set; }

        [Column("courseCatID", TypeName = "bigint")]
        public Int64 courseCatID { get; set; }

        [Column("courseID", TypeName = "bigint")]
        public Int64 courseID { get; set; }

        [Column("isActive", TypeName = "bit")]
        [Display(Name = "isActive")]
        public bool isActive { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Required(ErrorMessage = "Enter By is required")]
        public Int64 enterByID { get; set; }

        [Column("enterDate", TypeName = "DateTime")]
        public DateTime enterDate { get; set; }
    }
}
