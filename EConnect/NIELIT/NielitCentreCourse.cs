using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EConnect.NIELIT

{
    [Serializable]
   public class NielitCentreCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
         public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Nielit Centre Course Name is required")]
        [MaxLength(150)]
        [Display(Name = "NielitCentreCourse Name")]
        public String Name { get; set; }

        [Column("NAME_REGIONAL")]
        [MaxLength(150)]
        [Display(Name = "Course Regional Name")]
        public String NameRegional { get; set; }

        [Column("Code")]
        [Required(ErrorMessage = "Course Code is required")]
        [MaxLength(30)]
        [Display(Name = "Course Code")]
        public String Code { get; set; }
        
        [Column("Course_Category_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Category is required")]
        [Display(Name = "Course Category")]
        public Int32 CourseCategoryID { get; set; }

        [Column("NielitTrgSplID", TypeName = "bigint")]
        [Required(ErrorMessage = "NielitTrg SplID is required")]
        [Display(Name = "NielitTrgSplID")]
        public Int32 NielitTrgSpclID { get; set; }

        [Column("Display_Order", TypeName = "int")]
        [Required(ErrorMessage = "Display Order is required")]
        [Display(Name = "Display Order")]
        public Int32 DisplayOrder { get; set; }       

        [Column("whetherShortTerm", TypeName = "bit")]
        [Display(Name = "whether Short Term ")]
        public bool whetherShortTerm { get; set; }

        [Column("Show_On_Web", TypeName = "bit")]
        public bool ShowOnWeb { get; set; }

        [Column("IsActive", TypeName = "bit")]
        public bool IsActive { get; set; }

        [Column("IsVerified", TypeName = "bit")]
         public bool? IsVerified { get; set; }


        [Column("verifiedOn")]
        public DateTime? verifiedOn { get; set; }


        [Column("verifiedBy", TypeName = "int")]
         public Int32?  verifiedBy { get; set; }

        [Column("verifiedStatus", TypeName = "int")]
        public Int32? verifiedStatus { get; set; }

        [Column("verificationMessage")]
        [MaxLength(500)]
        public String verificationMessage { get; set; }

        [Column("verificationStatusDate")]
        public DateTime? verificationStatusDate { get; set; }

        [Column("enterBy", TypeName = "int")]
        public int enterBy { get; set; }

        [Column("enterDate")]
         public DateTime  enterDate { get; set; }

        [Column("Remarks")]
       // [Required(ErrorMessage = "Remarks is required")]
        [MaxLength(150)]
        [Display(Name = "Remarks for nielit centre course")]
        public String remarks { get; set; }

        [Column("ExistingCourseID", TypeName = "int")]       
        [Display(Name = "Display Order")]
        public Int32 ExistingCourseID { get; set; }

       
    }
}
