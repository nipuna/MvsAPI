using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CwiAPI.Concrete;
using CwiAPI.Abstract;

namespace CwiAPI
{
    /// <summary>
    /// Summary description for CwiAPIService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CwiAPIService : System.Web.Services.WebService
    {

        #region authenticates the Request and returns the Customer Identifiction no for the User
        /// <summary>
        /// authenticates the Request
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">password</param>
        /// <returns>Jquery string with authentication status and the customerID</returns>
        [WebMethod]
        public string authenticateRequest(string username, string password)
        {
            SQLUserRepository userRepo = new SQLUserRepository();
            string reqdInfo = userRepo.authenticateRequest(username, password);
            return reqdInfo;
        }
        #endregion

        #region authenticates the Request and returns the Brand Identifiction no for the User
        /// <summary>
        /// authenticates the Request
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">password</param>
        /// <returns>Jquery string with authentication status and the brandID</returns>
        [WebMethod]
        public string authenticateRequestWithBrand(string username, string password)
        {
            SQLUserRepository userRepo = new SQLUserRepository();
            string reqdInfo = userRepo.authenticateRequestWithBrand(username, password);
            return reqdInfo;
        }
        #endregion

        #region authenticates the Request and returns the Customer Identifiction no for the User
        /// <summary>
        /// authenticates the Request
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">password</param>
        /// <returns>Jquery string with authentication status and the customerID</returns>
        [WebMethod]
        public string authenticateRequestAllCustomers(string username, string password)
        {
            SQLUserRepository userRepo = new SQLUserRepository();
            string reqdInfo = userRepo.authenticateRequestAllCustomers(username, password);
            return reqdInfo;
        }
        #endregion

        #region gets Customer ID for a brand name
        /// <summary>
        /// gets Customer ID for a brand name
        /// </summary>
        /// <param name="brandName"></param>
        /// <returns></returns>
        [WebMethod]
        public string getCustomerIdForBrand(string brandName)
        {
            SqlBrandRepository brandRepo = new SqlBrandRepository();
            string reqdInfo = brandRepo.getCustomerIdForBrand(brandName);
            return reqdInfo;
        }
        #endregion

        #region gets All Regions
        /// <summary>
        /// gets regions for the Customer
        /// </summary>
        /// <param name="customerId">id of the customer for which regions are required</param>
        /// <returns>JQuery string with region details for a customer </returns>
        [WebMethod]
        public string getAllRegions(int customerId)
        {
            SQLRegionRepository regRepo = new SQLRegionRepository();
            string reqdInfo = regRepo.getAllRegions(customerId);
            return reqdInfo;
        }
        #endregion

        #region getAllVehicleDetails
        /// <summary>
        ///  gets details for vehicles associated with a cutomer
        /// </summary>
        /// <param name="customerId">id of the customer for which regions are required</param>
        /// <returns>JQuery string with vehicle details for a customer </returns>
        [WebMethod]
        public string getAllVehicleDetails(int customerId)
        {
            SQLVehicleRepository vehRepo = new SQLVehicleRepository();
            string reqdInfo = vehRepo.getAllVehicleDetails(customerId);
            return reqdInfo;
        }
        #endregion

        #region getAllVehicleDetails
        /// <summary>
        ///  gets details for vehicles associated with a cutomer
        /// </summary>
        /// <param name="customerId">id of the customer for which regions are required</param>
        /// <returns>JQuery string with vehicle details for a customer </returns>
        [WebMethod]
        public string getAllVehicleDetailsForBrand(int customerId, string brandName)
        {
            SQLVehicleRepository vehRepo = new SQLVehicleRepository();
            string reqdInfo = vehRepo.getAllVehicleDetails(customerId, brandName);
            return reqdInfo;
        }
        #endregion

        #region getModelYears
        /// <summary>
        ///  gets years associated wwith a model
        /// </summary>
        /// <param name="customerId">id of the model for which years are required</param>
        /// <returns>JQuery string with model years </returns>
        [WebMethod]
        public string getModelYears(int modelId)
        {
            SQLVehicleRepository vehRepo = new SQLVehicleRepository();
            string reqdInfo = vehRepo.getModelYears(modelId);
            return reqdInfo;
        }
        #endregion

        #region getModelYearsWithCustomization
        /// <summary>
        ///  gets years associated wwith a model
        /// </summary>
        /// <param name="customerId">id of the model for which years are required</param>
        /// <returns>JQuery string with model years </returns>
        [WebMethod]
        public string getModelYearsWithCustomization(int modelId)
        {
            SQLVehicleRepository vehRepo = new SQLVehicleRepository();
            string reqdInfo = vehRepo.getModelYearsWithCustomization(modelId);
            return reqdInfo;
        }
        #endregion

        #region getAllVehicleDetailsWithBrand

        [WebMethod]
        public string getAllVehicleDetailsWithBrand(int brandId)
        {
            SQLVehicleRepository vehRepo = new SQLVehicleRepository();
            return vehRepo.getAllVehicleDetailsWithBrand(brandId);
        }
        #endregion

        [WebMethod]
        public string getSystem(Int32 modelId, string modelYear, Int32 regionId)
        {
            VehicleSystem vehicleSystem = new VehicleSystem();

            vehicleSystem.ModelId = modelId;
            vehicleSystem.ModelYear = modelYear;
            vehicleSystem.CountryId = regionId;
            string reqdInfo = vehicleSystem.getSystem();
            return reqdInfo;
        }

        [WebMethod]
        public string getSystemsForModel(Int32 modelId, string modelYear)
        {
            VehicleSystem vehicleSystem = new VehicleSystem();

            vehicleSystem.ModelId = modelId;
            vehicleSystem.ModelYear = modelYear;
            string reqdInfo = vehicleSystem.getSystemsForModel();
            return reqdInfo;
        }

        #region getAllPhoneDetails
        /// <summary>
        /// gets Phone Brands and Models compatible for the system Ids passed 
        /// </summary>
        /// <param name="testInstanceId">systemids in a string seperated with ',' </param>
        /// <returns>Jquery string with all the phones tested against particular System</returns>
        [WebMethod]
        public string getAllPhoneDetails(string SystemIds)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getAllPhoneDetails(SystemIds);
            return reqdInfo;
        }
        #endregion

