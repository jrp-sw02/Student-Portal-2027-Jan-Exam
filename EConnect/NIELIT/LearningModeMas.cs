using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EConnect.NIELIT
{
   public  class LearningModeMas 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; } 

        [Column("Code")]
        [Required(ErrorMessage = "Learning Code is required")]
        [MaxLength(20)]
        [Display(Name = "Learning Code")]
        public String Code { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Learning Name is required")]
        [MaxLength(200)]
        [Display(Name = "Learning Name")]
        public String Name { get; set; }

        [Column("Description")]
        [MaxLength(1000)]
        [Display(Name = "Learning Description")]
        public String Description { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int64 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}
