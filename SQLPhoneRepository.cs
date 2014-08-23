using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;
using System.Web.Script.Serialization;
using CwiAPI.Entities;
using System.Linq.Expressions;

namespace CwiAPI.Concrete
{
    using wLinqExtensions;

    public class SQLPhoneRepository : EntityContainer, IPhoneRepository
    {

        #region getAllPhoneDetails
        /// <summary>
        /// gets Phone Brands and Models compatible for the system Ids passed 
        /// </summary>
        /// <param name="testInstanceId">systemids in a string seperated with ',' </param>
        /// <returns>Jquery string with all the Brands and Models </returns>
        public string getAllPhoneDetails(string SystemIds)
        {
            //string reqdInfo = "";
            //List<TestScript> testScripts = new List<TestScript>();

            //String[] systemIds = SystemIds.Split(',');

            //foreach (var systemId in systemIds)
            //{
            //    Int32 id = Convert.ToInt32(systemId);
            //    testScripts.AddRange(_entities.TestScripts.Where(testscript => testscript.Device.ID == id));
            //}

            //List<TestInstance> testInstances = new List<TestInstance>();

            //foreach (var testScript in testScripts)
            //{
            //    testInstances.AddRange(_entities.TestInstances.Where(testinstance => testinstance.TestScript.ID == testScript.ID ));
            //}

            //List<Int32> phoneSystemIds = new List<Int32>();
            //List<phoneModels> models = new List<phoneModels>();
            //List<Int32> brandIds = new List<Int32>();
            //List<phoneBrands> brands = new List<phoneBrands>();

            //_entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, _entities.Devices);
            //_entities.Refresh(System.Data.Objects.RefreshMode.StoreWins, _entities.Brands);

            //foreach (var testInst in testInstances)
            //{

            //    #region set image path for phone
            //    string photo = testInst.Device.Photo;
            //    if (photo.StartsWith("~"))
            //    {
            //        photo = photo.Substring(1);
            //        photo.Trim();
            //    }
            //    if (!photo.StartsWith("/"))
            //    {
            //        photo = "/" + photo;
            //    }
            //    #endregion
            //    phoneSystemIds.Add(testInst.Device.ID);
            //    models.Add(new phoneModels(testInst.Device.ID, testInst.Device.Model , testInst.Device.Brand.ID, photo , testInst.ID));
            //    int brandId = ((from device in _entities.Devices
            //                     where device.ID == testInst.Device.ID
            //                     select device.Brand.ID).Distinct()).First();
            //    //brands.Contains
            //    if (!brandIds.Contains(brandId))
            //    {
            //        brandIds.Add(brandId);
            //    }
            //}
            //foreach (var brandId in brandIds)
            //{
            //    brands.Add(new phoneBrands(brandId, _entities.Brands.Where(brand => brand.ID == brandId).First().Name));
            //}

            //JavaScriptSerializer sr = new JavaScriptSerializer();
            //string reqdBrands = sr.Serialize(brands);
            //string reqdModels = sr.Serialize(models);

            //reqdInfo = "{\"brands\":" + reqdBrands + ",\"models\":" + reqdModels  + "}";

            return "";
        }
        #endregion

        #region get Top Features Results
        /// <summary>
        /// gets Results for Top Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the Top feature results for that instance</returns>
        public string getTopFeatureResults(int testInstanceId)
        {

            string finalcomment = "";

            var details = from testInst in _entities.TestInstances
                          where testInst.ID == testInstanceId
                          from topFeatRs in _entities.TopFeatureResults
                          where topFeatRs.TestInstance.ID == testInstanceId
                          select new
                          {
                              Conclusion = testInst.TestInstanceConclusion.Conclusion,
                              TopFeatureResultId = topFeatRs.ID,
                              TopFeatureId = topFeatRs.TopFeature.ID,
                              TopFeatureResult = topFeatRs.Result,
                              TopFeatureName = topFeatRs.TopFeature.Name,
                              CommentText = finalcomment
                          };
            if (details.First().Conclusion.ToLower() == "not Recommended")
            {
                finalcomment = getFinalComments(testInstanceId);
            }
            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdResutls = sr.Serialize(details);
            return reqdResutls;

        }
        #endregion

        #region getMainFeatureResults
        /// <summary>
        /// gets Results for Main Feature 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the main feature results for that instance</returns>
        public string getMainFeatureResults(int testInstanceId)
        {

            List<mainFeatureResults> details = (from testInst in _entities.TestInstances
                                                where testInst.ID == testInstanceId
                                                from ans in testInst.TestAnswers
                                                orderby ans.TestQuestion.Feature.Section.SectionNumber, ans.TestQuestion.Feature.OrderNumber
                                                where ans.TestQuestion.Feature != null && ans.TestQuestion.Feature.FeatureType.ID == 1
                                                select new mainFeatureResults
                                                {
                                                    TestInstanceId = testInstanceId,
                                                    SectionNumber = ans.TestQuestion.Feature.Section.SectionNumber,
                                                    SectionName = ans.TestQuestion.Feature.Section.SectionName,
                                                    FeatureName = ans.TestQuestion.Feature.Name,
                                                    Passed = ans.Passed,
                                                    OrderNumber = ans.TestQuestion.Feature.OrderNumber
                                                }).Distinct().OrderBy(det => det.SectionNumber).ThenBy(det => det.OrderNumber).ToList();

            #region Distinct features
            List<mainFeatureResults> distinctDtls = new List<mainFeatureResults>();

            foreach (var detail in details)
            {
                bool exists = false;
                if (distinctDtls.Count == 0)
                {
                    distinctDtls.Add(detail);
                }
                else
                {
                    foreach (var dtl in distinctDtls)
                    {
                        if (detail.SectionName == dtl.SectionName && detail.FeatureName == dtl.FeatureName)
                        {
                            exists = true;
                            if (detail.Passed == 0)
                            {
                                dtl.Passed = 0;
                            }
                            break;
                        }
                        else
                        {
                            exists = false;
                        }
                    }
                    if (!exists)
                    {
                        distinctDtls.Add(detail);
                    }
                }
            }
            #endregion

            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdResutls = sr.Serialize(distinctDtls);
            return reqdResutls;

        }
        #endregion

