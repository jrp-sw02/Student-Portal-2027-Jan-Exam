using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class CourseFeetype
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int32 ID { get; set; }

        [Column("courseID", TypeName = "bigint")]
        [Required(ErrorMessage = "Course is Required")]
        [Display(Name = "Course Name")]
        public Int32 courseID { get; set; }

        [Column("feeTypeID", TypeName = "bigint")]
        [Required(ErrorMessage = "Fee type is Required")]
        [Display(Name = "Fee type")]
        public Int32 feeTypeID { get; set; }

        [Column("isActive", TypeName = "bit")]
        public bool isActive { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

       

    }
}

