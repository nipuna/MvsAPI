using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;
using System.Web.Script.Serialization;
using CwiAPI.Entities;

namespace CwiAPI.Concrete
{
    public class VehicleSystem : EntityContainer, ISystems 
    {
        //public Int32 modelId, CountryId;
        //public string ModelYear;
        public Int32 ModelId { get; set; }
        public string ModelYear { get; set; }
        public Int32 CountryId { get; set; }

        public string getSystem()
        {
            string requiredInfo = "";
            string yearSt = "";
            if (ModelYear.Trim().Length > 4)
            {
                yearSt = ModelYear.Substring(0, 4);
            }
            else
            {
                yearSt = ModelYear;
            }
            IQueryable<Int32> deviceIds = ((from cwirange in _entities.CWIRanges
                                            where cwirange.RangeStart == yearSt && cwirange.Vehicle.ID == ModelId
                                  select cwirange.Device.ID).Distinct());

            List<systemDetails> systems = new List<systemDetails>();
            foreach (var deviceId in deviceIds)
            {
                var vSystem = _entities.Devices.Where(pDevice => pDevice.ID == deviceId && pDevice.Country.ID == CountryId);
                if (vSystem.Count() > 0)
                {
                    systems.Add(new systemDetails(vSystem.First().ID));
                }
            }

            if (systems != null)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                requiredInfo = sr.Serialize(systems);
            }
            return requiredInfo;
        }


        public string getSystemsForModel()
        {
            string requiredInfo = "";
            string yearSt = "";
            if (ModelYear.Trim().Length > 4)
            {
                yearSt = ModelYear.Substring(0, 4);
            }
            else
            {
                yearSt = ModelYear;
            }
            IQueryable<systemDetails> systems = (from cwirange in _entities.CWIRanges
                                                 where cwirange.RangeStart == yearSt && cwirange.Vehicle.ID == ModelId
                                                 select new systemDetails
                                                 {
                                                     Id = cwirange.Device.ID
                                                 }).Distinct();

            if (systems != null)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                requiredInfo = sr.Serialize(systems);
            }
            return requiredInfo;
        }

        public string getSystemForVehicle(Int32 vehicleid)
        {
            string requiredInfo = "";
            IQueryable<Int32> deviceIds = ((from cwirange in _entities.CWIRanges
                                            where cwirange.Vehicle.ID == vehicleid
                                            select cwirange.Device.ID).Distinct());
            List<systemDetailsWithNames> systems = new List<systemDetailsWithNames>();
            foreach (var deviceId in deviceIds)
            {
                var vSystem = _entities.Devices.Where(pDevice => pDevice.ID == deviceId);
                if (vSystem.Count() > 0)
                {
                    systems.Add(new systemDetailsWithNames(vSystem.First().ID, vSystem.First().Model));
                }
            }

            if (systems != null)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                requiredInfo = sr.Serialize(systems);
            }
            return requiredInfo;
        }

        public string getSystemForBrand(Int32 brandid)
        {
            string requiredInfo = "";
            IQueryable<Int32> deviceIds = ((from cwirange in _entities.CWIRanges
                                            where cwirange.Device.Brand.ID == brandid
                                            select cwirange.Device.ID).Distinct());
            //IQueryable<Int32> deviceIds = ((from device in _entities.Devices
            //                                where device.Brand.ID == brandid
            //                                select device.ID).Distinct());
            List<systemDetailsWithNames> systems = new List<systemDetailsWithNames>();
            foreach (var deviceId in deviceIds)
            {
                var vSystem = _entities.Devices.Where(pDevice => pDevice.ID == deviceId);
                if (vSystem.Count() > 0)
                {
                    systems.Add(new systemDetailsWithNames(vSystem.First().ID, vSystem.First().Model));
                }
            }

            if (systems != null)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                requiredInfo = sr.Serialize(systems);
            }
            return requiredInfo;
        }

    }


    public class systemDetails
    {
        public systemDetails(int id)
        {
            Id = id;
        }

        public systemDetails()
        {
            // TODO: Complete member initialization
        }
        public int Id { get; set; }

    }


    public class systemDetailsWithNames
    {
        public systemDetailsWithNames(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

    }

}