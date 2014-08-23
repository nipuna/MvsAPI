using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CwiAPI.Abstract
{
    public interface IRegionRepository
    {
        string getAllRegions(Int32 customerId);
    }
}
