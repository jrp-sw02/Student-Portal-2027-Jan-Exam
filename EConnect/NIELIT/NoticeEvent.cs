using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
   public class NoticeEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Activity_Name")]
        [MaxLength(50)]
        [Required]
        public String ActivityName { get; set; }

        [Column("Activity_Description")]
        [MaxLength(300)]
        [Required]
        public String ActivityDescription { get; set; }

        [Column("Start_Date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Column("End_Date")]
        [Required]
        public DateTime EndDate { get; set; }

    }
}