        #region gets Results for Top Features
        /// <summary>
        /// gets Results for Top Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the Top feature results for that instance</returns>
        [WebMethod]
        public string getTopFeatureResults(int testInstanceId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getTopFeatureResults(testInstanceId);
            return reqdInfo;
        }
        #endregion

        #region getMainFeatureResults
        /// <summary>
        /// gets Results for Main Feature 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the main feature results for that instance</returns>
        [WebMethod]
        public string getMainFeatureResults(int testInstanceId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getMainFeatureResults(testInstanceId);
            return reqdInfo;
        }
        #endregion

        #region getAdditionalFeatureResults
        /// <summary>
        /// gets Results for additional Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the additional feature results for that instance</returns>
        [WebMethod]
        public string getAdditionalFeatureResults(int testInstanceId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getAdditionalFeatureResults(testInstanceId);
            return reqdInfo;
        }
        #endregion

        #region getDistinctPhoneDetails
        /// <summary>
        /// gets Phone Brands and Models compatible for the system Ids passed 
        /// </summary>
        /// <param name="testInstanceId">systemids in a string seperated with ',' </param>
        /// <returns>Jquery string with all the phones tested against particular System</returns>
        [WebMethod]
        public string getDistinctPhoneDetails(string SystemIds)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getDistinctPhoneDetails(SystemIds);
            return reqdInfo;
        }
        #endregion

        #region getDistinctPhoneDetailsWithRating
        /// <summary>
        /// gets Phone Brands and Models compatible for the system Ids passed 
        /// </summary>
        /// <param name="testInstanceId">systemids in a string seperated with ',' </param>
        /// <returns>Jquery string with all the phones tested against particular System</returns>
        [WebMethod]
        public string getDistinctPhoneDetailsWithRating(string SystemIds)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();
            string reqdInfo = phoneRepo.getDistinctPhoneDetailsWithRating(SystemIds);
            return reqdInfo;
        }
        #endregion

        #region get Comments
        /// <summary>
        /// Get Comments for a test instance
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with comments for each section</returns>
        [WebMethod]
        public string getComments(int testInstanceId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getComments(testInstanceId);
            return reqdInfo;
        }
        #endregion

        #region get Final Comments
        /// <summary>
        /// Get Final Comments for a test instance
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with Final comments for a Test Instance</returns>
        [WebMethod]
        public string getFinalComments(int testInstanceId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getFinalComments(testInstanceId);
            return reqdInfo;
        }
        #endregion

        #region get Quick Guides
        /// <summary>
        /// Get Quick Guides for a test instance
        /// </summary>
        /// <param name="testInstanceId">Phone ID</param>
        /// <returns>Jquery string with Quick Guides for a Device</returns>
        [WebMethod]
        public string getQuickGuides(int SystemId, int PhoneId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();
            string reqdInfo = phoneRepo.getQuickGuides(SystemId, PhoneId);
            return reqdInfo;
        }
        #endregion

        #region gets Results for Top Features
        /// <summary>
        /// gets Results for Top Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the Top feature results for that instance</returns>
        [WebMethod]
        public string getTopFeatureResultsForCulture(int testInstanceId, int cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getTopFeatureResultsForCulture(testInstanceId, cultureId);
            return reqdInfo;
        }
        #endregion

        #region getMainFeatureResults
        /// <summary>
        /// gets Results for Main Feature 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the main feature results for that instance</returns>
        [WebMethod]
        public string getMainFeatureResultsForCulture(int testInstanceId, int cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getMainFeatureResultsForCulture(testInstanceId, cultureId);
            return reqdInfo;
        }
        #endregion

        #region getAdditionalFeatureResults
        /// <summary>
        /// gets Results for additional Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the additional feature results for that instance</returns>
        [WebMethod]
        public string getAdditionalFeatureResultsForCulture(int testInstanceId, int cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getAdditionalFeatureResultsForCulture(testInstanceId, cultureId);
            return reqdInfo;
        }
        #endregion

        #region get Quick Guides
        /// <summary>
        /// Get Quick Guides for a test instance
        /// </summary>
        /// <param name="testInstanceId">Phone ID</param>
        /// <returns>Jquery string with Quick Guides for a Device</returns>
        [WebMethod]
        public string getQuickGuidesForCulture(int SystemId, int PhoneID, int cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();
            string reqdInfo = phoneRepo.getQuickGuidesForCulture(SystemId, PhoneID, cultureId);
            return reqdInfo;
        }
        #endregion

        [WebMethod]
        public string getCultureId(string languageCode)
        {
            SqlCultureRepository cultRepo = new SqlCultureRepository();
            string reqdInfo = cultRepo.getCultureId(languageCode);
            return reqdInfo;
        }

        #region getDistinctPhoneDetailsForCulture
        /// <summary>
        /// gets Phone Brands and Models compatible for the system Ids passed 
        /// </summary>
        /// <param name="testInstanceId">systemids in a string seperated with ',' </param>
        /// <returns>Jquery string with all the phones tested against particular System</returns>
        [WebMethod]
        public string getDistinctPhoneDetailsForCulture(string SystemIds, Int32 cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();
            string reqdInfo = phoneRepo.getDistinctPhoneDetailsForCulture(SystemIds, cultureId);
            return reqdInfo;
        }
        #endregion

        #region getDistinctPhoneDetailsAsPerDeviceType
        /// <summary>
        /// gets Phone Brands and Models compatible for the system Ids passed 
        /// </summary>
        /// <param name="testInstanceId">systemids in a string seperated with ',' </param>
        /// <returns>Jquery string with all the phones tested against particular System</returns>
        [WebMethod]
        public string getDistinctPhoneDetailsAsPerDeviceType(string SystemIds, Int32 cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();
            string reqdInfo = phoneRepo.getDistinctPhoneDetailsAsPerDeviceType(SystemIds, cultureId);
            return reqdInfo;
        }
        #endregion

        #region get Comments For Culture
        /// <summary>
        /// Get Comments for a test instance
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with comments for each section</returns>
        [WebMethod]
        public string getCommentsForCulture(int testInstanceId, int cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();
            string reqdInfo = phoneRepo.getCommentsForCulture(testInstanceId, cultureId);
            return reqdInfo;
        }
        #endregion

        #region get Final Comments For Culture
        /// <summary>
        /// Get Final Comments for a test instance
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with Final comments for a Test Instance</returns>
        [WebMethod]
        public string getFinalCommentsForCulture(int testInstanceId, int cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getFinalCommentsForCulture(testInstanceId, cultureId);
            return reqdInfo;
        }
        #endregion

        [WebMethod]
        public string getCWIText(int customerId, string brandName, Int32 cultureId)
        {
            SQLCWITextRepository cwiTextRepo = new SQLCWITextRepository();
            string reqdInfo = cwiTextRepo.getCWIText(customerId, brandName, cultureId);
            return reqdInfo;
        }

        #region
        [WebMethod]
        public string getCWITextForBrand(int brandId, Int32 cultureId)
        {
            SQLCWITextRepository cwiTextRepo = new SQLCWITextRepository();
            string reqdInfo = cwiTextRepo.getCWITextForBrand(brandId, cultureId);
            return reqdInfo;
        }

        #endregion

        #region get System For Vehicle
        /// <summary>
        /// Get Systems based uplon vehicle Id
        /// </summary>
        /// <param name="testInstanceId">Vehicle ID</param>
        /// <returns>Jquery string with systems for a vehicle</returns>
        [WebMethod]
        public string getSystemForVehicle(Int32 vehicleId)
        {
            VehicleSystem vehicleSystem = new VehicleSystem();
            string reqdInfo = vehicleSystem.getSystemForVehicle(vehicleId);
            return reqdInfo;
        }

        #endregion

        #region get System For Brand
        /// <summary>
        /// Get Systems based uplon vehicle Id
        /// </summary>
        /// <param name="testInstanceId">Vehicle ID</param>
        /// <returns>Jquery string with systems for a vehicle</returns>
        [WebMethod]
        public string getSystemForBrand(Int32 brandid)
        {
            VehicleSystem vehicleSystem = new VehicleSystem();
            string reqdInfo = vehicleSystem.getSystemForBrand(brandid);
            return reqdInfo;
        }

        #endregion

        #region
        /// <summary>
        /// Get Devide based uplon vehicle Id
        /// </summary>
        /// <param name="testInstanceId">Vehicle ID</param>
        /// <returns>Jquery string with systems for a vehicle</returns>
        [WebMethod]
        public string getModelForTestInstance(Int32 testInstanceId)
        {
            SQLPhoneRepository phRepo = new SQLPhoneRepository();
            string reqdInfo = phRepo.getModelForTestInstance(testInstanceId);
            return reqdInfo;
        }

        #endregion

        #region gets Results for Top Features
        /// <summary>
        /// gets Results for Top Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the Top feature results for that instance</returns>
        [WebMethod]
        public string getTopFRForCultureWthCmmntText(int testInstanceId, int cultureId)
        {
            SQLPhoneRepository phoneRepo = new SQLPhoneRepository();

            string reqdInfo = phoneRepo.getTopFRForCultureWthCmmntText(testInstanceId, cultureId);
            return reqdInfo;
        }
        #endregion

        #region gets Results for Top Features
        /// <summary>
        /// gets Results for Top Features 
        /// </summary>
        /// <param name="testInstanceId">Test Instance ID</param>
        /// <returns>Jquery string with all the Top feature results for that instance</returns>
        [WebMethod]
        public void InsertLogDetails(Int32 VehicleID, Int32 PageID, DateTime dateAndTime, Int32 cultureId, Int32 testInstanceID )
        {
            SQLLoggingRepository logRepo = new SQLLoggingRepository();
            logRepo.InsertLogDetails(VehicleID, PageID, dateAndTime, cultureId, testInstanceID);
        }
        #endregion

    }
}