        #region getAdditionalFeatureResults
        /// <summary>
        /// gets Results for additional Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the additional feature results for that instance</returns>
        public string getAdditionalFeatureResults(int testInstanceId)
        {

            List<mainFeatureResults> details = (from testInst in _entities.TestInstances
                                                where testInst.ID == testInstanceId
                                                from ans in testInst.TestAnswers
                                                orderby ans.TestQuestion.Feature.Section.SectionNumber, ans.TestQuestion.Feature.OrderNumber
                                                where ans.TestQuestion.Feature != null && ans.TestQuestion.Feature.FeatureType.ID == 2
                                                select new mainFeatureResults
                                                {
                                                    TestInstanceId = testInstanceId,
                                                    SectionNumber = ans.TestQuestion.Feature.Section.SectionNumber,
                                                    SectionName = ans.TestQuestion.Feature.Section.SectionName,
                                                    FeatureName = ans.TestQuestion.Feature.Name,
                                                    Passed = ans.Passed,
                                                    OrderNumber = ans.TestQuestion.Feature.OrderNumber
                                                }).Distinct().OrderBy(det => det.SectionNumber).ThenBy(det => det.OrderNumber).ToList();

            #region Distinct features
            List<mainFeatureResults> distinctDtls = new List<mainFeatureResults>();
            foreach (var detail in details)
            {
                bool exists = false;
                if (distinctDtls.Count == 0)
                {
                    distinctDtls.Add(detail);
                }
                else
                {
                    foreach (var dtl in distinctDtls)
                    {
                        if (detail.SectionName == dtl.SectionName && detail.FeatureName == dtl.FeatureName)
                        {
                            exists = true;
                            if (detail.Passed == 0)
                            {
                                dtl.Passed = 0;
                            }
                            break;
                        }
                        else
                        {
                            exists = false;
                        }
                    }
                    if (!exists)
                    {
                        distinctDtls.Add(detail);
                    }
                }
            }
            #endregion


            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdResutls = sr.Serialize(distinctDtls);
            return reqdResutls;

        }
        #endregion

        #region getDistinctPhoneDetails
        /// <summary>
        /// get Distinct Phone Details
        /// </summary>
        /// <param name="SystemIds"></param>
        /// <returns></returns>
        public string getDistinctPhoneDetails(string SystemIds)
        {
            try
            {

                string reqdInfo = "";
                List<TestScript> testScripts = new List<TestScript>();
                String[] systemIds = SystemIds.Split(',');
                JavaScriptSerializer sr = new JavaScriptSerializer();

                List<phoneModels> allDetails = new List<phoneModels>();
                List<topFeatureResults> topfeatureResult = new List<topFeatureResults>();
                List<finalComment> finalComments = new List<finalComment>();
                //Random random = new Random();
                #region getting all the phones for a System
                foreach (var systemId in systemIds)
                {
                    Int32 id = Convert.ToInt32(systemId);

                    allDetails.AddRange((from t in _entities.TestScripts
                                         where t.Device.ID == id
                                         from i in t.TestInstances
                                         where i.TestStatusID == 8
                                         orderby i.Device.ID, i.DeviceSoftwareVersion descending
                                         select new phoneModels
                                         {
                                             RelatedSystemID = id,
                                             PhoneID = i.Device.ID,
                                             PhoneSoftwareVersion = i.DeviceSoftwareVersion,
                                             Name = i.Device.Model,
                                             BrandId = i.Device.Brand.ID,
                                             BrandName = i.Device.Brand.Name,
                                             //Photo = i.Device.Photo.StartsWith("~") ? i.Device.Photo.Substring(1).Trim() : "/" + i.Device.Photo,
                                             Photo = i.Device.Photo,
                                             TestInstanceId = i.ID
                                         }).ToList());


                }
                #endregion
                #region extracting distinct phones for a All the phones extracted earlier
                List<phoneModels> distinctDtls = new List<phoneModels>();
                foreach (var detail in allDetails)
                {
                    //Int32 testIntsanceId = 0;
                    bool exists = false;
                    if (distinctDtls.Count == 0)
                    {
                        distinctDtls.Add(detail);
                        distinctDtls[0].PhoneSoftwareVersion = distinctDtls[0].TestInstanceId.ToString() + "|" + distinctDtls[0].PhoneSoftwareVersion;
                        //distinctDtls[0].Photo = distinctDtls[0].Photo + "?" + random.Next();
                    }
                    else
                    {
                        foreach (var dtl in distinctDtls)
                        {
                            if (dtl.PhoneID == detail.PhoneID)
                            {
                                exists = true;
                                break;
                            }
                            else
                            {
                                exists = false;
                            }
                        }
                        if (!exists)
                        {
                            distinctDtls.Add(detail);
                            distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion = distinctDtls[distinctDtls.Count - 1].TestInstanceId.ToString() + "|"
                                                                                            + distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion;
                            //distinctDtls[distinctDtls.Count - 1].Photo = distinctDtls[distinctDtls.Count - 1].Photo + "?" + random.Next();
                        }

                    }

                }
                distinctDtls = distinctDtls.OrderBy(dtl => dtl.BrandName).ThenBy(dtl => dtl.Name).ToList();
                #endregion
                #region Getting Brands for distinct Phones
                var brandIds = (from dtl in distinctDtls
                                orderby dtl.BrandId ascending
                                select dtl.BrandId).Distinct().ToList();

                List<phoneBrands> brands = new List<phoneBrands>();

                foreach (var brandId in brandIds)
                {
                    brands.Add(new phoneBrands(brandId, _entities.Brands.Where(brand => brand.ID == brandId).First().Name));
                }
                #endregion
                #region reqd Results
                foreach (var detail in distinctDtls)
                {
                    #region final comment
                    List<string> finalCommnts = (from testInst in _entities.TestInstances
                                                 where testInst.ID == detail.TestInstanceId
                                                 from commt in testInst.Comments
                                                 from commtCult in commt.CommentCultureDetails
                                                 where commtCult.Culture.Locale == "English"
                                                 select commtCult.CommentText).ToList();

                    string reqdCommnts = "";
                    foreach (var comment in finalCommnts)
                    {
                        reqdCommnts = reqdCommnts + comment;
                    }

                    #endregion
                    #region top feaure results
                    topfeatureResult.AddRange(from testInst in _entities.TestInstances
                                              where testInst.ID == detail.TestInstanceId
                                              from topFeatRs in _entities.TopFeatureResults
                                              where topFeatRs.TestInstance.ID == detail.TestInstanceId
                                              select new topFeatureResults
                                              {
                                                  TestInstanceId = detail.TestInstanceId,
                                                  Conclusion = testInst.TestInstanceConclusion.Conclusion,
                                                  TopFeatureResultId = topFeatRs.ID,
                                                  TopFeatureId = topFeatRs.TopFeature.ID,
                                                  TopFeatureResult = topFeatRs.Result,
                                                  TopFeatureName = topFeatRs.TopFeature.Name,
                                                  CommentText = reqdCommnts
                                              });
                    #endregion
                }

                #endregion

                string stopfeatureResult = sr.Serialize(topfeatureResult);
                string sfinalComments = sr.Serialize(finalComments);
                string reqdBrands = sr.Serialize(brands);
                string reqdModels = sr.Serialize(from dst in distinctDtls
                                                 orderby dst.BrandName, dst.Name
                                                 select dst
                                                );
                //",\"finalComments\":" + sfinalComments +
                reqdInfo = "{\"brands\":" + reqdBrands + ",\"models\":" + reqdModels + ",\"topFeatureResults\":" + stopfeatureResult + "}";

                return reqdInfo;

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                throw;
            }
        }
        #endregion

