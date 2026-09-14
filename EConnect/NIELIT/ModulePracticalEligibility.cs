using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class ModulePracticalEligibility
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Practical_Module_ID", TypeName = "int")]
        [Required(ErrorMessage = "Practical Module Name is required")]
        public Int32 PracticalModuleID { get; set; }

        [Column("Theory_Module_ID", TypeName = "int")]
        [Required(ErrorMessage = "Theory Module Name is required")]
        public Int32 TheoryModuleID { get; set; }

        [Column("Selection_Type_ID", TypeName = "int")]
        public Int32? SelectionTypeID { get; set; }
        
        [Column("Elective_Group", TypeName = "int")]
        public Int32? ElectiveGroup { get; set; }

        [Column("Created_By", TypeName = "int")]
        [Required(ErrorMessage = "Created By User ID is required")]
        public Int32 CreatedBy { get; set; }

        [Column("Created_On")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("SelectionTypeID")]
        public virtual SelectionType SelectionType { get; set; }

        [ForeignKey("PracticalModuleID")]
        public virtual Module PracticalModule { get; set; }

        [ForeignKey("TheoryModuleID")]
        public virtual Module TheoryModule { get; set; }
    }
}
