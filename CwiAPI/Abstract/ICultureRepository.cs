using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CwiAPI.Abstract
{
    interface ICultureRepository
    {
        string getCultureId(string languageCode);

        string getCultureLocale(Int32 cultureId);
    }

}


