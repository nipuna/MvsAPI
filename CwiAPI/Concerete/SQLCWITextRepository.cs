using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;
using System.Web.Script.Serialization;
using CwiAPI.Entities;

namespace CwiAPI.Concrete
{
    public class SQLCWITextRepository : EntityContainer, ICWITextRepository
    {

        public string getCWIText(int customerId, string brandName, Int32 cultureId)
        {
            
            var cwitext = (from cust in _entities.Customers
                           where cust.ID == customerId
                           from brand in cust.Brands
                           where brand.Name == brandName
                           from brandCwi in brand.CWITexts
                           from brandCwiCulDt in brandCwi.CWITextCultureDetails
                           where brandCwiCulDt.Culture.ID == cultureId
                           select new
                           {
                               CwiTextId = brandCwi.ID,
                               CultureId = cultureId,
                               Cwitext = brandCwiCulDt.CwiText
                           });
            JavaScriptSerializer sr = new JavaScriptSerializer();
            string requiredInfo = sr.Serialize(cwitext);
            return requiredInfo;

        }

        public string getCWITextForBrand(int brandId, Int32 cultureId)
        {

            var cwitext = (from bnd in _entities.Brands
                           where bnd.ID == brandId
                           from brandCwi in bnd.CWITexts
                           from brandCwiCulDt in brandCwi.CWITextCultureDetails
                           where brandCwiCulDt.Culture.ID == cultureId
                           select new
                           {
                               CwiTextId = brandCwi.ID,
                               CultureId = cultureId,
                               Cwitext = brandCwiCulDt.CwiText
                           });
            JavaScriptSerializer sr = new JavaScriptSerializer();
            string requiredInfo = sr.Serialize(cwitext);
            return requiredInfo;

        }
    }
}