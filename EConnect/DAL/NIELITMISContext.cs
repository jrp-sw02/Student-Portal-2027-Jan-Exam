using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using EConnect.HRMS;
using EConnect.NIELIT;
using EConnect.URM;

/// <summary>
/// Summary description for EConnectContext
/// </summary>
namespace EConnect.DAL
{
    public class NIELITMISContext : DbContext
    {
        
        //Added 21 june 2019
        public DbSet<NielitCentres> NielitCentres { get; set; }
        public DbSet<NielitSectors> NielitSectorss { get; set; }
        public DbSet<NielitTrgSpecialization> NielitTrgSpecializations { get; set; }
        public DbSet<NielitCentreCourse> NielitCentreCourses { get; set; }
        public DbSet<NielitCentreCourseCategory> NielitCentreCourseCategorys { get; set; }
       // public DbSet<NielitCentreBatch> NielitCentreBatchs { get; set; }
        public DbSet<NielitProjects> NielitProjectss { get; set; }
	    public DbSet<NonAffInstitute> NonAffInstitutes { get; set; }
        public DbSet<AffInstitute> AffInstitutes { get; set; }
        public DbSet<NielitCentreBatch> NielitCentreBatchs { get; set; }
        public DbSet<feeTypeMas> feeTypeMas { get; set; }
        public DbSet<NielitCentreBatchFee> NielitCentreBatchFees { get; set; }
        public DbSet<SessionMas > SessionMass { get; set; }

	
	public DbSet<projectMainCentre> projectMainCentres { get; set; }
    public DbSet<projectSubCentre> projectSubCentres { get; set; }
	public DbSet<NielitCentreStudent> NielitCentreStudent { get; set; }
    public DbSet<companyMaster> companyMasters { get; set; }
    
   
    
       
        public DbSet<NielitCourseDuration> NielitCourseDurations { get; set; }
        
        public DbSet<NielitProjCourses> NielitProjCoursess { get; set; }
        public DbSet<CourseFeetype> CourseFeetypes { get; set; }
	public DbSet<NIELITStudentFeePaid> NIELITStudentFeePaids { get; set; }
	public DbSet<verifyStatusMas> verifyStatusMass { get; set; }
	public DbSet<companyMasterHistory> companyMasterHistorys { get; set; }
	public DbSet<aadhaarResponse> aadhaarResponses { get; set; }
	public DbSet<studentPlacementDetail> studentPlacementDetails { get; set; }
	public DbSet<centreTrgCalendar> centreTrgCalendar { get; set; }
	public DbSet<LearningModeMas> LearningModeMass { get; set; }
	 public DbSet<SubjectMaster> SubjectMaster { get; set; }
      public DbSet<SemesterMaster> SemesterMaster { get; set; }   
      public DbSet<SemesterSubjectMaster> SemesterSubjectMaster { get; set; }
      
      public DbSet<SemesterFeeMaster> SemesterFeeMaster { get; set; }
      public DbSet<SemesterFeePaid> SemesterFeePaid { get; set; }
      public DbSet<semesterResultMaster> semesterResultMaster { get; set; }
      public DbSet<StudentSemesterAcademicDetails> StudentSemesterAcademicDetails { get; set; }
	public DbSet<NonAffiliatedInstituteCoursesDetail> NonAffiliatedInstituteCoursesDetails { get; set; }
	public DbSet<NonAffiliatedInstituteCoursesDetailHistory> NonAffiliatedInstituteCoursesDetailHistorys { get; set;}
	public DbSet<SemesterDetail> SemesterDetail { get; set; }

	//Added for virtual Academy
      public DbSet<virtualAcademyRegistration> virtualAcademyRegistration { get; set; }
      public DbSet<VirtualAcademyCentreCourse> VirtualAcademyCentreCourse { get; set; }
      public DbSet<virtualAcademyCalendar> virtualAcademyCalendar { get; set; }
      public DbSet<MISCourse_Registraton_Policy> MISCourse_Registraton_Policy { get; set; }
      public DbSet<MISQualification_Eligibility> MISQualification_Eligibility { get; set; }
      public DbSet<VirtualAcademyCutOffDate> VirtualAcademyCutOffDate { get; set; }

      public DbSet<VirtualAcademyDemandNote> VirtualAcademyDemandNotes { get; set; }
      public DbSet<VirtualAcademyOnlineTransaction> VirtualAcademyOnlineTransaction { get; set; }
      public DbSet<VirtualAcademyCourseServiceID> VirtualAcademyCourseServiceIDs { get; set; }
      public DbSet<virtualAcademyCourseProspectus> virtualAcademyCourseProspectus { get; set; }
      public DbSet<virtualAcademyCourseImages> virtualAcademyCourseImages { get; set; }
      public DbSet<teachingLanguage> teachingLanguage { get; set; }

      
      public DbSet<VirtualAcademyOtpVerification> VirtualAcademyOtpVerifications { get; set; }

      
      public DbSet<virtualAcademyOnlineRefund> virtualAcademyOnlineRefund { get; set; }
      public DbSet<DocumentMas> DocumentMas { get; set; }
      //Added_New_Class_06_09_2024
      public DbSet<ProjectDocumentMaster> ProjectDocumentMasters { get; set; }
      //
      //Added_New_10_09_2024
      public DbSet<UploadedDocs> UploadedDocss { get; set; }
      //Added_New_15_10_2024
      public DbSet<UploadedDocsHistory> UploadedDocsHistorys { get; set; }
      // Added By  Amit Start
      public DbSet<projDataUploadAllowed> projDataUploadAllowed { get; set; }

