using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect
{
    public class RevisionChoice
    {
        [Key, Column("course_id", TypeName = "int", Order= 0)]
        //[Column("course_id", TypeName = "int")]
        [Required(ErrorMessage = "Course Id is required")]
        [Display(Name = "course id")]
        public Int16 course_id { get; set; }

        [Key, Column("course_code", Order = 1)]
        //[Column("course_code")]
        [Required(ErrorMessage = "course code is required")]
        [MaxLength(100)]
        [Display(Name = "course code")]
        public String course_code { get; set; }

        [Key, Column("new_revision_number", TypeName = "int", Order = 2)]
        //[Column("new_revision_number",TypeName = "int")]
        [Required(ErrorMessage = "new revision number is required")]
        [Display(Name = "new revision number")]
        public Int16 new_revision_number { get; set; }

        [Column("previous_revision_number")]
        //[Column("new_revision_number",TypeName = "int")]
        [Required(ErrorMessage = "previous revision number is required")]
        [Display(Name = "previous revision number")]
        public Int16 previous_revision_number { get; set; }

        [Column("revision_choice_effective_date")]
        [Required(ErrorMessage = "revision choice effective date is required")]
        public DateTime revision_choice_effective_date { get; set; }
   
        [Column("whether_show_revision_choice")]
        [Required(ErrorMessage = "whether show revision choice is required")]
        [MaxLength(1)]
        [Display(Name = "whether show revision choice")]
        public String whether_show_revision_choice { get; set; }

        [Column("show_revision_choice_till_date")]
        [Required(ErrorMessage = "show revision choice till date is required")]
        public DateTime show_revision_choice_till_date { get; set; }

        [Column("registration_date_to_show")]
        [Required(ErrorMessage = "registration date to show is required")]
        public DateTime registration_date_to_show { get; set; }

        [Column("registration_date_show_upto")]
        [Required(ErrorMessage = "registration date show upto is required")]
        public DateTime registration_date_show_upto { get; set; }     
    
    }
}