        #region get Comments
        /// <summary>
        /// Get Comments for a test instance
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with comments for each section</returns>
        public string getComments(int testInstanceId)
        {

            var comments = from i in _entities.TestInstances
                           where i.ID == testInstanceId
                           from a in i.TestAnswers
                           from c in a.Comments
                           from ccd in c.CommentCultureDetails
                           where ccd.Culture.Locale == "English"
                           select new
                           {
                               testInstanceId = testInstanceId,
                               c.Section.SectionNumber,
                               SectionName = c.Section.SectionName,
                               comment = ccd.CommentText
                           };

            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdCommnts = sr.Serialize(comments.OrderBy(c => c.SectionNumber).Distinct());
            return reqdCommnts;
        }
        #endregion

        #region get Final Comments
        /// <summary>
        /// Get Final Comments for a test instance
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with Final comments for a Test Instance</returns>
        public string getFinalComments(int testInstanceId)
        {
            List<string> finalCommnts = (from testInst in _entities.TestInstances
                                         where testInst.ID == testInstanceId
                                         from commt in testInst.Comments
                                         from commtCult in commt.CommentCultureDetails
                                         where commtCult.Culture.Locale == "English"
                                         select commtCult.CommentText).ToList();

            string reqdCommnts = "";
            foreach (var comment in finalCommnts)
            {
                reqdCommnts = reqdCommnts + comment;
            }

            return reqdCommnts;
        }
        #endregion

