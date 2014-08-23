using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;
using System.Web.Script.Serialization;
using CwiAPI.Entities;

namespace CwiAPI.Concrete
{

    public class SqlCultureRepository : EntityContainer, ICultureRepository
    {

        public string getCultureId(string languageCode)
        {
            var cultureIds = (from cult in _entities.Cultures
                               where cult.LanguageCode == languageCode
                               select cult.ID).ToList();
            Int32 cultureId;
            
            if (cultureIds.Count == 0 )
            {
                cultureId = 23;
            }
            else
            {
                cultureId = cultureIds.First();
            }

            cultureDetails data = new cultureDetails();
            data.cultureId = cultureId;
            JavaScriptSerializer sr = new JavaScriptSerializer();
            string requiredInfo = sr.Serialize(data);
            return requiredInfo;

        }

        public string getCultureLocale(Int32 cultureId)
        {
            var cultureLocales = (from cult in _entities.Cultures
                              where cult.ID == cultureId
                              select cult.Locale).ToList();
            string cultureLocale;

            if (cultureLocales.Count == 0)
            {
                cultureLocale = "English";
            }
            else
            {
                cultureLocale = cultureLocales.First();
            }

            //cultureLocale data = new cultureLocale();
            //data.cultureLocale = cultureLocale;
            //JavaScriptSerializer sr = new JavaScriptSerializer();
            //string requiredInfo = sr.Serialize(data);
            return cultureLocale;

        }
    }


    public class cultureDetails
    {
        public Int32 cultureId { get; set; }
    }

    public class cultureLocale
    {
        public string Locale { get; set; }

    }
}