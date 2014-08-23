using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CwiAPI.Abstract
{
    public interface IVehicleRepository
    {
        string getAllVehicleDetails(Int32 customerId);

        string getModelYears(Int32 modelId);

        string getAllVehicleDetailsWithBrand(int brandId);
    }
}
