using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect
{
    public class DebarredCandidate
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id", TypeName = "bigint")]
        public Int64 id { get; set; }

        [Column("registration_number", TypeName = "bigint")]
        //[Column("course_id", TypeName = "int")]
        [Required(ErrorMessage = "registration number is required")]
        [Display(Name = "registration number")]
        public Int64 registration_number { get; set; }

        [Column("course_id", TypeName = "int")]
        //[Column("course_id", TypeName = "int")]
        [Required(ErrorMessage = "course id is required")]
        [Display(Name = "course id")]
        public Int16 course_id { get; set; }

        [Column("debarred_for_examcycle", TypeName = "int")]
        //[Column("course_id", TypeName = "int")]
        [Required(ErrorMessage = "debarred for examcycle is required")]
        [Display(Name = "debarred for examcycle")]
        public Int16 debarred_for_examcycle { get; set; }

        [Column("debarred_upto_examcycle_count", TypeName = "int")]
        //[Column("course_id", TypeName = "int")]
        [Required(ErrorMessage = "debarred upto examcycle count")]
        [Display(Name = "debarred upto examcycle count")]
        public Int16 debarred_upto_examcycle_count { get; set; }

        [Column("whether_active")]
        [Required(ErrorMessage = "whether active is required")]
        [MaxLength(1)]
        [Display(Name = "whether active")]
        public String whether_active { get; set; }
    }
}
