using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class NSQFFreeCourseMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("courseID", TypeName = "int")]
        [Required(ErrorMessage = "Course ID is required")]
        [Display(Name = "Course ID")]
        public Int32 CourseID { get; set; }

        [Column("bucketCourseID", TypeName = "int")]
        [Required(ErrorMessage = "Bucket Course ID is required")]
        [Display(Name = "Mapped Course ID")]
        public Int32 mappedCourseID { get; set; }

        [Column("effectiveFrom")]
        public DateTime? effectiveFrom { get; set; }

        [Column("effectiveTo")]
        public DateTime? effectiveTo { get; set; }

        [Column("mappingApprovalDate")]
        public DateTime? mappingApprovalDate { get; set; }

        [Column("eFileNo")]
        public string eFileNo { get; set; }

        [Column("isReplacement")]
        [DefaultValue(true)]
        public bool isReplacement { get; set; }

       [Column("replacedCourseID", TypeName = "int")]
       [Display(Name = "Replaced Course ID")]
       public Int32 replacedCourseID { get; set; }
       
        [Column("enterDate")]
        public DateTime  enterDate { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32 enterBy { get; set; }
       
    }
}
