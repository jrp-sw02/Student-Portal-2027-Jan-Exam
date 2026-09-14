using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace EConnect.NIELIT
{
    public class projEmailMobExempt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("projID")]
        [Display(Name = "projID")]
        public Int64 projID { get; set; }

        [Column("EmailExempt")]
        [StringLength(1)]
        public string EmailExempt { get; set; }

        [Column("MobileExempt")]
        [StringLength(1)]
        public string MobileExempt { get; set; }

        [Column("enterBy", TypeName = "int")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }
    }
}

