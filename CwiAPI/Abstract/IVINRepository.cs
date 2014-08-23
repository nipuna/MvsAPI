using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CwiAPI.Abstract
{
    public interface IVINRepository
    {
        string getAllVINs(Int32 customerId, Int32 regionId);
    }
}
