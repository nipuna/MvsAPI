using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;
using System.Web.Script.Serialization;
using CwiAPI.Entities;

namespace CwiAPI.Concrete
{

    public class SqlBrandRepository : EntityContainer, IBrandRepository
    {

        public string getCustomerIdForBrand(string brandName)
        {
            var customerIds = (from bnd in _entities.Brands
                               where bnd.Name == brandName
                               from cust in bnd.Customers
                               select cust.ID).ToList();
            if (customerIds.Count() > 0)
            {
                brandDetails data = new brandDetails();
                data.customerId = customerIds.First();
                JavaScriptSerializer sr = new JavaScriptSerializer();
                string requiredInfo = sr.Serialize(data);
                string requiredInfoNew = sr.Serialize(customerIds);
                return requiredInfoNew;
            }
            return "";
        }
    }

    public class brandDetails
    {
        public Int32 customerId { get; set; }
    }

}