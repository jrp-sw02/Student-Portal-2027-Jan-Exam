using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EConnect.NIELIT
{
 public   class CourseMappingWithNSQFCoursecode
    {
     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     [Column("ID", TypeName = "int")]
     public Int32 ID { get; set; }

     [Column("CourseID", TypeName = "int")]     
     public Int32? CourseID { get; set; }

     [Column("NSQFCourseCode")]     
     [MaxLength(4)]
     public String NSQFCourseCode { get; set; }

     [Column("NSQF_Aligned_at_Level", TypeName = "int")]
     public Int32? NSQF_Aligned_at_Level { get; set; }

     [Column("NSQFLearnHourType", TypeName = "int")]
     public Int32? NSQFLearnHourType { get; set; }
    }
}
