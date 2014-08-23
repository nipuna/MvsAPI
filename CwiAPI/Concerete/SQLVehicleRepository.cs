using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;
using System.Web.Script.Serialization;
using CwiAPI.Entities;

namespace CwiAPI.Concrete
{
    public class SQLVehicleRepository: EntityContainer , IVehicleRepository
    {

        #region getAllVehicleDetails
        /// <summary>
        ///  gets details for vehicles associated with a cutomer
        /// </summary>
        /// <param name="customerId">id of the customer for which regions are required</param>
        /// <returns>JQuery string with vehicle details for a customer </returns>
        public string getAllVehicleDetails(int customerId)
        {
            string requiredInfo = "";

            var Customer = (from customer in _entities.Customers
                            where customer.ID == customerId
                            select customer).First();
            Customer.Brands.Load();
            List<Brand> brands = Customer.Brands.ToList();
            List<vehiclesForBrand> vehicles = new List<vehiclesForBrand>();
            vehicles.Add(new vehiclesForBrand(0, "Please Select"));
            foreach (var brand in brands)
            {
                brand.Vehicles.Load();
                List<Vehicle> allVehicles = brand.Vehicles.ToList();
                foreach (var vehicle in allVehicles)
                {
                    vehicles.Add(new vehiclesForBrand(vehicle.ID, vehicle.Model));
                }
            }

            if (vehicles != null)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                requiredInfo = sr.Serialize(vehicles);
            }
            return requiredInfo;
        }
        #endregion

        #region getAllVehicleDetails
        /// <summary>
        ///  gets details for vehicles associated with a cutomer
        /// </summary>
        /// <param name="customerId">id of the customer for which regions are required</param>
        /// <returns>JQuery string with vehicle details for a customer </returns>
        public string getAllVehicleDetails(int customerId, string brandName)
        {
            string requiredInfo = "";

            var Customer = (from customer in _entities.Customers
                            where customer.ID == customerId
                            select customer).First();
            Customer.Brands.Load();
            List<Brand> brands = Customer.Brands.ToList();
            List<vehiclesForBrand> vehicles = new List<vehiclesForBrand>();
            vehicles.Add(new vehiclesForBrand(0, "Please Select"));
            foreach (var brand in brands)
            {
                if (brand.Name.ToLower() == brandName.ToLower())
                {
                    brand.Vehicles.Load();
                    List<Vehicle> allVehicles = brand.Vehicles.ToList();
                    foreach (var vehicle in allVehicles)
                    {
                        var cwiR = (from cwi in _entities.CWIRanges
                                    where cwi.Vehicle.ID == vehicle.ID
                                   select cwi).ToList();
                        if (cwiR.Count > 0)
                        {
                            vehicles.Add(new vehiclesForBrand(vehicle.ID, vehicle.Model));
                        }
                    }
                }
            }

            if (vehicles != null)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                requiredInfo = sr.Serialize(vehicles);
            }
            return requiredInfo;
        }
        #endregion

        #region getModelYears
        /// <summary>
        ///  gets years associated wwith a model
        /// </summary>
        /// <param name="customerId">id of the model for which years are required</param>
        /// <returns>JQuery string with model years </returns>
        public string getModelYears(int modelId )
        {
            string requiredInfo;
            List<vehicleYears> Years = new List<vehicleYears>();

            var ranges = (from range in _entities.CWIRanges
                          where range.Vehicle.ID == modelId
                          select range.RangeStart).Distinct();

            JavaScriptSerializer sr = new JavaScriptSerializer();
            requiredInfo = sr.Serialize(ranges);
            return requiredInfo;
        }
        #endregion

        #region getModelYearsWithCustomization
        /// <summary>
        ///  gets years associated wwith a model
        /// </summary>
        /// <param name="customerId">id of the model for which years are required</param>
        /// <returns>JQuery string with model years </returns>
        public string getModelYearsWithCustomization(int modelId)
        {
            string requiredInfo;
            List<vehicleYears> Years = new List<vehicleYears>();

            var ranges = (from range in _entities.CWIRanges
                          where range.Vehicle.ID == modelId
                          select range.RangeStart + " onwards").Distinct();

            JavaScriptSerializer sr = new JavaScriptSerializer();
            requiredInfo = sr.Serialize(ranges);
            return requiredInfo;
        }
        #endregion

        #region getAllVehicleDetailsWithBrand
        /// <summary>
        ///  gets details for vehicles associated with a brand
        /// </summary>
        /// <param name="brandId">id of the brand for which regions are required</param>
        /// <returns>JQuery string with vehicle details for a brand </returns>
        public string getAllVehicleDetailsWithBrand(int brandId)
        {
            string requiredInfo = "";

            var Brand = (from brand in _entities.Brands
                            where brand.ID == brandId
                            select brand).First();
            Brand.Vehicles.Load();
            List<Vehicle> allVehicles = Brand.Vehicles.ToList();
            List<vehiclesForBrand> vehicles = new List<vehiclesForBrand>();
            vehicles.Add(new vehiclesForBrand(0, "Please Select"));
            foreach (var vehicle in allVehicles)
                    {
                        var cwiR = (from cwi in _entities.Vehicles
                                    where cwi.Model == vehicle.Model
                                    select cwi).ToList();
                        if (cwiR.Count > 0)
                        {
                            vehicles.Add(new vehiclesForBrand(vehicle.ID, vehicle.Model));
                        }
                    }

            if (vehicles != null)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                requiredInfo = sr.Serialize(vehicles);
            }
            return requiredInfo;
        }
        #endregion

    }

    public class vehiclesForBrand
    {
        public vehiclesForBrand(int id , string name)
        {
            Id = id;
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class vehicleYears
    {
        public vehicleYears(int Range)
        {
            range = Range;
        }
        public int range { get; set; }
        //public string regionName { get; set; }
    }
}