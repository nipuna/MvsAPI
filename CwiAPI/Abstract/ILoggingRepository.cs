using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CwiAPI.Abstract
{
    public interface ILoggingRepository
    {
        void InsertLogDetails(Int32 VehicleID, Int32 PageID, DateTime DateAndTime, Int32 CultureId, Int32 TestInstanceID);
    }
}

