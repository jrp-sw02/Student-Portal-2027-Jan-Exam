using System;
using System.Collections.Generic;
using System.Linq;
using EConnect.DAL;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace EConnect.NIELIT
{
    public class CourseManager
    {
        public static Int32 GetCourseRevisionNumberAtRegistrationCommenced(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
        {
            try
            {
                Int32? registrationRevisionNumber = (from c in context.CourseExamApplicationDetails
                                                     join m in context.Modules on c.ModuleID equals m.ID
                                                     where c.CourseID == courseID && c.RegistrationNumber == registrationNumber && c.CandidateID == candidateID
                                                     select m.RevisionNumber).Min();
                if (registrationRevisionNumber.HasValue)
                    return (Int32)registrationRevisionNumber;
                else
                    return 0;
            }
            catch (Exception) { return 0; }
        }
        public static Int32 GetCourseRevisionNumberAtRegistrationCompleted(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
        {
            try
            {
                enmRegistrationStatus registrationStatus;
                Int32 currentevisionNumber = 0;
                RegistrationDetail regDetail = context.RegistrationDetails.Where(r => (r.CourseID == courseID && r.RegistrationNo == registrationNumber && r.CandidateID == candidateID)).FirstOrDefault();
                registrationStatus = regDetail.enmRegistrationStatus;
                if (registrationStatus == enmRegistrationStatus.Cancelled || registrationStatus == enmRegistrationStatus.Completed || registrationStatus == enmRegistrationStatus.Expired || regDetail.ValidUptoDate <= DateTime.Now.Date)
                {
                    try
                    {
                        currentevisionNumber = (from c in context.CourseExamApplicationDetails
                                                join m in context.Modules on c.ModuleID equals m.ID
                                                where c.CourseID == courseID && c.RegistrationNumber == registrationNumber && c.CandidateID == candidateID
                                                select m.RevisionNumber).Max();
                    }
                    catch (Exception) { }
                }
                else
                {
                    currentevisionNumber = CourseManager.GetCurrentCourseRevisionNumber(context, courseID);
                }
                return currentevisionNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Int32 GetCurrentCourseRevisionNumber(EConnectContext context, Int32 courseID)
        {
            try
            {
                Int32 currentRevisionNumber = (from r in context.CourseRevisions
                                               where r.CourseID == courseID &&
                                               r.EffectiveFromDate <= DateTime.Now
                                               select r.RevisionNumber).Distinct().Max();
                return currentRevisionNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IQueryable<Module> GetPassedModulesListOfAnyRevision(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
        {
            try
            {
                IQueryable<Module> passedModulesOfAllRevisions = (from s in context.CourseExamApplicationDetails
                                                                  join g in context.ResultGrades
                                                                  on s.ResultGradeID equals g.ID
                                                                  join m in context.Modules on s.ModuleID equals m.ID
                                                                  where (s.CourseID == courseID &&
                                                                          s.CandidateID == candidateID &&
                                                                          s.RegistrationNumber == registrationNumber &&
                                                                          g.IsPassed == true)
                                                                  orderby m.RevisionNumber, m.Code
                                                                  select m);
                return passedModulesOfAllRevisions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /* public static IQueryable<Module> GetListOfAttempteddModulesOfAnyRevision(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
         {
             try
             {
                 IQueryable<Module> passedModulesOfAllRevisions = (from s in context.CourseExamApplicationDetails
                                                                   join g in context.ResultGrades
                                                                   on s.ResultGradeID equals g.ID
                                                                   join m in context.Modules on s.ModuleID equals m.ID
                                                                   where (s.CourseID == courseID &&
                                                                           s.CandidateID == candidateID &&
                                                                           s.RegistrationNumber == registrationNumber
                                                                           )
                                                                   orderby m.RevisionNumber, m.Code
                                                                   select m).Distinct();
                 return passedModulesOfAllRevisions;
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }*/
        public static IQueryable<Module> GetListOfAttempteddModulesOfAnyRevision(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
        {
            try
            {
                IQueryable<Module> passedModulesOfAllRevisions = (from s in context.CourseExamApplicationDetails
                                                                      //join g in context.ResultGrades
                                                                      //on s.ResultGradeID equals g.ID
                                                                  join m in context.Modules on s.ModuleID equals m.ID
                                                                  where (s.CourseID == courseID &&
                                                                          s.CandidateID == candidateID &&
                                                                          s.RegistrationNumber == registrationNumber
                                                                          )
                                                                  orderby m.RevisionNumber, m.Code
                                                                  select m).Distinct();
                return passedModulesOfAllRevisions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Int32 GetCountOfAttempteddModulesOfAnyRevision(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID, enmModuleType? moduleType)
        {
            try
            {
                Int32 count = 0;
                var cnt = (from s in context.CourseExamApplicationDetails
                           join m in context.Modules on s.ModuleID equals m.ID
                           where (s.CourseID == courseID &&
                                   s.CandidateID == candidateID &&
                                   s.RegistrationNumber == registrationNumber
                                   )
                           orderby m.RevisionNumber, m.Code
                           select m).Distinct();
                if (moduleType.HasValue)
                {
                    Int32 moduleTypeID = Convert.ToInt32(moduleType);
                    Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                    if (moduleType.Value == enmModuleType.Theory)
                        count = cnt.Where(c => (c.ModuleTypeID == moduleTypeID || c.ModuleTypeID == bridge)).Count();
                    else
                        count = cnt.Where(c => c.ModuleTypeID == moduleTypeID).Count();
                }
                else
                {
                    count = cnt.Count();
                }
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IQueryable<Module> GetModulesList(EConnectContext context, Int32 courseID, Int32 revisionNumber)
        {
            try
            {
                IQueryable<Module> appliedModulesOfCurrentRevision = (from m in context.Modules
                                                                      where (m.CourseID == courseID &&
                                                                              m.RevisionNumber == revisionNumber
                                                                              )
                                                                      orderby m.Code
                                                                      select m);
                return appliedModulesOfCurrentRevision;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ICollection<Module> GetRemainingModules(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int32 currentRevisionNumber, Int64 candidateID)
        {
            try
            {
                Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
                Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
                Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
                Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
                Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, courseID, registrationNumber, candidateID);
                ICollection<Module> listOfModulesPassed = EConnect.NIELIT.CourseManager.GetPassedModulesListOfAnyRevision(context, courseID, registrationNumber, candidateID).ToList();

                ICollection<Module> listOfModulesOfCurrentRevision = EConnect.NIELIT.CourseManager.GetModulesList(context, courseID, currentRevisionNumber).ToList();

                List<Module> passedModulesOfCurrentRevision = new List<Module>();

                if (registrationRevisionNumber == currentRevisionNumber)
                {
                    listOfModulesPassed.ToList().ForEach(s => listOfModulesOfCurrentRevision.Remove(s));
                    passedModulesOfCurrentRevision = (List<Module>)listOfModulesPassed;
                }
                else
                {
                    foreach (var mod in listOfModulesPassed.ToList())
                    {
                        //Implement Elctive/Selective Rule
                        Int32 passedRevisionNumber = mod.RevisionNumber;
                        if (passedRevisionNumber == currentRevisionNumber)
                        {
                            passedModulesOfCurrentRevision.Add(mod);
                        }
                        else
                        {
                            Int32 index = 0;
                            Int32 oldModuleID = mod.ID;
                            for (index = passedRevisionNumber; index < currentRevisionNumber; index++)
                            {
                                int newModule = (from s in context.Parities
                                                 where s.CourseID == courseID && s.OldRevisionNumber == index
                                                && s.OldModuleID == oldModuleID
                                                 select s.NewModuleID).FirstOrDefault();
                                if (newModule > 0)
                                {
                                    oldModuleID = (Int32)newModule;
                                }
                                else
                                    break;
                            }

                            Module newMod = context.Modules.Find(oldModuleID);
                            if (newMod != null)
                            {
                                passedModulesOfCurrentRevision.Add(newMod);
                            }
                        }
                    }
					
					
                    if (currentRevisionNumber == 6 && courseID == 1)
                    {
                        if (listOfModulesPassed.Any(p => p.ID == 929) || listOfModulesPassed.Any(p => p.ID == 930) || listOfModulesPassed.Any(p => p.ID == 931) || listOfModulesPassed.Any(p => p.ID == 932))
                        {
                            Module moduleF = context.Modules.Find(715);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }
                    }
                    if (currentRevisionNumber == 6 && courseID == 2)
                    {
                        if (listOfModulesPassed.Any(p => p.ID == 938) || listOfModulesPassed.Any(p => p.ID == 939) || listOfModulesPassed.Any(p => p.ID == 940) || listOfModulesPassed.Any(p => p.ID == 941))
                        {
                            Module moduleF = context.Modules.Find(735);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }
                    }
					
					
                    if (currentRevisionNumber == 6 && courseID == 1)
                    {

                        bool b748 = passedModulesOfCurrentRevision.Any(p => p.ID == 929);
                        if (b748 == true)
                        {
                            Module moduleF = context.Modules.Find(933);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }

                        bool b749 = passedModulesOfCurrentRevision.Any(p => p.ID == 930);
                        if (b749 == true)
                        {
                            Module moduleF = context.Modules.Find(934);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }
                        bool b750 = passedModulesOfCurrentRevision.Any(p => p.ID == 931);
                        if (b750 == true)
                        {
                            Module moduleF = context.Modules.Find(935);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }
                        bool b751 = passedModulesOfCurrentRevision.Any(p => p.ID == 932);
                        if (b751 == true)
                        {
                            Module moduleF = context.Modules.Find(936);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }
                        //passedModulesOfCurrentRevision.ForEach(p => listOfModulesOfCurrentRevision.Remove(p));
                    }
                    else if (currentRevisionNumber == 6 && courseID == 2)
                    {
                        bool b757 = passedModulesOfCurrentRevision.Any(p => p.ID == 938);
                        if (b757 == true)
                        {
                            Module moduleF = context.Modules.Find(956);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }

                        bool b758 = passedModulesOfCurrentRevision.Any(p => p.ID == 939);
                        if (b758 == true)
                        {
                            Module moduleF = context.Modules.Find(957);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }
                        bool b759 = passedModulesOfCurrentRevision.Any(p => p.ID == 940);
                        if (b759 == true)
                        {
                            Module moduleF = context.Modules.Find(958);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }
                        bool b760 = passedModulesOfCurrentRevision.Any(p => p.ID == 941);
                        if (b760 == true)
                        {
                            Module moduleF = context.Modules.Find(959);
                            listOfModulesOfCurrentRevision.Remove(moduleF);
                        }

                    }
                    passedModulesOfCurrentRevision.ForEach(s => listOfModulesOfCurrentRevision.Remove(s));
                }
                Int32 elective = Convert.ToInt32(enmSelectionType.Elective);

                //this code will remove all modules of elective group whome any of the module has been passed by the candidate
                List<Int32> arrElectiveGroup = new List<int>();
                foreach (Module module in passedModulesOfCurrentRevision)
                {
                    if (module != null)
                    {
                        if (module.SelectionTypeID == elective)
                        {
                            if (!arrElectiveGroup.Contains(module.ElectiveGroup.Value))
                                arrElectiveGroup.Add(module.ElectiveGroup.Value);
                        }
                    }
                }
                if (arrElectiveGroup.Count > 0)
                {
                    foreach (Int32? groupID in arrElectiveGroup)
                    {
                        var modulesToBeCleared = (from el in context.Modules
                                                  where el.ElectiveGroup == groupID && el.RevisionNumber == currentRevisionNumber && el.CourseID == courseID
                                                  select el.NumberOfElectiveModulesAllowed).Distinct().Sum();
                        if (modulesToBeCleared.HasValue)
                        {
                            Int32 moduleToBePassed = (Int32)modulesToBeCleared;
                            //Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber && d.CourseID == courseID)).Select(d => d.NumberOfElectiveModulesAllowed).Distinct().Count();                            
                            Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber && d.CourseID == courseID)).Select(d => d.ID).Distinct().Count();
                            if (modulesCleared.HasValue)
                            {
                                if (modulesCleared.Value >= moduleToBePassed)
                                {
                                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ElectiveGroup.HasValue ? d.ElectiveGroup.Value != groupID : 1 == 1)).ToList();
                                }
                            }
                        }
                    }

                }

                //---Start----------Special Case for BE7-R4 & B252-R4 (Sw Testing & Quality Management) in 'B'-Level on dated 14May2020-------------------------
                if (courseID == 3)
                {
                    Int32 theory = Convert.ToInt32(enmModuleType.Theory);
                    Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                    Int32 moduleno_392 = (from d in context.CourseExamApplicationDetails
                                          join m in context.Modules on d.ModuleID equals m.ID
                                          where d.CourseID == courseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                          d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && m.ID == 392
                                          select m).Distinct().Count();
                    Int32 moduleno_412 = (from d in context.CourseExamApplicationDetails
                                          join m in context.Modules on d.ModuleID equals m.ID
                                          where d.CourseID == courseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
                                          d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && m.ID == 412
                                          select m).Distinct().Count();

                    if (moduleno_392 != 0)
                    {
                        listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ID != 412)).ToList();
                    }
                    else if (moduleno_412 != 0)
                    {
                        listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ID != 392)).ToList();
                    }
                }
                //---------------------------------------------------------------END-----------------------------------------------------------

                //
                Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
                Int32 theoryModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Theory, enmSelectionType.Compulsory);
                //theoryModulesTobePassed += GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Bridge, null);
                Int32 theoryModulesPassed = passedModulesOfCurrentRevision.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == compulsory)).Count();
                if (theoryModulesPassed >= theoryModulesTobePassed)
                    if (registrationRevisionNumber == currentRevisionNumber)
                        listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => ((d.ModuleTypeID == moduleTypeTheory ? d.SelectionTypeID != compulsory : 1 == 1))).ToList();

                if (listOfModulesOfCurrentRevision.Contains(context.Modules.Find(946)) && !listOfModulesOfCurrentRevision.Contains(context.Modules.Find(951)))
                {

                    Module moduleXi = context.Modules.Find(951);
                    listOfModulesOfCurrentRevision.Add(moduleXi);

                    Module moduleXii = context.Modules.Find(952);
                    listOfModulesOfCurrentRevision.Add(moduleXii);

                    Module moduleXiii = context.Modules.Find(953);
                    listOfModulesOfCurrentRevision.Add(moduleXiii);

                    Module moduleXiv = context.Modules.Find(954);
                    listOfModulesOfCurrentRevision.Add(moduleXiv);

                    Module moduleXv = context.Modules.Find(955);
                    listOfModulesOfCurrentRevision.Add(moduleXv);

                }
                else if (!listOfModulesOfCurrentRevision.Contains(context.Modules.Find(946)) && listOfModulesOfCurrentRevision.Contains(context.Modules.Find(951)))
                {
                    Module moduleXi = context.Modules.Find(946);
                    listOfModulesOfCurrentRevision.Add(moduleXi);

                    Module moduleXii = context.Modules.Find(947);
                    listOfModulesOfCurrentRevision.Add(moduleXii);

                    Module moduleXiii = context.Modules.Find(948);
                    listOfModulesOfCurrentRevision.Add(moduleXiii);

                    Module moduleXiv = context.Modules.Find(949);
                    listOfModulesOfCurrentRevision.Add(moduleXiv);

                    Module moduleXv = context.Modules.Find(950);
                    listOfModulesOfCurrentRevision.Add(moduleXv);
                }

                Int32 bridgeModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Bridge, null);
                Int32 bridgeModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeBridge).Count();
                if (bridgeModulesPassed >= bridgeModulesTobePassed)
                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeBridge).ToList();

                Int32 practicalModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Practical, null);
                Int32 practicalModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypePractical).Count();
                if (practicalModulesPassed >= practicalModulesTobePassed)
                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypePractical).ToList();

                Int32 projectModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Project, null);
                Int32 projectModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeProject).Count();
                if (projectModulesPassed >= projectModulesTobePassed)
                    listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeProject).ToList();

                return listOfModulesOfCurrentRevision.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public static ICollection<Module> GetRemainingModules(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int32 currentRevisionNumber, Int64 candidateID)
        //{
        //    try
        //    {
        //        Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
        //        Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
        //        Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
        //        Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
        //        Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, courseID, registrationNumber, candidateID);
        //        ICollection<Module> listOfModulesPassed = EConnect.NIELIT.CourseManager.GetPassedModulesListOfAnyRevision(context, courseID, registrationNumber, candidateID).ToList();

        //        ICollection<Module> listOfModulesOfCurrentRevision = EConnect.NIELIT.CourseManager.GetModulesList(context, courseID, currentRevisionNumber).ToList();

        //        List<Module> passedModulesOfCurrentRevision = new List<Module>();

        //        if (registrationRevisionNumber == currentRevisionNumber)
        //        {
        //            listOfModulesPassed.ToList().ForEach(s => listOfModulesOfCurrentRevision.Remove(s));
        //            passedModulesOfCurrentRevision = (List<Module>)listOfModulesPassed;
        //        }
        //        else
        //        {
        //            foreach (var mod in listOfModulesPassed.ToList())
        //            {
        //                //Implement Elctive/Selective Rule
        //                Int32 passedRevisionNumber = mod.RevisionNumber;

        //                if (passedRevisionNumber == currentRevisionNumber)
        //                {
        //                    passedModulesOfCurrentRevision.Add(mod);
        //                }
        //                else
        //                {
        //                    Int32 index = 0;
        //                    Int32 oldModuleID = mod.ID;
        //                    for (index = passedRevisionNumber; index < currentRevisionNumber; index++)
        //                    {

        //                        int newModule = (from s in context.Parities
        //                                         where s.CourseID == courseID && s.OldRevisionNumber == index
        //                                        && s.OldModuleID == oldModuleID
        //                                         select s.NewModuleID).FirstOrDefault();
        //                        if (newModule > 0)
        //                        {
        //                            oldModuleID = (Int32)newModule;
        //                        }
        //                        else
        //                            break;
        //                    }

        //                    Module newMod = context.Modules.Find(oldModuleID);
        //                    if (newMod != null)
        //                    {
        //                        passedModulesOfCurrentRevision.Add(newMod);
        //                    }

        //                }
        //            }
        //            passedModulesOfCurrentRevision.ForEach(s => listOfModulesOfCurrentRevision.Remove(s));
        //        }
        //        Int32 elective = Convert.ToInt32(enmSelectionType.Elective);

        //        //this code will remove all modules of elective group whome any of the module has been passed by the candidate
        //        List<Int32> arrElectiveGroup = new List<int>();
        //        foreach (Module module in passedModulesOfCurrentRevision)
        //        {
        //            if (module != null)
        //            {
        //                if (module.SelectionTypeID == elective)
        //                {
        //                    if (!arrElectiveGroup.Contains(module.ElectiveGroup.Value))
        //                        arrElectiveGroup.Add(module.ElectiveGroup.Value);
        //                }
        //            }
        //        }
        //        if (arrElectiveGroup.Count > 0)
        //        {
        //            foreach (Int32? groupID in arrElectiveGroup)
        //            {
        //                var modulesToBeCleared = (from el in context.Modules
        //                                          where el.ElectiveGroup == groupID && el.RevisionNumber == currentRevisionNumber && el.CourseID == courseID
        //                                          select el.NumberOfElectiveModulesAllowed).Distinct().Sum();
        //                if (modulesToBeCleared.HasValue)
        //                {
        //                    Int32 moduleToBePassed = (Int32)modulesToBeCleared;
        //                    //Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber && d.CourseID == courseID)).Select(d => d.NumberOfElectiveModulesAllowed).Distinct().Count();                            
        //                    Int32? modulesCleared = passedModulesOfCurrentRevision.Where(d => (d.ElectiveGroup == groupID && d.RevisionNumber == currentRevisionNumber && d.CourseID == courseID)).Select(d => d.ID).Distinct().Count();
        //                    if (modulesCleared.HasValue)
        //                    {
        //                        if (modulesCleared.Value >= moduleToBePassed)
        //                        {
        //                            listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ElectiveGroup.HasValue ? d.ElectiveGroup.Value != groupID : 1 == 1)).ToList();
        //                        }
        //                    }
        //                }
        //            }

        //        }

        //        //---Start----------Special Case for BE7-R4 & B252-R4 (Sw Testing & Quality Management) in 'B'-Level on dated 14May2020-------------------------
        //        if (courseID == 3)
        //        {
        //            Int32 theory = Convert.ToInt32(enmModuleType.Theory);
        //            Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
        //            Int32 moduleno_392 = (from d in context.CourseExamApplicationDetails
        //                                  join m in context.Modules on d.ModuleID equals m.ID
        //                                  where d.CourseID == courseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
        //                                  d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && m.ID == 392
        //                                  select m).Distinct().Count();
        //            Int32 moduleno_412 = (from d in context.CourseExamApplicationDetails
        //                                  join m in context.Modules on d.ModuleID equals m.ID
        //                                  where d.CourseID == courseID && d.RegistrationNumber == registrationNumber && d.CandidateID == candidateID &&
        //                                  d.Grade.IsPassed == true && (m.ModuleTypeID == theory || m.ModuleTypeID == bridge) && m.ID == 412
        //                                  select m).Distinct().Count();

        //            if (moduleno_392 != 0)
        //            {
        //                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ID != 412)).ToList();
        //            }
        //            else if (moduleno_412 != 0)
        //            {
        //                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => (d.ID != 392)).ToList();
        //            }
        //        }
        //        //---------------------------------------------------------------END-----------------------------------------------------------

        //        //
        //        Int32 compulsory = Convert.ToInt32(enmSelectionType.Compulsory);
        //        Int32 theoryModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Theory, enmSelectionType.Compulsory);
        //        //theoryModulesTobePassed += GetTotalModules(currentCourseID, currentevisionNumber, enmModuleType.Bridge, null);
        //        Int32 theoryModulesPassed = passedModulesOfCurrentRevision.Where(d => (d.ModuleTypeID == moduleTypeTheory && d.SelectionTypeID == compulsory)).Count();
        //        if (theoryModulesPassed >= theoryModulesTobePassed)
        //            if (registrationRevisionNumber == currentRevisionNumber)
        //                listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => ((d.ModuleTypeID == moduleTypeTheory ? d.SelectionTypeID != compulsory : 1 == 1))).ToList();

        //        Int32 bridgeModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Bridge, null);
        //        Int32 bridgeModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeBridge).Count();
        //        if (bridgeModulesPassed >= bridgeModulesTobePassed)
        //            listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeBridge).ToList();

        //        Int32 practicalModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Practical, null);
        //        Int32 practicalModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypePractical).Count();
        //        if (practicalModulesPassed >= practicalModulesTobePassed)
        //            listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypePractical).ToList();

        //        Int32 projectModulesTobePassed = GetTotalModules(courseID, currentRevisionNumber, enmModuleType.Project, null);
        //        Int32 projectModulesPassed = passedModulesOfCurrentRevision.Where(d => d.ModuleTypeID == moduleTypeProject).Count();
        //        if (projectModulesPassed >= projectModulesTobePassed)
        //            listOfModulesOfCurrentRevision = listOfModulesOfCurrentRevision.Where(d => d.ModuleTypeID != moduleTypeProject).ToList();

        //        return listOfModulesOfCurrentRevision.Distinct().ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public static IQueryable<Exam> GetListOfTheoryPassed(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
        {
            try
            {
                IQueryable<Exam> exams = (from s in context.CourseExamApplicationDetails
                                          join e in context.Exams on s.ExamID equals e.ID
                                          join m in context.Modules on s.ModuleID equals m.ID
                                          join r in context.ResultGrades on s.ResultGradeID equals r.ID
                                          where s.CourseID == courseID && s.RegistrationNumber == registrationNumber && s.CandidateID == candidateID
                                          && r.IsPassed == true
                                          && r.CourseCategoryID == 1 && m.ModuleTypeID == 1 && s.CourseID == m.CourseID
                                          //&& s.ExamID.HasValue == true
                                          select s.Exam).Distinct();
                return exams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ICollection<Module> GetPassedModulesOfCurrentRevision(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int32 currentRevisionNumber, Int64 candidateID)
        {
            try
            {
                Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
                Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
                Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
                Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
                Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, courseID, registrationNumber, candidateID);
                ICollection<Module> listOfModulesPassed = EConnect.NIELIT.CourseManager.GetPassedModulesListOfAnyRevision(context, courseID, registrationNumber, candidateID).ToList();

                ICollection<Module> listOfModulesOfCurrentRevision = EConnect.NIELIT.CourseManager.GetModulesList(context, courseID, currentRevisionNumber).ToList();

                List<Module> passedModulesOfCurrentRevision = new List<Module>();

                if (registrationRevisionNumber == currentRevisionNumber)
                {
                    passedModulesOfCurrentRevision = (List<Module>)listOfModulesPassed;
                }
                else
                {
                    foreach (var mod in listOfModulesPassed.ToList())
                    {
                        //Implement Elctive/Selective Rule
                        Int32 passedRevisionNumber = mod.RevisionNumber;

                        if (passedRevisionNumber == currentRevisionNumber)
                        {
                            passedModulesOfCurrentRevision.Add(mod);
                        }
                        else
                        {
                            Int32 index = 0;
                            Int32 oldModuleID = mod.ID;
                            for (index = passedRevisionNumber; index < currentRevisionNumber; index++)
                            {

                                int newModule = (from s in context.Parities
                                                 where s.CourseID == courseID && s.OldRevisionNumber == index
                                                && s.OldModuleID == oldModuleID
                                                 select s.NewModuleID).FirstOrDefault();

                                if (newModule > 0)
                                    oldModuleID = (Int32)newModule;
                                else
                                    break;
                            }
                            Module newMod = context.Modules.Find(oldModuleID);
                            if (newMod != null)
                            {
                                passedModulesOfCurrentRevision.Add(newMod);
                            }

                        }
                    }
                }
                return passedModulesOfCurrentRevision.Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IQueryable<Exam> GetListOfAttemptedExams(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int64 candidateID)
        {
            try
            {
                IQueryable<Exam> exams = (from s in context.CourseExamApplicationDetails
                                          join e in context.Exams on s.ExamID equals e.ID
                                          where s.CourseID == courseID && s.RegistrationNumber == registrationNumber && s.CandidateID == candidateID
                                          //&& s.ExamID.HasValue == true
                                          select s.Exam).Distinct();
                return exams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Int32 GetTotalModules(Int32 courseID, Int32 revisionNumber, enmModuleType? moduleType, enmSelectionType? selectionType)
        {
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 compulsoryType = Convert.ToInt32(enmSelectionType.Compulsory);
                    Int32? selectionTypeID = null;
                    if (selectionType.HasValue)
                        selectionTypeID = Convert.ToInt32(selectionType);
                    Int32 count = 0;
                    if (moduleType.HasValue)
                    {

                        Int32 moduleTypeID = Convert.ToInt32(moduleType);
                        if (moduleType.Value == enmModuleType.Theory)
                        {
                            Int32 bridgeTypeID = Convert.ToInt32(enmModuleType.Bridge);
                            if (selectionTypeID.HasValue)
                            {
                                if (selectionType.Value == enmSelectionType.Compulsory)
                                {
                                    count = (from m in context.Modules
                                             where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID == selectionTypeID.Value && m.ModuleTypeID == moduleTypeID
                                             select m).Count();
                                }
                                else
                                {
                                    var electiveGroup = (from m in context.Modules
                                                         where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType && m.ModuleTypeID == moduleTypeID
                                                         select m.ElectiveGroup).Distinct();
                                    if (electiveGroup != null)
                                    {
                                        ////-------Start----------Added on 15052020 due to count mismatch of elective group-id during the switching of revision choice and total module count
                                        foreach (var groupid in electiveGroup.ToList())
                                        {
                                            var cnt = (from m in context.Modules
                                                       where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ElectiveGroup == groupid
                                                       select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                            if (cnt.HasValue)
                                            {
                                                count += (Int32)cnt.Value;
                                            }
                                        }
                                        ////------------------------------------------------------------End    

                                        ////-------Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                        ////var cnt = (from m in context.Modules
                                        ////           where electiveGroup.Contains(m.ElectiveGroup) && m.CourseID == courseID && m.RevisionNumber == revisionNumber
                                        ////           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                        ////if (cnt.HasValue)
                                        ////    //Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                        ////    //count = (Int32)cnt.Value;
                                        ////    //----------------------------------------------------------------End

                                        ////    //Start-------Added on 05052020 for rectify count mismatch of elective group-id during the switching of revision choice-----------
                                        ////    for (Int32 totalelectivegroup = 0; totalelectivegroup < electiveGroup.Count(); totalelectivegroup++)
                                        ////    {
                                        ////        count += (Int32)cnt.Value;
                                        ////    }
                                        ////    //------------------------------------------------------------End    
                                    }
                                }

                            }
                            else
                            {
                                count = (from m in context.Modules
                                         where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID == compulsoryType && m.ModuleTypeID == moduleTypeID
                                         select m).Count();
                                var electiveGroup = (from m in context.Modules
                                                     where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType && m.ModuleTypeID == moduleTypeID
                                                     select m.ElectiveGroup).Distinct();
                                if (electiveGroup != null)
                                {

                                    ////-------Start----------Added on 15052020 due to count mismatch of elective group-id during the switching of revision choice and total module count
                                    foreach (var groupid in electiveGroup.ToList())
                                    {
                                        var cnt = (from m in context.Modules
                                                   where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ElectiveGroup == groupid
                                                   select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                        if (cnt.HasValue)
                                        {
                                            count += (Int32)cnt.Value;
                                        }
                                    }
                                    ////------------------------------------------------------------End    

                                    ////-------Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                    //var cnt = (from m in context.Modules
                                    //           where electiveGroup.Contains(m.ElectiveGroup) && m.CourseID == courseID && m.RevisionNumber == revisionNumber
                                    //           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                    //if (cnt.HasValue)
                                    // //Start----------comment on 24042020 due to count mismatch of elective group-id during the switching of revision choice
                                    ////count += (Int32)cnt.Value;                                    
                                    ////----------------------------------------------------------------End

                                    ////Start-------Added on 24042020 for rectify count mismatch of elective group-id during the switching of revision choice-----------
                                    //for (Int32 totalelectivegroup = 0; totalelectivegroup < electiveGroup.Count(); totalelectivegroup++)
                                    //{
                                    //    count += (Int32)cnt.Value;
                                    //}
                                    ////------------------------------------------------------------End    
                                }
                                //count += (from m in context.Modules
                                //          where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType && (m.ModuleTypeID == moduleTypeID || m.ModuleTypeID == bridgeTypeID)
                                //          select m.ElectiveGroup).Distinct().Count();
                            }
                        }
                        else
                        {
                            count = (from m in context.Modules
                                     where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ModuleTypeID == moduleTypeID
                                     select m).Count();
                        }
                    }
                    else
                    {
                        if (selectionTypeID.HasValue)
                        {
                            if (selectionType.Value == enmSelectionType.Compulsory)
                            {
                                count = (from m in context.Modules
                                         where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID == selectionTypeID.Value
                                         select m).Count();
                            }
                            else
                            {
                                var electiveGroup = (from m in context.Modules
                                                     where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType
                                                     select m.ElectiveGroup).Distinct();
                                if (electiveGroup != null)
                                {

                                    ////-------Start----------Added on 15052020 due to count mismatch of elective group-id during the switching of revision choice and total module count
                                    foreach (var groupid in electiveGroup.ToList())
                                    {
                                        var cnt = (from m in context.Modules
                                                   where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ElectiveGroup == groupid
                                                   select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                        if (cnt.HasValue)
                                        {
                                            count += (Int32)cnt.Value;
                                        }
                                    }
                                    ////------------------------------------------------------------End   

                                    ////-------Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                    //var cnt = (from m in context.Modules
                                    //           where electiveGroup.Contains(m.ElectiveGroup) && m.CourseID == courseID && m.RevisionNumber == revisionNumber
                                    //           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                    //if (cnt.HasValue)
                                    //    //count = (Int32)cnt.Value;

                                    //    //Start-------Added on 05052020 for rectify count mismatch of elective group-id during the switching of revision choice-----------
                                    //    for (Int32 totalelectivegroup = 0; totalelectivegroup < electiveGroup.Count(); totalelectivegroup++)
                                    //    {
                                    //        count += (Int32)cnt.Value;
                                    //    }
                                    //    //------------------------------------------------------------End    
                                }
                            }

                        }
                        else
                        {
                            count = (from m in context.Modules
                                     where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID == compulsoryType
                                     select m).Count();
                            var electiveGroup = (from m in context.Modules
                                                 where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType
                                                 select m.ElectiveGroup).Distinct();
                            if (electiveGroup != null)
                            {
                                ////-------Start----------Added on 15052020 due to count mismatch of elective group-id during the switching of revision choice and total module count                                
                                foreach (var groupid in electiveGroup.ToList())
                                {
                                    var cnt = (from m in context.Modules
                                               where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.ElectiveGroup == groupid
                                               select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                    if (cnt.HasValue)
                                    {
                                        count += (Int32)cnt.Value;
                                    }
                                }
                                ////------------------------------------------------------------End    

                                ////-------Start----------comment on 05052020 due to count mismatch of elective group-id during the switching of revision choice
                                //var cnt = (from m in context.Modules
                                //           where electiveGroup.Contains(m.ElectiveGroup) && m.CourseID == courseID && m.RevisionNumber == revisionNumber
                                //           select m.NumberOfElectiveModulesAllowed).Distinct().Sum();
                                //if (cnt.HasValue)
                                //    //count += (Int32)cnt.Value;

                                //    //Start-------Added on 05052020 for rectify count mismatch of elective group-id during the switching of revision choice-----------
                                //    for (Int32 totalelectivegroup = 0; totalelectivegroup < electiveGroup.Count(); totalelectivegroup++)
                                //    {
                                //        count += (Int32)cnt.Value;
                                //    }
                                ////------------------------------------------------------------End    
                            }
                            //count += (from m in context.Modules
                            //          where m.CourseID == courseID && m.RevisionNumber == revisionNumber && m.SelectionTypeID != compulsoryType
                            //          select m.ElectiveGroup).Distinct().Count();
                        }
                    }
                    return count;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ICollection<Module> GetModulesForImprovement(EConnectContext context, Int32 courseID, Int64 registrationNumber, Int32 currentRevisionNumber, Int64 candidateID)
        {
            try
            {
                Int32[] moduleTypes = { Convert.ToInt32(enmModuleType.Theory), Convert.ToInt32(enmModuleType.Bridge) };
                ICollection<Module> listOfModulesPassed = EConnect.NIELIT.CourseManager.GetPassedModulesListOfAnyRevision(context, courseID, registrationNumber, candidateID).ToList();
                listOfModulesPassed = listOfModulesPassed.Where(d => moduleTypes.Contains(d.ModuleTypeID)).ToList();
                List<Module> passedModulesOfCurrentRevision = new List<Module>();
                foreach (var module in listOfModulesPassed.ToList())
                {
                    //Implement Elctive/Selective Rule
                    Int32 passedRevisionNumber = module.RevisionNumber;

                    if (passedRevisionNumber == currentRevisionNumber)
                    {
                        passedModulesOfCurrentRevision.Add(module);
                    }
                    else
                    {
                        Int32 index = 0;
                        Int32 oldModuleID = module.ID;
                        for (index = passedRevisionNumber; index < currentRevisionNumber; index++)
                        {

                            int newModule = (from s in context.Parities
                                             where s.CourseID == courseID && s.OldRevisionNumber == index
                                            && s.OldModuleID == oldModuleID
                                             select s.NewModuleID).FirstOrDefault();
                            if (newModule > 0)
                                oldModuleID = (Int32)newModule;
                            else
                                break;
                        }
                        if (index == currentRevisionNumber)
                        {
                            Module newMod = context.Modules.Find(oldModuleID);
                            if (newMod != null)
                                passedModulesOfCurrentRevision.Add(newMod);
                        }
                    }
                }
                return passedModulesOfCurrentRevision;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Exam GetNextExam(EConnectContext context, Int32 courseID, Int32 applicantTypeID)
        {
            try
            {
                Int32 commencementDate = Convert.ToInt32(enmActivity.DateFfCommencementOfOnlineFillInExaminationApplicationForm);
                var examsAll = (from f in context.Exams
                                join c in context.CutOffDates on f.ID equals c.ExamID
                                where f.CourseID == courseID && c.ActivityID == commencementDate && c.EfferctiveDate <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) &&
                                f.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now) && f.DateOfPublishingOfRollNumber == null &&
                                f.ExamStartDate > System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                orderby c.EfferctiveDate descending
                                select f);
                if (examsAll.Count() > 0)
                {
                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                    var exmasWithNormalLastDate = (from t in context.CutOffDates
                                                   where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == applicantTypeID &&
                                                   t.ActivityID == NormalFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                   orderby t.Exam.ExamStartDate
                                                   select t.Exam).Distinct();
                    if (exmasWithNormalLastDate.Count() == 0)
                    {
                        var exmasWithLateFeeLastDate = (from t in context.CutOffDates
                                                        where examsAll.Select(d => d.ID).Contains(t.ExamID) && t.ApplicantTypeID == applicantTypeID &&
                                                        t.ActivityID == LateFeeActivityId && t.EfferctiveDate >= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                                                        orderby t.Exam.ExamStartDate
                                                        select t.Exam).Distinct();
                        return exmasWithLateFeeLastDate.FirstOrDefault();
                    }
                    else
                    {
                        return examsAll.FirstOrDefault();
                    }
                }
                else
                    return examsAll.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Boolean IsLateFeeApplicable(EConnectContext context, Int32 examID, Int32 applicantTypeID)
        {
            try
            {
                Int32 submisssionDateActivityID = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);
                DateTime lastDate = (from c in context.CutOffDates
                                     where c.ActivityID == submisssionDateActivityID && c.ExamID == examID && c.ApplicantTypeID == applicantTypeID
                                     select c.EfferctiveDate).FirstOrDefault();
                if (DateTime.Now.Date > lastDate.Date)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Exam GeLastExam(EConnectContext context, Int32 courseID, Int32 applicantTypeID)
        {
            try
            {
                Exam nextEexam = CourseManager.GetNextExam(context, courseID, applicantTypeID);
                Exam lastExam = (from e in context.Exams
                                 where (e.ExamYear < nextEexam.ExamYear || e.ExamMonth < nextEexam.ExamMonth) && e.CourseID == courseID
                                 select e).OrderByDescending(a => a.ExamYear).ThenByDescending(b => b.ExamMonth).FirstOrDefault();
                return lastExam;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static CourseRegistrationPolicy GetCurrentRegistrationPolicy(EConnectContext context, Int32 courseID)
        {
            try
            {
                CourseRegistrationPolicy policy = (from p in context.CourseRegistrationPolicies
                                                   where p.EffectiveFromDate <= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                                                   && p.CourseID == courseID
                                                   orderby p.EffectiveFromDate descending
                                                   select p).FirstOrDefault();
                return policy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ICollection<Module> GetListtOfAttempteddModulesOfCurrentRevision(EConnectContext context, Int32 currentExamID, Int32 courseID, Int64 registrationNumber, Int32 currentRevisionNumber, Int64 candidateID, enmModuleType? moduleType)
        {
            try
            {
                Int32 moduleTypeTheory = Convert.ToInt32(enmModuleType.Theory);
                Int32 moduleTypeBridge = Convert.ToInt32(enmModuleType.Bridge);
                Int32 moduleTypePractical = Convert.ToInt32(enmModuleType.Practical);
                Int32 moduleTypeProject = Convert.ToInt32(enmModuleType.Project);
                Int32 registrationRevisionNumber = EConnect.NIELIT.CourseManager.GetCourseRevisionNumberAtRegistrationCommenced(context, courseID, registrationNumber, candidateID);
                IEnumerable<Module> attemptedModules = (from s in context.CourseExamApplicationDetails
                                                        join m in context.Modules on s.ModuleID equals m.ID
                                                        where (s.CourseID == courseID &&
                                                                s.CandidateID == candidateID &&
                                                                s.RegistrationNumber == registrationNumber && s.ExamID != currentExamID
                                                                )
                                                        orderby m.RevisionNumber, m.Code
                                                        select m).Distinct();
                if (moduleType.HasValue)
                {
                    Int32 moduleTypeID = Convert.ToInt32(moduleType);
                    Int32 bridge = Convert.ToInt32(enmModuleType.Bridge);
                    if (moduleType.Value == enmModuleType.Theory)
                        attemptedModules = attemptedModules.Where(c => (c.ModuleTypeID == moduleTypeID || c.ModuleTypeID == bridge));
                    else
                        attemptedModules = attemptedModules.Where(c => (c.ModuleTypeID == moduleTypeID));
                }
                List<Module> attemptedModulesOfCurrentRevision = new List<Module>();
                if (registrationRevisionNumber == currentRevisionNumber)
                {
                    return attemptedModules.ToList();
                }
                else
                {
                    foreach (var mod in attemptedModules.ToList())
                    {
                        //Implement Elctive/Selective Rule
                        Int32 passedRevisionNumber = mod.RevisionNumber;

                        if (passedRevisionNumber == currentRevisionNumber)
                        {
                            attemptedModulesOfCurrentRevision.Add(mod);
                        }
                        else
                        {
                            Int32 index = 0;
                            Int32 oldModuleID = mod.ID;
                            for (index = passedRevisionNumber; index < currentRevisionNumber; index++)
                            {

                                var newModule = (from s in context.Parities
                                                 where s.CourseID == courseID && s.OldRevisionNumber == index
                                                && s.OldModuleID == oldModuleID
                                                 select s.NewModuleID).FirstOrDefault();

                                if (newModule > 0)
                                    oldModuleID = (Int32)newModule;
                                else
                                    break;
                            }
                            Module newMod = context.Modules.Find(oldModuleID);
                            if (newMod != null)
                            { attemptedModulesOfCurrentRevision.Add(newMod); }

                        }
                    }
                    return attemptedModulesOfCurrentRevision;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Int32 GetAppliableFee(EConnectContext context, Int32 courseID, enmFeeType feeType)
        {
            try
            {
                Int32 feeTypeID = Convert.ToInt32(feeType);
                int fee = (from r in context.FeeDetails
                           where r.CourseID == courseID &&
                           r.FeeTypeID == feeTypeID && 
                           r.EffectiveFromDate <= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                           //Added on 12 Nov 2025 
                           && r.projectid== null
                           orderby r.EffectiveFromDate descending
                           select r.FeeAmount).FirstOrDefault();
                if (fee > 0)
                    return fee;
                else
                    return 0;
            }
            catch (Exception) { return 0; }
        }
        public static Int32 GetCSCProcessingCharge(EConnectContext context, Int32 applicationTypeID, Int32 activityID)
        {
            try
            {
                Int32? feeamount = (from d in context.CSCCharges
                                    where d.ApplicationTypeID == applicationTypeID &&
                                    d.ActivityID == activityID && d.EffectiveDate <= System.Data.Entity.DbFunctions.CreateDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                                    orderby d.EffectiveDate descending
                                    select d.Amount).FirstOrDefault();
                if (feeamount.HasValue)
                    return feeamount.Value;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void GetCertificateExamFee(Int32 courseID, Int32 examID, Int32 applicantTypeID, out Int32 feeAmount, out Int32 lateFeeAmount)
        {
            try
            {
                feeAmount = 0;
                lateFeeAmount = 0;
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 feeTypeID = Convert.ToInt32(enmFeeType.RegistrationCumExaminationFee);
                    feeAmount = (from f in context.FeeDetails
                                 where f.CourseID == courseID && f.FeeTypeID == feeTypeID &&
                                    f.EffectiveFromDate == (from c in context.FeeDetails
                                                            where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                            select c.EffectiveFromDate).Max()
                                 select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;
                    int LateFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationFormWithLateFee);
                    int NormalFeeActivityId = Convert.ToInt32(enmActivity.LastDateOfOnlineSubmissionOfExaminationApplicationForm);

                    int LateFeeTypeId = Convert.ToInt32(enmFeeType.LateFeeExam);

                    var Fee = (from f in context.CutOffDates
                               where f.CourseID == courseID &&
                               f.ExamID == examID &&
                               f.ApplicantTypeID == applicantTypeID
                               select new { EffectiveDate = f.EfferctiveDate, f.ActivityID }).ToList();
                    var NormalFee = Fee.Where(l => l.ActivityID == NormalFeeActivityId);
                    var lateFee = Fee.Where(l => l.ActivityID == LateFeeActivityId);

                    if (lateFee.Count() > 0 && lateFee != null)
                    {
                        if (NormalFee.FirstOrDefault().EffectiveDate <= DateTime.Now)
                        {
                            Int32? LatefeeAmount = (from f in context.FeeDetails
                                                    where f.CourseID == courseID && f.FeeTypeID == LateFeeTypeId &&
                                                    f.EffectiveFromDate == (from c in context.FeeDetails
                                                                            where c.CourseID == f.CourseID && c.FeeTypeID == f.FeeTypeID
                                                                            select c.EffectiveFromDate).Max()
                                                    select new { FeeAmount = f.FeeAmount }).FirstOrDefault().FeeAmount;
                            if (LatefeeAmount.HasValue)
                            {
                                if (LatefeeAmount.Value > 0)
                                {
                                    lateFeeAmount = LatefeeAmount.Value;
                                }
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static enmCurrentRegistrationStatus GetCurrentRegistrationStatus(Int32 CurrentCourseID, Int64 candidateID) //checking Registration status
        {
            try
            {
                using (EConnectContext context = new EConnectContext())
                {

                    var currentRegistration = (from c in context.RegistrationDetails
                                               where c.CandidateID == candidateID
                                               orderby c.CommencementFromDate descending
                                               select c).FirstOrDefault();
                    DateTime dt = currentRegistration.ValidUptoDate.Date;
                    //var reRegistrationPolicy = context.CourseRegistrationPolicies.Where(a => a.CourseID == currentRegistration.CourseID && a.EffectiveFromDate <= DateTime.Now).OrderByDescending(c => c.EffectiveFromDate).FirstOrDefault();
                    var reRegistrationPolicy = context.CourseRegistrationPolicies.Where(a => a.CourseID == currentRegistration.CourseID && a.EffectiveFromDate <= DateTime.Now).OrderByDescending(c => c.EffectiveFromDate).FirstOrDefault();
                    enmCurrentRegistrationStatus reasons = enmCurrentRegistrationStatus.None;
                    if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Completed)
                    {
                        //Current Level Registration status is completed
                        //Valdate whether eligible for auto upgradation or not
                        reasons = enmCurrentRegistrationStatus.CompletedAsAllModulesOfCurrentLevelPassedCompletelyWithinRegistrationPeriod;
                    }
                    else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Cancelled)
                    {
                        reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;

                    }
                    else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.ProjectPending)
                    {
                        //If valid upto date is less  than current date: Registration expired
                        //  if (DateTime.Now > currentRegistration.ValidUptoDate)
                        if (DateTime.Today > dt)
                        {
                            if (reRegistrationPolicy.ReRegistrationChance == true)
                            {
                                //if (DateTime.Now > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value))
                                if (DateTime.Today > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value).Date)
                                {
                                    //Registration validity period is over and no chances for reregistration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;

                                }
                                else
                                {
                                    //Registration validity period is over but eligible for re-registration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;

                                }
                            }
                            else
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;

                            }
                        }
                        else
                        {
                            reasons = enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed;

                        }

                    }
                    else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Registered)
                    {
                        //If valid upto date is less  than current date: Registration expired
                        //if (DateTime.Now > currentRegistration.ValidUptoDate)
                        if (DateTime.Today > dt)
                        {
                            if (reRegistrationPolicy.ReRegistrationChance == true)
                            {
                                //if (DateTime.Now > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value).AddDays(1))
                                if (DateTime.Today > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value).Date)
                                {
                                    //Registration validity period is over and no chances for reregistration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                }
                                else
                                {
                                    //Registration validity period is over but eligible for re-registration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                                }
                            }
                            else
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                            }
                        }
                        else
                        {
                            //Registration validity period is over but eligible for re-registration
                            reasons = enmCurrentRegistrationStatus.RegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndValidityPeriodNotLapsed;
                        }
                    }
                    else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.Expired)
                    {
                        //If valid upto date is less  than current date: Registration expired
                        //if (DateTime.Now > currentRegistration.ValidUptoDate)
                        if (DateTime.Today > dt)
                        {
                            if (reRegistrationPolicy.ReRegistrationChance == true)
                            {
                                //if (DateTime.Now > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value))
                                if (DateTime.Today > currentRegistration.ValidUptoDate.AddMonths(reRegistrationPolicy.ReRegistrationGapInMonths.Value).Date)
                                {
                                    //Registration validity period is over and no chances for reregistration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                                }
                                else
                                {
                                    //Registration validity period is over but eligible for re-registration
                                    reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                                }
                            }
                            else
                            {
                                //Registration validity period is over and no chances for reregistration
                                reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                            }
                        }
                        else
                        {
                            //Registration validity period is over but eligible for re-registration
                            reasons = enmCurrentRegistrationStatus.ExpiredAndUnderReRegistrationGracePeriodAsValidityOfCurrentLevelRegistrationHasExpired;
                        }

                    }
                    else if (currentRegistration.enmRegistrationStatus == enmRegistrationStatus.ReRegistered)
                    {
                        //If valid upto date is less  than current date: Registration expired
                        //if (DateTime.Now > currentRegistration.ValidUptoDate)
                        if (DateTime.Today > dt)
                        {
                            //ReRegistration validity period is over
                            reasons = enmCurrentRegistrationStatus.ExpiredAsAllModulesOfCurrentLevelNotPassedCompletedWithinReRegistrationValidityPeriod;
                        }
                        else
                        {
                            //Under re-registration validity period
                            reasons = enmCurrentRegistrationStatus.ReRegisteredAsAllModulesOfCurrentLevelNotPassedCompletelyTillDateAndReRegistrationValidityPeriodNotLapsed;
                        }
                    }
                    else
                    { }
                    return reasons;


                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static Boolean IsCourseReviesd(Int32 CurrentCourseID)//whether revision of syllabus is applicable to show -----28022020
        {
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var revisiondetails = (from s in context.RevisionChoices
                                           where s.course_id == CurrentCourseID && s.whether_show_revision_choice == "Y" && s.show_revision_choice_till_date >= DateTime.Now
                                           orderby s.revision_choice_effective_date descending
                                           select s).FirstOrDefault();
                    if (revisiondetails != null)

                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Int32 IsAlreadyApplied(EConnectContext context, Int32 currentExamID, Int32 courseID, Int64 registrationNumber, Int64 candidateID)//find revision number of non final submit-----02032020
        {
            try
            {
                int revision_number = (from d in context.CourseExamApplicationDetails
                                       join c in context.CourseExamApplications
                                       on d.ExamID equals c.ExamID
                                       join m in context.Modules on d.ModuleID equals m.ID
                                       where (c.CourseID == courseID &&
                                               c.CandidateID == candidateID &&
                                               c.RegistrationNumber == d.RegistrationNumber &&
                                               c.RegistrationNumber == registrationNumber && c.ExamID == currentExamID
                                               && c.FinalSubmitted == false)
                                       select m.RevisionNumber).FirstOrDefault();
                if (revision_number > 0)
                {
                    return revision_number;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Int32 IsFinalSubmitted(EConnectContext context, Int32 currentExamID, Int32 courseID, Int64 registrationNumber, Int64 candidateID)//find revision number of  final submit-----14032020
        {
            try
            {
                int revision_number = (from d in context.CourseExamApplicationDetails
                                       join c in context.CourseExamApplications
                                       on d.ExamID equals c.ExamID
                                       join m in context.Modules on d.ModuleID equals m.ID
                                       where (c.CourseID == courseID &&
                                               c.CandidateID == candidateID &&
                                               c.RegistrationNumber == registrationNumber
                                               && c.FinalSubmitted == true && c.PaymentStatusID != null && c.DemandNoteID != null)
                                       select m.RevisionNumber).FirstOrDefault();
                if (revision_number > 0)
                {
                    return revision_number;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Boolean IsNewRegisteredCandidate(Int32 CurrentCourseID, Int64 registrationNumber)//Show new revision to fresh candidate only -----13032020
        {
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var registration = (from d in context.RevisionChoices
                                        join c in context.RegistrationDetails
                                        on d.course_id equals c.CourseID
                                        where (c.CommencementFromDate >= d.registration_date_to_show //&& c.CommencementFromDate <=d.registration_date_show_upto 
                                        && c.RegistrationNo == registrationNumber && c.CourseID == CurrentCourseID)
                                        select d).FirstOrDefault();

                    if (registration != null)

                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Boolean IsCandidateDebarred(Int32 CurrentCourseID, Int64 registrationNumber)//whether candidate is debarred -----01062020
        {
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var debarred = (from d in context.DebarredCandidates
                                    where d.course_id == CurrentCourseID && d.registration_number == registrationNumber
                                    select d.whether_active).FirstOrDefault();

                    if (debarred == "Y")
                    {
                        int examid_count = (from e in context.Exams
                                            join d in context.DebarredCandidates
                                            on e.CourseID equals d.course_id
                                            where e.ID > d.debarred_for_examcycle && d.course_id == CurrentCourseID && d.registration_number == registrationNumber && d.whether_active == "Y"
                                            select e.ID).Distinct().Count();

                        int examid_debarred_releasing_count = (from d in context.DebarredCandidates
                                                               where d.course_id == CurrentCourseID && d.registration_number == registrationNumber && d.whether_active == "Y"
                                                               select d.debarred_upto_examcycle_count).FirstOrDefault();

                        if (examid_count > 0 && examid_count > 0 && examid_count > examid_debarred_releasing_count)
                            return true;
                        else
                            return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Int16 Eligibility_Method_For_Apply_In_Old_Exam_Pattren_O_LVL(Int64 registrationNumber)
        {

            string constr =   ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
          //  string constr = confi
            DataTable myDt = new DataTable();
            SqlConnection con = new SqlConnection(constr);

            try
            {
                SqlCommand cmd = new SqlCommand("Eligibility_Check_For_Apply_In_Old_Exam_Pattren_O_LVL", con);


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_Reg_no", SqlDbType.Int);
                cmd.Parameters["@P_Reg_no"].Value = registrationNumber;


                cmd.Parameters.Add("@P_Elegible", SqlDbType.Int, 500);
                cmd.Parameters["@P_Elegible"].Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();
                Int16 isEligible;
                isEligible = Convert.ToInt16(cmd.Parameters["@P_Elegible"].Value);

                return isEligible;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public static Int16 Eligibility_Method_For_Apply_In_Old_Exam_Pattren_A_LVL(Int64 registrationNumber)
        {

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            DataTable myDt = new DataTable();
            SqlConnection con = new SqlConnection(constr);

            try
            {
                SqlCommand cmd = new SqlCommand("Eligibility_Check_For_Apply_In_Old_Exam_Pattren_A_LVL", con);


                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@P_Reg_no", SqlDbType.Int);
                cmd.Parameters["@P_Reg_no"].Value = registrationNumber;


                cmd.Parameters.Add("@P_Elegible", SqlDbType.Int, 500);
                cmd.Parameters["@P_Elegible"].Direction = ParameterDirection.Output;

                con.Open();

                cmd.ExecuteNonQuery();
                Int16 isEligible;
                isEligible = Convert.ToInt16(cmd.Parameters["@P_Elegible"].Value);

                return isEligible;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }



        public static Boolean IsEligibleForProject(Int32 CurrentCourseID, Int64 registrationNumber, Int64 candidateID, string projectShortName)//To check the elligibility of the candidate for project module
        {
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 RevisionNumber = 0;
                    bool Is_Level_Upgreaded = false;
                    int theorymodule = Convert.ToInt32(enmModuleType.Theory);
                    int projectmodule = Convert.ToInt32(enmModuleType.Project);
                    int bridgemodule = Convert.ToInt32(enmModuleType.Bridge);

                    var passed_theory_modules = CourseManager.GetPassedModulesListOfAnyRevision(context, CurrentCourseID, registrationNumber, candidateID);
                    var attempted_theory_modules = CourseManager.GetListOfAttempteddModulesOfAnyRevision(context, CurrentCourseID, registrationNumber, candidateID);

                    //Added on 13 May 2024 for CHM(T)-O Level project
                    if (CurrentCourseID == 1213)
                    {

                        // var moduleNumbers = Enumerable.Range(1, 4); // Assuming module numbers 1, 2, 3, 4

                        //// var allModulesPassed = true;

                        // foreach (var moduleNumber in moduleNumbers)
                        // {
                        //     var moduleExists = context.Modules.Any(m => m.CourseID == CurrentCourseID && m.ProjectNumber == moduleNumber && m.ModuleTypeID == 1);

                        //     if (!moduleExists)
                        //     {
                        //        // allModulesPassed = false;
                        //         return false;
                        //         break; // No need to continue checking if any module is missing
                        //     }

                        //     var project_eligilbilities = (
                        //                                     from m in context.Modules
                        //                                     where m.CourseID == CurrentCourseID && m.ModuleTypeID == 1 &&
                        //                                           (
                        //                                               from cead in context.CourseExamApplicationDetails
                        //                                               where cead.RegistrationNumber == registrationNumber && cead.CourseID == CurrentCourseID &&
                        //                                                     (
                        //                                                         from cea in context.CourseExamApplications
                        //                                                         where cead.CourseExamApplicationID == cea.ID && (cea.PaymentStatusID == 2 || cea.PaymentStatusID == 4)
                        //                                                         select 1
                        //                                                     ).Any() &&
                        //                                                     (
                        //                                                         from c in context.Modules
                        //                                                         where c.CourseID == CurrentCourseID && c.ModuleNUmber == moduleNumber && c.ModuleTypeID == 1
                        //                                                         select 1
                        //                                                     ).Any()
                        //                                               select 1
                        //                                           ).Count() >= 1
                        //                                     select 1
                        //                                 ).Any();

                        //     //if (project_eligilbilities == 1)
                        //     //{
                        //     //    re
                        //     //}

                        //     //if (!moduleConditionPassed)
                        //     //{
                        //     //    allModulesPassed = false;
                        //     //    break; // No need to continue checking if any module condition is not met
                        //     //}
                        // }
                        //// var result = allModulesPassed;

                        var q1 = (from cead in context.CourseExamApplicationDetails
                                  where cead.RegistrationNumber == registrationNumber
                                     && cead.CourseID == CurrentCourseID
                                     && (from cea in context.CourseExamApplications
                                         where cead.CourseExamApplicationID == cea.ID
                                            && (cea.PaymentStatusID == 2 || cea.PaymentStatusID == 4)
                                         select 1).Any()
                                     && (from module in context.Modules
                                         where module.CourseID == CurrentCourseID
                                            && module.ModuleNUmber == 1
                                            && module.ModuleTypeID == 1
                                         select module.ID).Contains(cead.ModuleID)

                                  select 1).Any() ? 1 : 0;

                        var q2 = (from cead in context.CourseExamApplicationDetails
                                  where cead.RegistrationNumber == registrationNumber
                                     && cead.CourseID == CurrentCourseID
                                     && (from cea in context.CourseExamApplications
                                         where cead.CourseExamApplicationID == cea.ID
                                            && (cea.PaymentStatusID == 2 || cea.PaymentStatusID == 4)
                                         select 1).Any()
                                     && (from module in context.Modules
                                         where module.CourseID == 1213
                                            && module.ModuleNUmber == 2
                                            && module.ModuleTypeID == 1
                                         select module.ID).Contains(cead.ModuleID)

                                  select 1).Any() ? 1 : 0;

                        var q3 = (from cead in context.CourseExamApplicationDetails
                                  where cead.RegistrationNumber == registrationNumber
                                     && cead.CourseID == CurrentCourseID
                                     && (from cea in context.CourseExamApplications
                                         where cead.CourseExamApplicationID == cea.ID
                                            && (cea.PaymentStatusID == 2 || cea.PaymentStatusID == 4)
                                         select 1).Any()
                                     && (from module in context.Modules
                                         where module.CourseID == 1213
                                            && module.ModuleNUmber == 3
                                            && module.ModuleTypeID == 1
                                         select module.ID).Contains(cead.ModuleID)

                                  select 1).Any() ? 1 : 0;
                        var q4 = (from cead in context.CourseExamApplicationDetails
                                  where cead.RegistrationNumber == registrationNumber
                                     && cead.CourseID == CurrentCourseID
                                     && (from cea in context.CourseExamApplications
                                         where cead.CourseExamApplicationID == cea.ID
                                            && (cea.PaymentStatusID == 2 || cea.PaymentStatusID == 4)
                                         select 1).Any()
                                     && (from module in context.Modules
                                         where module.CourseID == 1213
                                            && module.ModuleNUmber == 4
                                            && module.ModuleTypeID == 1
                                         select module.ID).Contains(cead.ModuleID)

                                  select 1).Any() ? 1 : 0;

                        //  var project_eligilbilities = q1.Union(q2).Union(q3).Union(q4);

                        var project_eligilbilities = q1 + q2 + q3 + q4;

                        if (project_eligilbilities == 4)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        ////////

                        var project_eligilbilities = (from e in context.ProjectEligibilities
                                                      where e.course_level_id == CurrentCourseID && e.whether_effective == true && e.project_shortname == projectShortName
                                                      select e).Distinct().FirstOrDefault();

                        if (project_eligilbilities != null)
                        {
                            if (project_eligilbilities.whether_max_revision_to_show == false)
                            {
                                RevisionNumber = attempted_theory_modules.Where(s => s.ModuleTypeID == theorymodule).Select(s => s.RevisionNumber).Distinct().Max();
                            }
                            else
                            {
                                RevisionNumber = CourseManager.GetCurrentCourseRevisionNumber(context, CurrentCourseID);
                            }
                        }

                        //var passedmodulesofcurrentrevision = CourseManager.GetPassedModulesOfCurrentRevision(context, CurrentCourseID, registrationNumber, RevisionNumber, candidateID).ToList();

                        //var projectnameforvalidation = CourseManager.GetModulesList(context,CurrentCourseID,RevisionNumber).Where(s=>s.ModuleTypeID==projectmodule).OrderBy(s=>s.ProjectNumber).Select(s=>s.ShortName).ToList();

                        var prior_courseLevel_history = CourseManager.GetPassedModulesListOfAnyRevision(context, (CurrentCourseID - 1), registrationNumber, candidateID);
                        //if (prior_courseLevel_history.Where(s => s.ModuleTypeID == theorymodule) != null) //&& prior_level_history.Where(s => s.ModuleTypeID == 1).Count()==context.Modules.Where(s => s.RevisionNumber == RevisionNumber && s.ModuleTypeID == 1 || s.ModuleTypeID == 5).Count())
                        //{
                        //    Is_Level_Upgreaded = true;
                        //}

                        if (project_eligilbilities.passed_theory_modules_number_upto == 0)
                        {
                            project_eligilbilities.passed_theory_modules_number_upto = passed_theory_modules.Where(s => s.ModuleTypeID == theorymodule).Select(s => s.ModuleNUmber).Distinct().Max();
                        }
                        if (project_eligilbilities.appeared_theory_modules_number_upto == 0)
                        {
                            project_eligilbilities.appeared_theory_modules_number_upto = attempted_theory_modules.Where(s => s.ModuleTypeID == theorymodule).Select(s => s.ModuleNUmber).Distinct().Max();
                        }
                        if (project_eligilbilities.whether_level_upgreaded_exemption_require == true && prior_courseLevel_history.Where(s => s.ModuleTypeID == theorymodule) != null)
                        {
                            Is_Level_Upgreaded = true;
                        }

                        var theory_modules_passed = passed_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.passed_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.passed_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count();
                        var bridge_modules_passed = passed_theory_modules.Where(s => s.ModuleTypeID == bridgemodule).Count();
                        var total_modules_passed = theory_modules_passed + bridge_modules_passed;

                        var theory_modules_attempted = attempted_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.appeared_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.appeared_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count();
                        var bridge_modules_attempted = attempted_theory_modules.Where(s => s.ModuleTypeID == bridgemodule).Count();
                        var total_modules_attempted = theory_modules_attempted + bridge_modules_attempted;

                        if (Is_Level_Upgreaded || (total_modules_passed >= project_eligilbilities.min_theory_modules_passed
                            && total_modules_attempted >= project_eligilbilities.min_theory_modules_appeared))
                        {
                            return true;
                        }


                        ////////////----Eligibility check for 'O'-Level Candidate------
                        //////////if (CurrentCourseID == 1)
                        //////////{
                        //////////    if (attempted_theory_modules.Where(s => s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_appeared)
                        //////////    {
                        //////////        return true;
                        //////////    }                  
                        //////////}

                        ////////////----Eligibility check for 'A'-Level Candidate------
                        //////////else if (CurrentCourseID == 2)
                        //////////{
                        //////////    foreach (var ShortName in projectnameforvalidation)
                        //////////    {
                        //////////        if (projectShortName == ShortName)
                        //////////        {
                        //////////            //var result1 = passed_theory_modules.Where(s => s.ModuleNUmber >= 1 && s.ModuleNUmber <= 4 && s.ModuleTypeID==1).Count();
                        //////////            //var result2 = attempted_theory_modules.Where(s => s.ModuleNUmber >= 5 && s.ModuleTypeID==1).Count();
                        //////////            //var result22 = attempted_theory_modules.ToList();
                        //////////            //var result11 = passed_theory_modules.Where(s => s.ModuleNUmber >= 1 && s.ModuleNUmber <= 4 && s.ModuleTypeID == 1).Select(s=> s.ModuleNUmber).ToList();
                        //////////            //var result21 = attempted_theory_modules.Where(s => s.ModuleNUmber >= 5 && s.ModuleTypeID == 1).Select(s=> s.ModuleNUmber).ToList();
                        //////////            if (Is_Level_Upgreaded || (passed_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.passed_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.passed_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_passed
                        //////////                && attempted_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.appeared_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.appeared_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_appeared))
                        //////////            {
                        //////////                return true;
                        //////////            }
                        //////////        }
                        //////////        else if (projectShortName == ShortName)
                        //////////        {
                        //////////            if (passed_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.passed_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.passed_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_passed
                        //////////                && attempted_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.appeared_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.appeared_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_appeared)
                        //////////            {
                        //////////                return true;
                        //////////            }
                        //////////        }
                        //////////    }
                        //////////}

                        ////////////----Eligibility check for 'B'-Level Candidate------
                        //////////else if (CurrentCourseID == 3)
                        //////////{
                        //////////    foreach (var ShortName in projectnameforvalidation)
                        //////////    {
                        //////////        if (projectShortName == ShortName)
                        //////////        {
                        //////////            if (Is_Level_Upgreaded || (passed_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.passed_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.passed_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_passed
                        //////////                && attempted_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.appeared_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.appeared_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_appeared))
                        //////////            {
                        //////////                return true;
                        //////////            }
                        //////////        }
                        //////////        else if (projectShortName == ShortName)
                        //////////        {
                        //////////            if (passed_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.passed_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.passed_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_passed
                        //////////                && attempted_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.appeared_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.appeared_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_appeared)
                        //////////            {
                        //////////                return true;
                        //////////            }
                        //////////        }
                        //////////        else if (projectShortName == ShortName)
                        //////////        {
                        //////////            if (passed_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.passed_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.passed_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_passed
                        //////////                && attempted_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.appeared_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.appeared_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_appeared)
                        //////////            {
                        //////////                return true;
                        //////////            }
                        //////////        }
                        //////////    }
                        //////////}
                        ////////////----Eligibility check for 'C'-Level Candidate------
                        //////////else if (CurrentCourseID == 4)
                        //////////{
                        //////////    var result1 = passed_theory_modules.Count();
                        //////////    var result2 = attempted_theory_modules.Count();

                        //////////    var result11 = passed_theory_modules.ToList();
                        //////////    var result21 = attempted_theory_modules.ToList();

                        //////////        if (projectShortName == "PJ1")
                        //////////        {                                
                        //////////            if (passed_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.passed_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.passed_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_passed
                        //////////                && attempted_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.appeared_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.appeared_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_appeared)
                        //////////            {
                        //////////                return true;
                        //////////            }
                        //////////        }
                        //////////        else if (projectShortName == "PJ2")
                        //////////        {
                        //////////            if (passed_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.passed_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.passed_theory_modules_number_upto && s.ModuleTypeID == theorymodule ).Count() >= project_eligilbilities.min_theory_modules_passed //context.Modules.Where(s => s.RevisionNumber == RevisionNumber && (s.ModuleTypeID == theorymodule || s.ModuleTypeID == Convert.ToInt32(enmModuleType.Bridge))).Count()
                        //////////                && attempted_theory_modules.Where(s => s.ModuleNUmber >= project_eligilbilities.appeared_theory_modules_number_from && s.ModuleNUmber <= project_eligilbilities.appeared_theory_modules_number_upto && s.ModuleTypeID == theorymodule).Count() >= project_eligilbilities.min_theory_modules_appeared)
                        //////////            {
                        //////////                return true;
                        //////////            }
                        //////////        }                       
                        //////////}
                        return false;
                    }
                }
            } // Added closing bracket 13 May 2024
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


}
