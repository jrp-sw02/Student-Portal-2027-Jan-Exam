using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class Module
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Course_Category_ID", TypeName = "int")]
        [Required]
        public Int32 CourseCategoryID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required]
        public Int32 CourseID { get; set; }

        [Column("Module_Type_ID", TypeName = "int")]
        [Required]
        public Int32 ModuleTypeID { get; set; }

        [Column("Selection_Type_ID", TypeName = "int")]
        [Required]
        public Int32 SelectionTypeID { get; set; }

        [Column("Elective_Group", TypeName = "int")]
        public Int32? ElectiveGroup { get; set; }

        [Column("Elective_Modules", TypeName = "int")]
        public Int32? NumberOfElectiveModulesAllowed { get; set; }

        [Column("Revision_Number", TypeName = "int")]
        [Required]
        public Int32 RevisionNumber { get; set; }

        [Column("Number", TypeName = "int")]
        public Int32 ModuleNUmber { get; set; }

        [Column("Sub_Number", TypeName = "int")]
        public Int32 ModuleSubNUmber { get; set; }

        [Column("Sub_Code", TypeName = "int")]
        public Int32? ModuleSubCode { get; set; }

        [Column("Short_Name")]
        [MaxLength(10)]
        public String ShortName { get; set; }

        [Column("Name")]
        [MaxLength(150)]
        [Required]
        public String Name { get; set; }

        [Column("NumberOfHours")]
        public int? Hours { get; set; }

        [Column("NumberOfCredits")]
        public int? Credit { get; set; }

        [Column("Synopsis_Required", TypeName = "bit")]
        [Required]
        public Boolean SynopsisRequired { get; set; }

        [Column("Project_Number", TypeName = "int")]
        public Int32? ProjectNumber { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveFromDate { get; set; }

        [Column("Effective_To_Date")]
        public DateTime? EffectiveToDate { get; set; }

        [Column("Code", TypeName = "int")]
        [Required]
        public Int32? Code { get; set; }

        [Column("Exam_Mode_ID", TypeName = "int")]
        public Int32? ExamModeId { get; set; }

        [NotMapped]
        public enmModuleType enmModuleType
        {
            get
            {
                return (enmModuleType)this.ModuleTypeID;
            }
            set
            {
                this.ModuleTypeID = (int)value;
            }
        }
        [NotMapped]
        public enmSelectionType enmSelectionType
        {
            get
            {
                return (enmSelectionType)this.SelectionTypeID;
            }
            set
            {
                this.SelectionTypeID = (int)value;
            }
        }

        [ForeignKey("ModuleTypeID")]
        public virtual ModuleType ModuleType { get; set; }

        [ForeignKey("SelectionTypeID")]
        public virtual SelectionType SelectionType { get; set; }

        [ForeignKey("CourseCategoryID")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [Column("Exam_Pattern", TypeName = "smallint")]
        public Int16? ExamPattern { get; set; }

        [Column("Max_Marks", TypeName = "smallint")]
        public Int16? MaxMarks { get; set; }

        [Column("TheoryWithPractical", TypeName = "int")]
        //     [Required]
        public Int16 TheoryWithPractical { get; set; }
    }
}
