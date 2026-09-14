using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
  public  class NSQFFreeCourseGrant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("instituteID", TypeName = "bigint")]
        [Display(Name = "institute ID")]
        public Int64 InstituteID { get; set; }

        [Column("accrCourseID", TypeName = "bigint")]
        [Display(Name = "accrCourse ID")]
        public Int64 accrCourseID { get; set; }

        [Column("mappedCourseID", TypeName = "bigint")]
        [Required(ErrorMessage = "Bucket Course ID is required")]
        [Display(Name = "Mapped Course ID")]
        public Int64 mappedCourseID { get; set; }

        [Column("undertakingSignatory")]
        public string undertakingSignatory { get; set; }

        [Column("undertakingSigDesig")]
        public string undertakingSigDesig { get; set; }

        [Column("undertakingSigAddress")]
        public string undertakingSigAddress { get; set; }

        [Column("Remarks")]
        public string Remarks { get; set; }

        [Column("grantDate")]
        public DateTime? grantDate { get; set; }

        [Column("isActive", TypeName = "bit")]
        [Display(Name = "Is isActive")]
        public Boolean isActive { get; set; }

        [Column("eFileNo")]
        public string eFileNo { get; set; }

        [Column("approvalDate")]
        public DateTime? approvalDate { get; set; }


        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        public Int64 enterBy { get; set; }
    }
}
