using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CwiAPI.Abstract
{
    public interface IUserRepository
    {
       string authenticateRequest(string username, string password);

       string authenticateRequestAllCustomers(string username, string password);

       string authenticateRequestWithBrand(string username, string password);
    }
}
