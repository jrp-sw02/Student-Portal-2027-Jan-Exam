using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public  class ApplicationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        public String Name { get; set; }

        [Column("Code")]
        [MaxLength(10)]
        public String Code { get; set; }

        [Column("Course_Type_ID", TypeName = "int")]
        [Required(ErrorMessage = "Course Type is required")]
        public Int32 CourseTypeID { get; set; }

        [NotMapped]
        public virtual  enmCourseType enmCourseType
        {
            get
            {
                return (enmCourseType)this.CourseTypeID;
            }
            set
            {
                this.CourseTypeID = Convert.ToInt32(value);
            }
        }

        [ForeignKey("CourseTypeID")]
        public virtual CourseType CourseType { get; set; }

        public virtual ICollection<Batch> Batches { get; set; }
    }
}
