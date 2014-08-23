using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CwiAPI.Abstract
{
    public interface IPhoneRepository
    {
        string getAllPhoneDetails(string systemId);

        string getTopFeatureResults(int testInstanceId);

        string getDistinctPhoneDetails(string SystemIds);
    }
}