        #region get Quick Guides
        /// <summary>
        /// Get Quick Guides for a test instance
        /// </summary>
        /// <param name="testInstanceId">Phone ID</param>
        /// <returns>Jquery string with Quick Guides for a Device</returns>
        public string getQuickGuides(int SystemId, int PhoneID)
        {
            string additonalAttribute = "";
            string additonalAttribute1 = "";
            string[] attribute = (from device in _entities.Devices
                                  where device.ID == SystemId
                                  from addAttValue in device.AdditionalAttributeValues
                                  where addAttValue.AdditionalAttributeID == 2 && addAttValue.AdditionalAttributeID == 4
                                  select addAttValue.AdditionalAttributeValuePrp).ToArray();

            if (attribute.Count() > 0)
            {
                additonalAttribute = attribute[0];
                additonalAttribute1 = attribute[1];
            }
            string tagToRplc = "<PARAMETER_NAME>";
            string tagToRplc1 = "<PARAMETER_PASSKEY>";
            #region Getting Quick guides
            var quickGuides = (from device in _entities.Devices
                               where device.ID == SystemId
                               from guide in device.QuickGuides
                               where guide.Discoverable == false
                               from step in guide.QuickGuideSteps
                               from cutltureDetail in step.QuickGuideText.QuickGuideTextCultureDetails
                               where cutltureDetail.Culture.ID == 23
                               select new
                               {
                                   GuideType = "system",
                                   DeviceId = device.ID,
                                   Section = step.QuickGuideSection.Section,
                                   //cutltureDetail.QuickGuideText,
                                   QuickGuideText = (cutltureDetail.QuickGuideText.Contains(tagToRplc) == false ? (cutltureDetail.QuickGuideText.Contains(tagToRplc1) == false ? cutltureDetail.QuickGuideText.Replace("''", "\"") : cutltureDetail.QuickGuideText.Replace(tagToRplc1, additonalAttribute1).Replace("''", "\"")) : cutltureDetail.QuickGuideText.Replace(tagToRplc, additonalAttribute).Replace("''", "\"")),
                                   SectionNumber = step.QuickGuideSection.SectionNumber,
                                   Grouping = step.Grouping,
                                   Step = step.Step,
                                   Discoverable = guide.Discoverable
                               }).ToList();

            quickGuides.AddRange((from device in _entities.Devices
                                  where device.ID == PhoneID
                                  from guide in device.QuickGuides
                                  where guide.Discoverable == true
                                  from step in guide.QuickGuideSteps
                                  from cutltureDetail in step.QuickGuideText.QuickGuideTextCultureDetails
                                  where cutltureDetail.Culture.ID == 23
                                  select new
                                  {
                                      GuideType = "phone",
                                      DeviceId = device.ID,
                                      Section = step.QuickGuideSection.Section,
                                      //cutltureDetail.QuickGuideText,
                                      QuickGuideText = (cutltureDetail.QuickGuideText.Contains(tagToRplc) == false ? (cutltureDetail.QuickGuideText.Contains(tagToRplc1) == false ? cutltureDetail.QuickGuideText.Replace("''", "\"") : cutltureDetail.QuickGuideText.Replace(tagToRplc1, additonalAttribute1).Replace("''", "\"")) : cutltureDetail.QuickGuideText.Replace(tagToRplc, additonalAttribute).Replace("''", "\"")),
                                      SectionNumber = step.QuickGuideSection.SectionNumber,
                                      Grouping = step.Grouping,
                                      Step = step.Step,
                                      Discoverable = guide.Discoverable
                                  }).ToList());
            #endregion
            foreach (var quickguide in quickGuides)
            {
                if (true)
                {

                }
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string reqdData = ser.Serialize(quickGuides.Distinct().OrderBy(quck => quck.SectionNumber).ThenBy(quck => quck.Grouping).ThenBy(quck => quck.Step));
            return reqdData;
        }
        #endregion

        #region getDistinctPhoneDetails
        /// <summary>
        /// get Distinct Phone Details
        /// </summary>
        /// <param name="SystemIds"></param>
        /// <returns></returns>
        public string getDistinctPhoneDetailsWithRating(string SystemIds)
        {
            try
            {

                string reqdInfo = "";
                List<TestScript> testScripts = new List<TestScript>();
                String[] systemIds = SystemIds.Split(',');
                JavaScriptSerializer sr = new JavaScriptSerializer();

                List<phoneModels> allDetails = new List<phoneModels>();
                List<topFeatureResultsWithRating> topfeatureResult = new List<topFeatureResultsWithRating>();
                List<finalComment> finalComments = new List<finalComment>();
                //Random random = new Random();
                #region getting all the phones for a System
                foreach (var systemId in systemIds)
                {
                    Int32 id = Convert.ToInt32(systemId);

                    allDetails.AddRange((from t in _entities.TestScripts
                                         where t.Device.ID == id
                                         from i in t.TestInstances
                                         where i.TestStatusID == 8
                                         orderby i.Device.ID, i.DeviceSoftwareVersion descending
                                         select new phoneModels
                                         {
                                             RelatedSystemID = id,
                                             PhoneID = i.Device.ID,
                                             PhoneSoftwareVersion = i.DeviceSoftwareVersion,
                                             Name = i.Device.Model,
                                             BrandId = i.Device.Brand.ID,
                                             BrandName = i.Device.Brand.Name,
                                             //Photo = i.Device.Photo.StartsWith("~") ? i.Device.Photo.Substring(1).Trim() : "/" + i.Device.Photo,
                                             Photo = i.Device.Photo,
                                             TestInstanceId = i.ID
                                         }).ToList());


                }
                #endregion
                #region extracting distinct phones for a All the phones extracted earlier
                List<phoneModels> distinctDtls = new List<phoneModels>();
                foreach (var detail in allDetails)
                {
                    //Int32 testIntsanceId = 0;
                    bool exists = false;
                    if (distinctDtls.Count == 0)
                    {
                        distinctDtls.Add(detail);
                        distinctDtls[0].PhoneSoftwareVersion = distinctDtls[0].TestInstanceId.ToString() + "|" + distinctDtls[0].PhoneSoftwareVersion;
                        //distinctDtls[0].Photo = distinctDtls[0].Photo + "?" + random.Next();
                    }
                    else
                    {
                        foreach (var dtl in distinctDtls)
                        {
                            if (dtl.PhoneID == detail.PhoneID)
                            {
                                exists = true;
                                break;
                            }
                            else
                            {
                                exists = false;
                            }
                        }
                        if (!exists)
                        {
                            distinctDtls.Add(detail);
                            distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion = distinctDtls[distinctDtls.Count - 1].TestInstanceId.ToString() + "|"
                                                                                            + distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion;
                            //distinctDtls[distinctDtls.Count - 1].Photo = distinctDtls[distinctDtls.Count - 1].Photo + "?" + random.Next();
                        }

                    }

                }
                distinctDtls = distinctDtls.OrderBy(dtl => dtl.BrandName).ThenBy(dtl => dtl.Name).ToList();
                #endregion
                #region Getting Brands for distinct Phones
                var brandIds = (from dtl in distinctDtls
                                orderby dtl.BrandId ascending
                                select dtl.BrandId).Distinct().ToList();

                List<phoneBrands> brands = new List<phoneBrands>();

                foreach (var brandId in brandIds)
                {
                    brands.Add(new phoneBrands(brandId, _entities.Brands.Where(brand => brand.ID == brandId).First().Name));
                }
                #endregion

                string stopfeatureResult = sr.Serialize(topfeatureResult);
                string sfinalComments = sr.Serialize(finalComments);
                string reqdBrands = sr.Serialize(brands);
                string reqdModels = sr.Serialize(from dst in distinctDtls
                                                 orderby dst.BrandName, dst.Name
                                                 select dst
                                                );
                //",\"finalComments\":" + sfinalComments +
                reqdInfo = "{\"brands\":" + reqdBrands + ",\"models\":" + reqdModels + ",\"topFeatureResults\":" + stopfeatureResult + "}";

                return reqdInfo;

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                throw;
            }
        }
        #endregion

        #region get Top Features Results ForCulture
        /// <summary>
        /// gets Results for Top Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the Top feature results for that instance</returns>
        public string getTopFeatureResultsForCulture(int testInstanceId, int cultureId)
        {

            string finalcomment = "";

            var details = from testInst in _entities.TestInstances
                          where testInst.ID == testInstanceId
                          from topFeatRs in _entities.TopFeatureResults
                          where topFeatRs.TestInstance.ID == testInstanceId
                          from cultureDetail in topFeatRs.TopFeature.TopFeatureCultureDetails
                          where cultureDetail.CultureID == cultureId
                          select new
                          {
                              Conclusion = testInst.TestInstanceConclusion.Conclusion,
                              TopFeatureResultId = topFeatRs.ID,
                              TopFeatureId = topFeatRs.TopFeature.ID,
                              TopFeatureResult = topFeatRs.Result,
                              TopFeatureName = cultureDetail.TopFeature ,
                          };

            if (details.First().Conclusion.ToLower() == "not Recommended")
            {
                finalcomment = getFinalComments(testInstanceId);
            }
            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdResutls = sr.Serialize(details);
            return reqdResutls;

        }
        #endregion

        #region getMainFeatureResults For culture
        /// <summary>
        /// gets Results for Main Feature 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the main feature results for that instance</returns>
        public string getMainFeatureResultsForCulture(int testInstanceId, int cultureId)
        {

            List<mainFeatureResults> details = (from testInst in _entities.TestInstances
                                                where testInst.ID == testInstanceId
                                                from ans in testInst.TestAnswers
                                                orderby ans.TestQuestion.Feature.Section.SectionNumber, ans.TestQuestion.Feature.OrderNumber
                                                where ans.TestQuestion.Feature != null && ans.TestQuestion.Feature.FeatureType.ID == 1
                                                from cultureDetail in ans.TestQuestion.Feature.FeatureCultureDetails
                                                where cultureDetail.Culture.ID == cultureId
                                                select new mainFeatureResults
                                                {
                                                    TestInstanceId = testInstanceId,
                                                    SectionNumber = ans.TestQuestion.Feature.Section.SectionNumber,
                                                    SectionName = ans.TestQuestion.Feature.Section.SectionName,
                                                    FeatureName = cultureDetail.Feature ,
                                                    Passed = ans.Passed,
                                                    OrderNumber = ans.TestQuestion.Feature.OrderNumber
                                                }).Distinct().OrderBy(det => det.SectionNumber).ThenBy(det => det.OrderNumber).ToList();

            #region Distinct features
            List<mainFeatureResults> distinctDtls = new List<mainFeatureResults>();

            foreach (var detail in details)
            {
                bool exists = false;
                if (distinctDtls.Count == 0)
                {
                    distinctDtls.Add(detail);
                }
                else
                {
                    foreach (var dtl in distinctDtls)
                    {
                        if (detail.SectionName == dtl.SectionName && detail.FeatureName == dtl.FeatureName)
                        {
                            exists = true;
                            if (detail.Passed == 0)
                            {
                                dtl.Passed = 0;
                            }
                            break;
                        }
                        else
                        {
                            exists = false;
                        }
                    }
                    if (!exists)
                    {
                        distinctDtls.Add(detail);
                    }
                }
            }
            #endregion

            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdResutls = sr.Serialize(distinctDtls);
            return reqdResutls;

        }
        #endregion

        #region getAdditionalFeatureResultsFor culture
        /// <summary>
        /// gets Results for additional Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the additional feature results for that instance</returns>
        public string getAdditionalFeatureResultsForCulture(int testInstanceId, int cultureId)
        {

            List<mainFeatureResults> details = (from testInst in _entities.TestInstances
                                                where testInst.ID == testInstanceId
                                                from ans in testInst.TestAnswers
                                                orderby ans.TestQuestion.Feature.Section.SectionNumber, ans.TestQuestion.Feature.OrderNumber
                                                where ans.TestQuestion.Feature != null && ans.TestQuestion.Feature.FeatureType.ID == 2
                                                from cultureDetail in ans.TestQuestion.Feature.FeatureCultureDetails
                                                where cultureDetail.Culture.ID == cultureId
                                                select new mainFeatureResults
                                                {
                                                    TestInstanceId = testInstanceId,
                                                    SectionNumber = ans.TestQuestion.Feature.Section.SectionNumber,
                                                    SectionName = ans.TestQuestion.Feature.Section.SectionName,
                                                    FeatureName = cultureDetail.Feature,
                                                    Passed = ans.Passed,
                                                    OrderNumber = ans.TestQuestion.Feature.OrderNumber
                                                }).Distinct().OrderBy(det => det.SectionNumber).ThenBy(det => det.OrderNumber).ToList();

            #region Distinct features
            List<mainFeatureResults> distinctDtls = new List<mainFeatureResults>();
            foreach (var detail in details)
            {
                bool exists = false;
                if (distinctDtls.Count == 0)
                {
                    distinctDtls.Add(detail);
                }
                else
                {
                    foreach (var dtl in distinctDtls)
                    {
                        if (detail.SectionName == dtl.SectionName && detail.FeatureName == dtl.FeatureName)
                        {
                            exists = true;
                            if (detail.Passed == 0)
                            {
                                dtl.Passed = 0;
                            }
                            break;
                        }
                        else
                        {
                            exists = false;
                        }
                    }
                    if (!exists)
                    {
                        distinctDtls.Add(detail);
                    }
                }
            }
            #endregion


            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdResutls = sr.Serialize(distinctDtls);
            return reqdResutls;

        }
        #endregion

        #region get Quick Guides
        /// <summary>
        /// Get Quick Guides for a test instance
        /// </summary>
        /// <param name="testInstanceId">Phone ID</param>
        /// <returns>Jquery string with Quick Guides for a Device</returns>
        public string getQuickGuidesForCulture(int SystemId, int PhoneID, int cultureId)
        {
            string additonalAttribute = "";
            string additonalAttribute1 = "";
            string[] attribute = (from device in _entities.Devices
                                  where device.ID == SystemId
                                  from addAttValue in device.AdditionalAttributeValues
                                  where addAttValue.AdditionalAttributeID == 2 && addAttValue.AdditionalAttributeID == 4
                                  select addAttValue.AdditionalAttributeValuePrp).ToArray();

            if (attribute.Count() > 0)
            {
                additonalAttribute = attribute[0];
                additonalAttribute1 = attribute[1];
            }
            string tagToRplc = "<PARAMETER_NAME>";
            string tagToRplc1 = "<PARAMETER_PASSKEY>";
            #region Getting Quick guides
            var quickGuides = (from device in _entities.Devices
                               where device.ID == SystemId
                               from guide in device.QuickGuides
                               where guide.Discoverable == false
                               from step in guide.QuickGuideSteps
                               from cutltureDetail in step.QuickGuideText.QuickGuideTextCultureDetails
                               where cutltureDetail.Culture.ID == cultureId
                               select new
                               {
                                   GuideType = "system",
                                   DeviceId = device.ID,
                                   Section = step.QuickGuideSection.Section,
                                   //cutltureDetail.QuickGuideText,
                                   QuickGuideText = (cutltureDetail.QuickGuideText.Contains(tagToRplc) == false ? (cutltureDetail.QuickGuideText.Contains(tagToRplc1) == false ? cutltureDetail.QuickGuideText.Replace("''", "\"") : cutltureDetail.QuickGuideText.Replace(tagToRplc1, additonalAttribute1).Replace("''", "\"")) : cutltureDetail.QuickGuideText.Replace(tagToRplc, additonalAttribute).Replace("''", "\"")),
                                   SectionNumber = step.QuickGuideSection.SectionNumber,
                                   Grouping = step.Grouping,
                                   Step = step.Step,
                                   Discoverable = guide.Discoverable
                               }).ToList();

            quickGuides.AddRange((from device in _entities.Devices
                                  where device.ID == PhoneID
                                  from guide in device.QuickGuides
                                  where guide.Discoverable == true
                                  from step in guide.QuickGuideSteps
                                  from cutltureDetail in step.QuickGuideText.QuickGuideTextCultureDetails
                                  where cutltureDetail.Culture.ID == cultureId
                                  select new
                                  {
                                      GuideType = "phone",
                                      DeviceId = device.ID,
                                      Section = step.QuickGuideSection.Section,
                                      //cutltureDetail.QuickGuideText,
                                      QuickGuideText = (cutltureDetail.QuickGuideText.Contains(tagToRplc) == false ? (cutltureDetail.QuickGuideText.Contains(tagToRplc1) == false ? cutltureDetail.QuickGuideText.Replace("''", "\"") : cutltureDetail.QuickGuideText.Replace(tagToRplc1, additonalAttribute1).Replace("''", "\"")) : cutltureDetail.QuickGuideText.Replace(tagToRplc, additonalAttribute).Replace("''", "\"")),
                                      SectionNumber = step.QuickGuideSection.SectionNumber,
                                      Grouping = step.Grouping,
                                      Step = step.Step,
                                      Discoverable = guide.Discoverable
                                  }).ToList());
            #endregion
            foreach (var quickguide in quickGuides)
            {
                if (true)
                {

                }
            }
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string reqdData = ser.Serialize(quickGuides.Distinct().OrderBy(quck => quck.SectionNumber).ThenBy(quck => quck.Grouping).ThenBy(quck => quck.Step));
            return reqdData;
        }
        #endregion

        #region getDistinctPhoneDetailsForCulture
        /// <summary>
        /// get Distinct Phone Details
        /// </summary>
        /// <param name="SystemIds"></param>
        /// <returns></returns>
        public string getDistinctPhoneDetailsForCulture(string SystemIds , Int32 cultureId)
        {
            try
            {

                string reqdInfo = "";
                List<TestScript> testScripts = new List<TestScript>();
                String[] systemIds = SystemIds.Split(',');
                JavaScriptSerializer sr = new JavaScriptSerializer();

                List<phoneModels> allDetails = new List<phoneModels>();
                //List<topFeatureResults> topfeatureResult = new List<topFeatureResults>();
                List<finalComment> finalComments = new List<finalComment>();
                
                //Random random = new Random();
                #region getting all the phones for a System
                foreach (var systemId in systemIds)
                {
                    Int32 id = Convert.ToInt32(systemId);

                    allDetails.AddRange((from t in _entities.TestScripts
                                         where t.Device.ID == id
                                         from i in t.TestInstances
                                         where i.TestStatusID == 4
                                         orderby i.Device.ID, i.DeviceSoftwareVersion descending
                                         select new phoneModels
                                         {
                                             RelatedSystemID = id,
                                             PhoneID = i.Device.ID,
                                             PhoneSoftwareVersion = i.DeviceSoftwareVersion,
                                             Name = i.Device.Model,
                                             BrandId = i.Device.Brand.ID,
                                             BrandName = i.Device.Brand.Name,
                                             //Photo = i.Device.Photo.StartsWith("~") ? i.Device.Photo.Substring(1).Trim() : "/" + i.Device.Photo,
                                             Photo = i.Device.Photo,
                                             TestInstanceId = i.ID
                                         }).ToList());

                }
                #endregion
                #region extracting distinct phones for a All the phones extracted earlier
                List<phoneModels> distinctDtls = new List<phoneModels>();
                
                foreach (var detail in allDetails)
                {
                    //Int32 testIntsanceId = 0;
                    bool exists = false;
                    if (distinctDtls.Count == 0)
                    {
                        distinctDtls.Add(detail);
                        distinctDtls[0].PhoneSoftwareVersion = distinctDtls[0].TestInstanceId.ToString() + "|" + distinctDtls[0].PhoneSoftwareVersion;
                        //distinctDtls[0].Photo = distinctDtls[0].Photo + "?" + random.Next();
                    }
                    else
                    {
                        foreach (var dtl in distinctDtls)
                        {
                            if (dtl.PhoneID == detail.PhoneID)
                            {
                                exists = true;
                                break;
                            }
                            else
                            {
                                exists = false;
                            }
                        }
                        if (!exists)
                        {
                            distinctDtls.Add(detail);
                            distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion = distinctDtls[distinctDtls.Count - 1].TestInstanceId.ToString() + "|"
                                                                                            + distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion;
                            //distinctDtls[distinctDtls.Count - 1].Photo = distinctDtls[distinctDtls.Count - 1].Photo + "?" + random.Next();
                        }

                    }

                }
                distinctDtls = distinctDtls.OrderBy(dtl => dtl.BrandName).ThenBy(dtl => dtl.Name).ToList();
                #endregion

                #region New required Results
                var intTestInstanceIds = (from dtl in distinctDtls
                                                        select dtl.TestInstanceId);

                SqlCultureRepository cultRepo = new SqlCultureRepository();
                string locale = cultRepo.getCultureLocale(cultureId);

                //var finalCommntsNew = (from testInst in _entities.TestInstances.WhereIn(tIt => tIt.ID, intTestInstanceIds)
                //                       where testInst.ID == testInst.ID
                //                       from commt in testInst.Comments
                //                       from commtCult in commt.CommentCultureDetails
                //                       where commtCult.Culture.Locale == locale
                //                       select new
                //                       {
                //                           TestInstanceId = testInst.ID,
                //                           CommentText = commtCult.CommentText
                //                       }).ToList();

                var topfeatureResult = (from testInst in _entities.TestInstances.WhereIn(tIt => tIt.ID, intTestInstanceIds)
                                          from topFeatRs in _entities.TopFeatureResults
                                          where topFeatRs.TestInstance.ID == testInst.ID
                                          from cultureDetail in topFeatRs.TopFeature.TopFeatureCultureDetails
                                          where cultureDetail.CultureID == cultureId
                                          select new 
                                          {
                                              TestInstanceId = testInst.ID,
                                              Conclusion = testInst.TestInstanceConclusion.Conclusion,
                                              TopFeatureResultId = topFeatRs.ID,
                                              TopFeatureId = topFeatRs.TopFeature.ID,
                                              TopFeatureResult = topFeatRs.Result,
                                              TopFeatureName = cultureDetail.TopFeature,
                                              CommentText = (from commt in testInst.Comments
                                                             from commtCult in commt.CommentCultureDetails
                                                             where commtCult.Culture.Locale == locale
                                                             select commtCult.CommentText)
                                          });
                #endregion

                //topfeatureResult.RemoveAll(t => t.TestInstanceId > 1);
                #region Getting Brands for distinct Phones
                var brandIds = (from dtl in distinctDtls
                                orderby dtl.BrandId ascending
                                select dtl.BrandId).Distinct().ToList();

                List<phoneBrands> brands = new List<phoneBrands>();

                foreach (var brandId in brandIds)
                {
                    brands.Add(new phoneBrands(brandId, _entities.Brands.Where(brand => brand.ID == brandId).First().Name));
                }
                #endregion

                string stopfeatureResult = sr.Serialize(topfeatureResult);
                string sfinalComments = sr.Serialize(finalComments);
                string reqdBrands = sr.Serialize(brands);
                string reqdModels = sr.Serialize(from dst in distinctDtls
                                                 orderby dst.BrandName, dst.Name
                                                 select dst
                                                );
                
                reqdInfo = "{\"brands\":" + reqdBrands + ",\"models\":" + reqdModels + ",\"topFeatureResults\":" + stopfeatureResult + "}";

                return reqdInfo;

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                throw;
            }
        }
        #endregion

        #region get Comments For Culture
        /// <summary>
        /// Get Comments for a test instance
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with comments for each section</returns>
        public string getCommentsForCulture(int testInstanceId, int cultureId)
        {
            SqlCultureRepository cultRepo = new SqlCultureRepository();
            string locale = cultRepo.getCultureLocale(cultureId);
            var comments = from i in _entities.TestInstances
                           where i.ID == testInstanceId
                           from a in i.TestAnswers
                           from c in a.Comments
                           from ccd in c.CommentCultureDetails
                           where ccd.Culture.Locale == locale
                           select new
                           {
                               testInstanceId = testInstanceId,
                               c.Section.SectionNumber,
                               SectionName = c.Section.SectionName,
                               comment = ccd.CommentText
                           };

            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdCommnts = sr.Serialize(comments.OrderBy(c => c.SectionNumber).Distinct());
            return reqdCommnts;
        }
        #endregion

        #region get Final Comments For Culture
        /// <summary>
        /// Get Final Comments for a test instance
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with Final comments for a Test Instance</returns>
        public string getFinalCommentsForCulture(int testInstanceId, int cultureId)
        {
            SqlCultureRepository cultRepo = new SqlCultureRepository();
            string locale = cultRepo.getCultureLocale(cultureId); 
            List<string> finalCommnts = (from testInst in _entities.TestInstances
                                         where testInst.ID == testInstanceId
                                         from commt in testInst.Comments
                                         from commtCult in commt.CommentCultureDetails
                                         where commtCult.Culture.Locale == locale
                                         select commtCult.CommentText).ToList();

            string reqdCommnts = "";
            foreach (var comment in finalCommnts)
            {
                reqdCommnts = reqdCommnts + comment;
            }

            return reqdCommnts;
        }
        #endregion

        #region get Model For TestInstance
        public string getModelForTestInstance(Int32 testInstanceId)
        {
            List<phoneModels> allDetails = new List<phoneModels>();
            JavaScriptSerializer sr = new JavaScriptSerializer();

            allDetails.AddRange((from i in _entities.TestInstances
                                 where i.ID == testInstanceId
                                 select new phoneModels
                                 {
                                     RelatedSystemID = 0,
                                     PhoneID = i.Device.ID,
                                     PhoneSoftwareVersion = i.DeviceSoftwareVersion,
                                     Name = i.Device.Model,
                                     BrandId = i.Device.Brand.ID,
                                     BrandName = i.Device.Brand.Name,
                                     //Photo = i.Device.Photo.StartsWith("~") ? i.Device.Photo.Substring(1).Trim() : "/" + i.Device.Photo,
                                     Photo = i.Device.Photo,
                                     TestInstanceId = i.ID
                                 }).ToList());

            #region extracting distinct phones for a All the phones extracted earlier
            List<phoneModels> distinctDtls = new List<phoneModels>();

            foreach (var detail in allDetails)
            {
                //Int32 testIntsanceId = 0;
                bool exists = false;
                if (distinctDtls.Count == 0)
                {
                    distinctDtls.Add(detail);
                    distinctDtls[0].PhoneSoftwareVersion = distinctDtls[0].TestInstanceId.ToString() + "|" + distinctDtls[0].PhoneSoftwareVersion;
                    //distinctDtls[0].Photo = distinctDtls[0].Photo + "?" + random.Next();
                }
                else
                {
                    foreach (var dtl in distinctDtls)
                    {
                        if (dtl.PhoneID == detail.PhoneID)
                        {
                            exists = true;
                            break;
                        }
                        else
                        {
                            exists = false;
                        }
                    }
                    if (!exists)
                    {
                        distinctDtls.Add(detail);
                        distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion = distinctDtls[distinctDtls.Count - 1].TestInstanceId.ToString() + "|"
                                                                                        + distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion;
                        //distinctDtls[distinctDtls.Count - 1].Photo = distinctDtls[distinctDtls.Count - 1].Photo + "?" + random.Next();
                    }

                }

            }
            distinctDtls = distinctDtls.OrderBy(dtl => dtl.BrandName).ThenBy(dtl => dtl.Name).ToList();
            #endregion

            string reqdModels = sr.Serialize(distinctDtls);

            string reqdInfo = reqdModels;
            //string reqdInfo = "{\"models\":" + reqdModels + "}";

            return reqdInfo;
        }
        #endregion

        #region get Top Features Results ForCulture
        /// <summary>
        /// gets Results for Top Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the Top feature results for that instance</returns>
        public string getTopFRForCultureWthCmmntText(int testInstanceId, int cultureId)
        {

            string finalcomment = "";
            finalcomment = getFinalComments(testInstanceId);

            var details = from testInst in _entities.TestInstances
                          where testInst.ID == testInstanceId
                          from topFeatRs in _entities.TopFeatureResults
                          where topFeatRs.TestInstance.ID == testInstanceId
                          from cultureDetail in topFeatRs.TopFeature.TopFeatureCultureDetails
                          where cultureDetail.CultureID == cultureId
                          select new
                          {
                              Conclusion = testInst.TestInstanceConclusion.Conclusion,
                              TopFeatureResultId = topFeatRs.ID,
                              TopFeatureId = topFeatRs.TopFeature.ID,
                              TopFeatureResult = topFeatRs.Result,
                              TopFeatureName = cultureDetail.TopFeature,
                              CommentText = finalcomment
                          };

            //if (details.First().Conclusion.ToLower() == "not Recommended")
            //{
            //    finalcomment = getFinalComments(testInstanceId);
            //}

            JavaScriptSerializer sr = new JavaScriptSerializer();
            string reqdResutls = sr.Serialize(details);
            string reqdInfo = reqdResutls;
            return reqdResutls;

        }
        #endregion

        #region getDistinctPhoneDetailsAsPerDeviceType
        /// <summary>
        /// get Distinct Phone Details
        /// </summary>
        /// <param name="SystemIds"></param>
        /// <returns></returns>
        public string getDistinctPhoneDetailsAsPerDeviceType(string SystemIds)
        {
            try
            {

                string reqdInfo = "";
                List<TestScript> testScripts = new List<TestScript>();
                String[] systemIds = SystemIds.Split(',');
                JavaScriptSerializer sr = new JavaScriptSerializer();

                List<phoneModels> allDetails = new List<phoneModels>();
                List<topFeatureResults> topfeatureResult = new List<topFeatureResults>();
                List<finalComment> finalComments = new List<finalComment>();
                //Random random = new Random();
                #region getting all the phones for a System
                foreach (var systemId in systemIds)
                {
                    Int32 id = Convert.ToInt32(systemId);

                    allDetails.AddRange((from t in _entities.TestScripts
                                         where t.Device.ID == id
                                         from i in t.TestInstances
                                         where i.TestStatusID == 8 && i.Device.DeviceTypeID == 3
                                         orderby i.Device.ID, i.DeviceSoftwareVersion descending
                                         select new phoneModels
                                         {
                                             RelatedSystemID = id,
                                             PhoneID = i.Device.ID,
                                             PhoneSoftwareVersion = i.DeviceSoftwareVersion,
                                             Name = i.Device.Model,
                                             BrandId = i.Device.Brand.ID,
                                             BrandName = i.Device.Brand.Name,
                                             //Photo = i.Device.Photo.StartsWith("~") ? i.Device.Photo.Substring(1).Trim() : "/" + i.Device.Photo,
                                             Photo = i.Device.Photo,
                                             TestInstanceId = i.ID
                                         }).ToList());


                }
                #endregion
                #region extracting distinct phones for a All the phones extracted earlier
                List<phoneModels> distinctDtls = new List<phoneModels>();
                foreach (var detail in allDetails)
                {
                    //Int32 testIntsanceId = 0;
                    bool exists = false;
                    if (distinctDtls.Count == 0)
                    {
                        distinctDtls.Add(detail);
                        distinctDtls[0].PhoneSoftwareVersion = distinctDtls[0].TestInstanceId.ToString() + "|" + distinctDtls[0].PhoneSoftwareVersion;
                        //distinctDtls[0].Photo = distinctDtls[0].Photo + "?" + random.Next();
                    }
                    else
                    {
                        foreach (var dtl in distinctDtls)
                        {
                            if (dtl.PhoneID == detail.PhoneID)
                            {
                                exists = true;
                                break;
                            }
                            else
                            {
                                exists = false;
                            }
                        }
                        if (!exists)
                        {
                            distinctDtls.Add(detail);
                            distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion = distinctDtls[distinctDtls.Count - 1].TestInstanceId.ToString() + "|"
                                                                                            + distinctDtls[distinctDtls.Count - 1].PhoneSoftwareVersion;
                            //distinctDtls[distinctDtls.Count - 1].Photo = distinctDtls[distinctDtls.Count - 1].Photo + "?" + random.Next();
                        }

                    }

                }
                distinctDtls = distinctDtls.OrderBy(dtl => dtl.BrandName).ThenBy(dtl => dtl.Name).ToList();
                #endregion
                #region Getting Brands for distinct Phones
                var brandIds = (from dtl in distinctDtls
                                orderby dtl.BrandId ascending
                                select dtl.BrandId).Distinct().ToList();

                List<phoneBrands> brands = new List<phoneBrands>();

                foreach (var brandId in brandIds)
                {
                    brands.Add(new phoneBrands(brandId, _entities.Brands.Where(brand => brand.ID == brandId).First().Name));
                }
                #endregion
                #region reqd Results
                foreach (var detail in distinctDtls)
                {
                    #region final comment
                    List<string> finalCommnts = (from testInst in _entities.TestInstances
                                                 where testInst.ID == detail.TestInstanceId
                                                 from commt in testInst.Comments
                                                 from commtCult in commt.CommentCultureDetails
                                                 where commtCult.Culture.Locale == "English"
                                                 select commtCult.CommentText).ToList();

                    string reqdCommnts = "";
                    foreach (var comment in finalCommnts)
                    {
                        reqdCommnts = reqdCommnts + comment;
                    }

                    #endregion
                    #region top feaure results
                    topfeatureResult.AddRange(from testInst in _entities.TestInstances
                                              where testInst.ID == detail.TestInstanceId
                                              from topFeatRs in _entities.TopFeatureResults
                                              where topFeatRs.TestInstance.ID == detail.TestInstanceId
                                              select new topFeatureResults
                                              {
                                                  TestInstanceId = detail.TestInstanceId,
                                                  Conclusion = testInst.TestInstanceConclusion.Conclusion,
                                                  TopFeatureResultId = topFeatRs.ID,
                                                  TopFeatureId = topFeatRs.TopFeature.ID,
                                                  TopFeatureResult = topFeatRs.Result,
                                                  TopFeatureName = topFeatRs.TopFeature.Name,
                                                  CommentText = reqdCommnts
                                              });
                    #endregion
                }

                #endregion

                string stopfeatureResult = sr.Serialize(topfeatureResult);
                string sfinalComments = sr.Serialize(finalComments);
                string reqdBrands = sr.Serialize(brands);
                string reqdModels = sr.Serialize(from dst in distinctDtls
                                                 orderby dst.BrandName, dst.Name
                                                 select dst
                                                );
                //",\"finalComments\":" + sfinalComments +
                reqdInfo = "{\"brands\":" + reqdBrands + ",\"models\":" + reqdModels + ",\"topFeatureResults\":" + stopfeatureResult + "}";

                return reqdInfo;

            }
            catch (Exception ex)
            {
                string m = ex.Message;
                throw;
            }
        }
        #endregion

        
    }

