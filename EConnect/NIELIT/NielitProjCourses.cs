using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT
{
    public class NielitProjCourses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("projID", TypeName = "bigint")]
        [Required(ErrorMessage = "Project is Required")]
        [Display(Name = "Project")]
        public Int64 projID { get; set; }

        [Column("courseID", TypeName = "bigint")]
        [Required(ErrorMessage = "Course is Required")]
        [Display(Name = "Course")]
        public Int64 courseID { get; set; }

        [Column("IsActive", TypeName = "bit")]
        [Display(Name = "Is Active")]
        public Boolean IsActive { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

       

    }
}

