using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;

namespace CwiAPI.Concrete
{
    public class VehicleSystemState 
    {
        private Int32 modelId, countryId;
        private string modelYear;

        public int ModelId
        {
            get
            {
                return modelId;
            }
            set
            {
                modelId = value;
            }
        }

        public string ModelYear
        {
            get
            {
                return modelYear;
            }
            set
            {
                modelYear = value;
            }
        }

        public int CountryId
        {
            get
            {
                return countryId;
            }
            set
            {
                countryId = value;
            }
        }

        //public int vinId
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

       
    }
}