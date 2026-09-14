using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class CourseWiseState
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "int")]
        public Int32 ID { get; set; }

        [Column("Regional_Center_ID", TypeName = "int")]
        [Required]
        public Int32 RegionalCenterID { get; set; }

        [Column("Course_ID", TypeName = "int")]
        [Required]
        public Int32 CourseID { get; set; }

        [Column("State_ID", TypeName = "bigint")]
        [Required]
        public Int64 StateID { get; set; }

        //Added 2 Jan 2023 
        [Column("Whether_distribute_district_wise", TypeName = "int")]
        public Int32? Whetherdistributedistrictwise { get; set; }

        [ForeignKey("StateID")]
        public virtual Location State { get; set; }
        public virtual Course Course { get; set; }
        public virtual RegionalCenter RegionalCenter { get; set; }
    }
}