    public class phoneModels
    {
        public int RelatedSystemID { get; set; }
        public int PhoneID { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Photo { get; set; }
        public int TestInstanceId { get; set; }
        public string PhoneSoftwareVersion { get; set; }

    }

    public class phoneBrands
    {
        public phoneBrands(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class topFeatureResults
    {
        public topFeatureResults()
        {

        }
        public topFeatureResults(string conclusion, int topFeatureResultId, int topFeatureId, string topFeatureResult, string topFeatureName, int testInstanceId)
        {
            TopFeatureResultId = topFeatureResultId;
            TopFeatureId = topFeatureId;
            TopFeatureResult = topFeatureResult;
            TopFeatureName = topFeatureName;
            Conclusion = conclusion;
            TestInstanceId = testInstanceId;
        }
        public int TestInstanceId { get; set; }
        public int TopFeatureResultId { get; set; }
        public string TopFeatureName { get; set; }
        public int TopFeatureId { get; set; }
        public string TopFeatureResult { get; set; }
        public string Conclusion { get; set; }
        public string CommentText { get; set; }

    }


    public class topFeatureResultsCt
    {
        public topFeatureResultsCt()
        {

        }
        public topFeatureResultsCt(string conclusion, int topFeatureResultId, int topFeatureId, string topFeatureResult, string topFeatureName, int testInstanceId)
        {
            TopFeatureResultId = topFeatureResultId;
            TopFeatureId = topFeatureId;
            TopFeatureResult = topFeatureResult;
            TopFeatureName = topFeatureName;
            Conclusion = conclusion;
            TestInstanceId = testInstanceId;
        }
        public int TestInstanceId { get; set; }
        public int TopFeatureResultId { get; set; }
        public string TopFeatureName { get; set; }
        public int TopFeatureId { get; set; }
        public string TopFeatureResult { get; set; }
        public string Conclusion { get; set; }
        public IEnumerable<string> CommentText { get; set; }

    }

    public class topFeatureResultsWithRating
    {
        public topFeatureResultsWithRating()
        {

        }
        public topFeatureResultsWithRating(int topFeatureResultId, int topFeatureId, string topFeatureResult, string topFeatureName, int testInstanceId, string starRating)
        {
            TopFeatureResultId = topFeatureResultId;
            TopFeatureId = topFeatureId;
            TopFeatureResult = topFeatureResult;
            TopFeatureName = topFeatureName;
            TestInstanceId = testInstanceId;
            StarRating = starRating;
        }
        public int TestInstanceId { get; set; }
        public int TopFeatureResultId { get; set; }
        public string TopFeatureName { get; set; }
        public int TopFeatureId { get; set; }
        public string TopFeatureResult { get; set; }
        public string StarRating { get; set; }
        public string CommentText { get; set; }

    }

    public class mainFeatureResults
    {
        public int TestInstanceId { get; set; }
        public string SectionName { get; set; }
        public int? Passed { get; set; }
        public int SectionId { get; set; }
        public string FeatureName { get; set; }
        public int SectionNumber { get; set; }
        public int? OrderNumber { get; set; }
    }

    public class finalComment
    {
        public int TestInstanceId { get; set; }
        public string CommentText { get; set; }
    }

    public class conclOfTopFtrReslts
    {
        public conclOfTopFtrReslts(string conclusion)
        {
            Conclusion = conclusion;
        }
        public string Conclusion { get; set; }

    }

}

namespace wLinqExtensions
{
    public static class generalLinqExtensions
    {
        public static IQueryable<T> WhereIn<T, TValue>(this IQueryable<T> source, Expression<Func<T, TValue>> propertySelector, params TValue[] values)
        {
            return source.Where(GetWhereInExpression(propertySelector, values));
        }

        public static IQueryable<T> WhereIn<T, TValue>(this IQueryable<T> source, Expression<Func<T, TValue>> propertySelector, IEnumerable<TValue> values)
        {
            return source.Where(GetWhereInExpression(propertySelector, values));
        }

        private static Expression<Func<T, bool>> GetWhereInExpression<T, TValue>(Expression<Func<T, TValue>> propertySelector, IEnumerable<TValue> values)
        {
            ParameterExpression p = propertySelector.Parameters.Single();
            if (!values.Any())
                return e => false;

            var equals = values.Select(value => (Expression)Expression.Equal(propertySelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));

            return Expression.Lambda<Func<T, bool>>(body, p);
        }

    }
}