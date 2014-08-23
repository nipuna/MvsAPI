using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Entities;
using CwiAPI.Abstract;

namespace CwiAPI.Concrete
{
    public class SQLLoggingRepository: EntityContainer,ILoggingRepository
    {
        public void InsertLogDetails(Int32 VehicleID, Int32 PageID, DateTime DateAndTime, Int32 CultureId, Int32 TestInstanceID)
        {
            CWILog AddLogEntry = new CWILog();

            AddLogEntry.DateTime = DateAndTime;
            AddLogEntry.Vehicle = (from v in _entities.Vehicles where v.ID == VehicleID select v).First();
            AddLogEntry.CWIPage = (from p in _entities.CWIPages where p.ID == PageID select p).First();
            AddLogEntry.Culture = (from clt in _entities.Cultures where clt.ID == CultureId select clt).First();
            AddLogEntry.TestInstance = (from tstI in _entities.TestInstances where tstI.ID == TestInstanceID select tstI).First();

            _entities.AddToCWILogs(AddLogEntry);
            _entities.SaveChanges();
        }
    }    
}