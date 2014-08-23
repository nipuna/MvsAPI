using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CwiAPI.Abstract
{
    interface IBrandRepository
    {
        string getCustomerIdForBrand(string brandName);
    }
}
