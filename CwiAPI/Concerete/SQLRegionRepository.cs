using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;
using System.Web.Script.Serialization;
using CwiAPI.Entities;

namespace CwiAPI.Concrete
{
    public class SQLRegionRepository : EntityContainer , IRegionRepository
    {

        #region gets All Regions
        /// <summary>
        /// gets regions for the Customer
        /// </summary>
        /// <param name="customerId">id of the customer for which regions are required</param>
        /// <returns>JQuery string with region details for a customer </returns>
        public string getAllRegions(int customerId)
        {
            string requiredInfo = "";
            
            var regions = (from customer in _entities.Customers
                           where customer.ID == customerId
                           from b in customer.Brands
                           from r in b.Regions
                           select r).ToList();
            //Customer.Regions.Load();
            List<Region> lRegions = regions;
            List<region> regionsReqd = new List<region>();
            regionsReqd.Add(new region(0, "Please Select"));
            foreach (var _region in regions)
            {
                regionsReqd.Add(new region(_region.ID, _region.Name));
            }
            JavaScriptSerializer sr = new JavaScriptSerializer();
            requiredInfo = sr.Serialize(regionsReqd);
            return requiredInfo;
        }
        #endregion

    }

    public class region
    {
        public region(int RegionId, string RegionName)
        {
            regionId = RegionId;
            regionName = RegionName;

        }
        public int regionId { get; set; }
        public string regionName { get; set; }
    }
}