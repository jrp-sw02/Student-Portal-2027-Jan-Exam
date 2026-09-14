using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{


    public class CourseENCAaddhar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("encAddh")]
        [MaxLength]
        public String encAddh { get; set; }

        [Column("CourseRegID", TypeName = "bigint")]
        public Int64? CourseRegID { get; set; }


        [Column("enterBy", TypeName = "bigint")]
        public Int64? enterBy { get; set; }

        [Column("enterDate")]
        public DateTime? enterDate { get; set; }

    }

}
