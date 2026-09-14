using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
    public class DLCExemptedStates
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("StateID", TypeName = "bigint")]
        public Int64 StateID { get; set; }

        [Column("fromDate")]
        public DateTime fromDate { get; set; }

        [Column("toDate")]
        public DateTime? toDate { get; set; }

        [Column("enterBy", TypeName = "bigint")]
        [Display(Name = "User Name")]
        public Int32 enterBy { get; set; }

        [Column("enterDate")]
        public DateTime enterDate { get; set; }

        [ForeignKey("StateID")]
        public virtual Location State { get; set; }


    }
}