        // Added By  Amit End

        public DbSet<batchSize> batchSize { get; set; }
        //
        public DbSet<ProjectCategoryMaster> ProjectCategoryMaster { get; set; }

       


        public NIELITMISContext()
            : base()
        {
            // Get the ObjectContext related to this DbContext
             var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 600;
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<ColumnTypeCasingConvention>();

            //Mapping of class with table and schema
            modelBuilder.Entity<NielitCentreCourseCategory>().ToTable("NielitCentreCourseCategory");
            modelBuilder.Entity<NielitProjCourses>().ToTable("NielitProjCourses");
            modelBuilder.Entity<CourseFeetype >().ToTable("courseFeeTypeAllowed");
            modelBuilder.Entity<centreTrgCalendar>().ToTable("centreTrgCalendar");
	    modelBuilder.Entity<NonAffiliatedInstituteCoursesDetail>().ToTable("nonAffInsttCourses");
            modelBuilder.Entity<NonAffiliatedInstituteCoursesDetailHistory>().ToTable("nonAffInsttCoursesHistory");
             modelBuilder.Entity<SubjectMaster>().ToTable("SubjectMaster");
      modelBuilder.Entity<SemesterMaster>().ToTable("SemesterMaster");          
      modelBuilder.Entity<SemesterSubjectMaster>().ToTable("SemesterSubjectMaster");
      modelBuilder.Entity<SubjectMaster>().ToTable("SubjectMaster");
      modelBuilder.Entity<SemesterMaster>().ToTable("SemesterMaster");
      modelBuilder.Entity<SemesterSubjectMaster>().ToTable("SemesterSubjectMaster");
      modelBuilder.Entity<SemesterFeeMaster>().ToTable("SemesterFeeMaster");
      modelBuilder.Entity<SemesterFeePaid>().ToTable("SemesterFeePaid");
      modelBuilder.Entity<semesterResultMaster>().ToTable("semesterResultMaster");
      modelBuilder.Entity<StudentSemesterAcademicDetails>().ToTable("StudentSemesterAcademicDetails");

	//Added for virtual Academy
	modelBuilder.Entity<virtualAcademyRegistration>().ToTable("virtualAcademyRegistration");
      modelBuilder.Entity<VirtualAcademyOnlineTransaction>().ToTable("virtualAcademyOnline_Transaction");
      modelBuilder.Entity<VirtualAcademyDemandNote>().ToTable("virtualAcademyDemand_Note");
    //  modelBuilder.Entity<VirtualAcademyCutOffDate>().ToTable("VirtualAcademyCutOffDate");

      modelBuilder.Entity<VirtualAcademyCentreCourse>().ToTable("VirtualAcademyCentreCourse");
      modelBuilder.Entity<virtualAcademyCalendar>().ToTable("virtualAcademyCalendar");
      modelBuilder.Entity<MISCourse_Registraton_Policy>().ToTable("MISCourse_Registraton_Policy");
      modelBuilder.Entity<MISQualification_Eligibility>().ToTable("MISQualification_Eligibility");
      modelBuilder.Entity<VirtualAcademyCutOffDate>().ToTable("VirtualAcademyCutOffDate");
      modelBuilder.Entity<VirtualAcademyCourseServiceID>().ToTable("VirtualAcademyCourseServiceID");
      modelBuilder.Entity<teachingLanguage>().ToTable("teachingLanguage");

      modelBuilder.Entity<DocumentMas>().ToTable("DocumentMas");
      //Added_New_06_09_2024
      modelBuilder.Entity<ProjectDocumentMaster>().ToTable("projDocumentsMas");
      //Added_New_10_09_2024
      modelBuilder.Entity<UploadedDocs>().ToTable("UploadedDocs");
      //Added_New_15_10_2024
      modelBuilder.Entity<UploadedDocsHistory>().ToTable("UploadedDocs_History");
// Added by  Amit Start
  modelBuilder.Entity<projDataUploadAllowed>().ToTable("projDataUploadAllowed");

            // Added by  Amit End
            modelBuilder.Entity<batchSize>().ToTable("batchSize");
            modelBuilder.Entity<ProjectCategoryMaster>().ToTable("ProjectCategoryMaster");

        }

    }

}