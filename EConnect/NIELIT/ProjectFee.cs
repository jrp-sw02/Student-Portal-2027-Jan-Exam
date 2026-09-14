using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class ProjectFee
    {
        [Key, Column("id", TypeName = "int", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column("id", TypeName = "int")]
        public Int32 id { get; set; }

        [Key, Column("project_name", Order = 1)]
        //[Column("project_name")]        
        [Required(ErrorMessage = "project name is required")]
        [MaxLength(200)]
        [Display(Name = "project name")]
        public String project_name { get; set; }

        [Key, Column("project_short_name", Order = 2)]
        //[Column("project_short_name")]        
        [Required(ErrorMessage = "project short name is required")]
        [MaxLength(10)]
        [Display(Name = "project short name")]
        public String project_short_name { get; set; }

        [Column("project_sequence_number")]        
        [Required(ErrorMessage = "project sequence number is required")]
        [Display(Name = "project sequence number")]
        public Int32 project_sequence_number { get; set; }

        [Key, Column("course_level_id", TypeName = "int", Order = 3)]
        //[Column("course_level_id")]        
        [Required(ErrorMessage = "course level id is required")]
        [Display(Name = "course level id")]
        public Int32 course_level_id { get; set; }        

        [Column("fee_amount")]        
        [Required(ErrorMessage = "fee amount is required")]
        [Display(Name = "fee amount")]
        public double fee_amount { get; set; }

        [Column("effective_from_date")]
        [Required(ErrorMessage = "effective from date is required")]
        public DateTime effective_from_date { get; set; }

        [Key, Column("whether_effective", Order = 4)]
        //[Column("whether_effective")]
        [Required(ErrorMessage = "whether effective is required")]
        [MaxLength(1)]
        [Display(Name = "whether effective")]
        public String whether_effective { get; set; }

        [Column("demandnote_validity_in_days")]       
        [Required(ErrorMessage = "demandnote validity in days is required")]
        [Display(Name = "demandnote validity in days")]
        public Int32 demandnote_validity_in_days { get; set; }

        [Column("neft_extension_period")]        
        [Required(ErrorMessage = "neft extension period is required")]
        [Display(Name = "neft extension period")]
        public Int32 neft_extension_period { get; set; }
        
    }
}
