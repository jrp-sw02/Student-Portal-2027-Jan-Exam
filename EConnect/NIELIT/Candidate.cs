using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace EConnect.NIELIT
{
    public class Candidate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID", TypeName = "bigint")]
        public Int64 ID { get; set; }

        [Column("Salutaion")]
        [Required(ErrorMessage = "Salutaion is required")]
        [MaxLength(10)]
        [Display(Name = "Salutaion")]
        public String Salutation { get; set; }

        [Column("Name")]
        [Required(ErrorMessage = "Candidate Name is required")]
        [MaxLength(60)]
        [Display(Name = "Candidate Name")]
        public String Name { get; set; }

        [Column("Father_Name")]
        [MaxLength(60)]        
        [Display(Name = "Father's Name Name")]
        public String FatherName { get; set; }

        [Column("Guardian_Name")]
        [MaxLength(60)]
        public String GuardianName { get; set; }

        [Column("Mother_Name")]
        [MaxLength(60)]        
        [Display(Name = "Mother's Name Name")]
        public String MotherName { get; set; }

        [Column("Gender")]       
        [MaxLength(6)]
        [Display(Name = "Gender")]
        public String Gender { get; set; }

        [Column("Marital_Status_ID", TypeName = "int")]
        [Display(Name = "Marital Status")]
        public Int32? MaritalStatusID { get; set; }

        [Column("Dob")]
        [Display(Name="Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("Cast_Category_ID", TypeName = "int")]
        [Display(Name = "Cast Category")]
        public Int32? CastCategoryID { get; set; }

        [Column("Religion_ID", TypeName = "int")]
        [Display(Name = "Religion")]
        public Int32? ReligionID { get; set; }

        [Column("Occupation_ID", TypeName = "int")]
        [Display(Name = "Occupation")]
        public Int32? OccupationID { get; set; }

        [Column("Is_Handicaped", TypeName="bit")]
        [Display(Name = "Is Handicapped")]
        public Boolean IsHandicaped { get; set; }

        [Column("Is_Ex_Servicemane", TypeName = "bit")]
        [Display(Name = "Is Ex Servicemane")]
        public Boolean IsExServicemane { get; set; }

        [Column("Photo_File_ID", TypeName = "bigint")]
        public Int64? PhotoFileID { get; set; }

        [Column("Signature_File_ID", TypeName = "bigint")]
        public Int64? SignatureFileID { get; set; }

        [Column("Left_Thumb_File_ID", TypeName = "bigint")]
        public Int64? LeftThumbImpressionFileID { get; set; }

        [Column("Body_Mark")]
        [MaxLength(50)]
        [Display(Name = "Body Mark")]
        public String BodyMark { get; set; }

        [Column("Effective_From_Date")]
        public DateTime EffectiveFromDate { get; set; }

        [Column("Is_Lock", TypeName = "bit")]
        [Display(Name = "Is Lock")]
        public Boolean IsLocked { get; set; }

        [Column("Locked_On")]
        public DateTime? LockedOn { get; set; }

        [Column("Photo_File_Name")]
        [MaxLength(50)]
        public String PhotoFileName { get; set; }

        [Column("Photo", TypeName = "varbinary(Max)")]
        public Byte[] TempPhoto { get; set; }

        [Column("Signature_File_Name")]
        [MaxLength(50)]
        public String SignatureFileName { get; set; }

        [Column("Signature", TypeName = "varbinary(Max)")]
        public Byte[] TempSignature { get; set; }

        [Column("Thumb_File_Name")]
        [MaxLength(50)]
        public String LeftThumbFileName { get; set; }

        [Column("Thumb", TypeName = "varbinary(Max)")]
        public Byte[] TempLeftThumb { get; set; }

        [Column("Is_Verified", TypeName = "bit")]
        [Display(Name = "Is Verified")]
        public Boolean IsVerified { get; set; }

        [Column("Verified_On")]
        public DateTime? VerifiedOn { get; set; }

        [Column("Verified_By", TypeName = "bigint")]
        public Int32? VerifiedBy { get; set; }

        [Column("Is_Synced", TypeName = "bit")]
        [Display(Name = "Is Synced")]
        public Boolean IsSynced { get; set; }

        [Column("Synced_On")]
        public DateTime? SyncedOn { get; set; }

        //jk sah on 14-1-2021
        [Column("Is_EWS", TypeName = "bit")]
        [Display(Name = "Is EWS")]
        public Boolean Is_EWS { get; set; }


        public virtual MaritalStatus MaritalStatus { get; set; }
        public virtual CastCategory CastCategory { get; set; }
        public virtual Religion Religion { get; set; }

        [ForeignKey("PhotoFileID")]
        public virtual UploadedFile Photo { get; set; }

        [ForeignKey("SignatureFileID")]
        public virtual UploadedFile Signature { get; set; }

        [ForeignKey("LeftThumbImpressionFileID")]
        public virtual UploadedFile LeftThumbImpression { get; set; }

        [NotMapped]
        public enmMaritalStatus enmMaritalStatus
        {
            get
            {
                return (enmMaritalStatus)this.MaritalStatusID;
            }
            set
            {
                this.MaritalStatusID = (int)value;
            }
        }

        [NotMapped]
        public enmCastCategory enmCastCategory
        {
            get
            {
                return (enmCastCategory)this.CastCategoryID;
            }
            set
            {
                this.MaritalStatusID = (int)value;
            }
        }

        public virtual ICollection<CandidateContactDetail> ContactDetails { get; set; }

        public virtual Occupation Occupation { get; set; }

        public virtual ICollection<CandidateQualificationDetail> EducationalQualifications { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }

        public virtual ICollection<CandidateRequest> CandidateRequests { get; set; }

        public virtual ICollection<RegistrationDetail> RegistrationDetails { get; set; }
    }
}
