using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class ProjectEligibility
    {
        [Key, Column("id", TypeName = "int", Order = 0)]        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column("id", TypeName = "int")]
        public Int32 id { get; set; }

        [Key, Column("course_level_id", TypeName = "int", Order = 1)]
        //[Column("course_level_id")]        
        [Required(ErrorMessage = "course level id is required")]
        [Display(Name = "course level id")]
        public Int32 course_level_id { get; set; }

        [Key, Column("project_name", Order = 2)]
        //[Column("project_name")]        
        [Required(ErrorMessage = "project name is required")]
        [MaxLength(200)]
        [Display(Name = "project name")]
        public String project_name { get; set; }

        [Key, Column("project_shortname", Order = 3)]
        //[Column("project_shortname")]        
        [Required(ErrorMessage = "project short name is required")]
        [MaxLength(200)]
        [Display(Name = "project short name")]
        public String project_shortname { get; set; }

        [Column("project_sequence_number")]       
        [Required(ErrorMessage = "project sequence number is required")]
        [Display(Name = "project sequence number")]
        public Int32? project_sequence_number { get; set; }

        [Column("min_theory_modules_passed")]        
        [Required(ErrorMessage = "min passed theory modules is required")]
        [Display(Name = "min passed theory modules")]
        public Int32 min_theory_modules_passed { get; set; }

        [Column("passed_theory_modules_number_from")]        
        [Required(ErrorMessage = "passed theory modules number from is required")]
        [Display(Name = "passed theory modules number from")]
        public Int32? passed_theory_modules_number_from { get; set; }

        [Column("passed_theory_modules_number_upto")]        
        [Required(ErrorMessage = "passed theory modules number upto is required")]
        [Display(Name = "passed theory modules number upto")]
        public Int32? passed_theory_modules_number_upto { get; set; }

        [Column("min_theory_modules_appeared")]        
        [Required(ErrorMessage = "min appeared theory modules is required")]
        [Display(Name = "min appeared theory modules")]
        public Int32 min_theory_modules_appeared { get; set; }

        [Column("appeared_theory_modules_number_from")]        
        [Required(ErrorMessage = "appeared theory modules number from is required")]
        [Display(Name = "appeared theory modules number from")]
        public Int32? appeared_theory_modules_number_from { get; set; }

        [Column("appeared_theory_modules_number_upto")]        
        [Required(ErrorMessage = "appeared theory modules number_upto is required")]
        [Display(Name = "appeared theory modules number upto")]
        public Int32? appeared_theory_modules_number_upto { get; set; }

        [Column("passed_project_modules_code")]        
        [Required(ErrorMessage = "passed project modules code is required")]
        [MaxLength(200)]
        [Display(Name = "passed project modules code is required")]
        public String passed_project_modules_code { get; set; }        

        [Column("min_attempt_allowed")]        
        [Required(ErrorMessage = "min attempt allowed is required")]
        [Display(Name = "min attempt allowed")]
        public Int32? min_attempt_allowed { get; set; }

        [Column("whether_max_revision_to_show", TypeName = "bit")]
        public Boolean whether_max_revision_to_show { get; set; }

        [Column("whether_level_upgreaded_exemption_require", TypeName = "bit")]
        public Boolean whether_level_upgreaded_exemption_require { get; set; }

        [Column("effective_from_date")]
        [Required(ErrorMessage = "effective from Ddate is required")]
        public DateTime? effective_from_date { get; set; }

        [Key, Column("whether_effective", TypeName = "bit", Order = 4)]
        //[Column("whether_effective", TypeName = "bit")]
        public Boolean whether_effective { get; set; }


    }
}
