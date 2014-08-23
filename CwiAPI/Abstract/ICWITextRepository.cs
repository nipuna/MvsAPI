using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CwiAPI.Abstract
{
    interface ICWITextRepository
    {
        string getCWIText(int customerId, string brandName, Int32 cultureId);

        string getCWITextForBrand(int brandId, Int32 cultureId);

    }
}
