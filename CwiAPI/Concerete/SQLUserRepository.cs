using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Abstract;
using System.Web.Script.Serialization;

namespace CwiAPI.Concrete
{
    public class SQLUserRepository : EntityContainer , IUserRepository
    {

        #region authenticates the Request and returns the Customer Identifiction no for the User
        /// <summary>
        /// authenticates the Request
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">password</param>
        /// <returns>Jquery string with authentication status and the customerID</returns>
        public string authenticateRequest(string username, string password)
        {
            string requiredInfo = "";
            bool authenticationStatus = false;
            Int32 custId = 0;
            var User = (from user in _entities.Users
                        where user.Username == username && user.Password == password
                        select user).First();
            if (User != null )
            {
                authenticationStatus = true;
                //User.Customers.Load();
                //custId = User.Customers.First().ID;
                custId = (from u in _entities.Users
                          where u.ID == User.ID
                          from b in u.Brands
                          from c in b.Customers
                          select c.ID).First();
            }
            userData data = new userData();
            data.AuthenticationStatus = authenticationStatus;
            data.customerId = custId;
            JavaScriptSerializer sr = new JavaScriptSerializer();
            requiredInfo = sr.Serialize(data);
            return requiredInfo;

        }
        #endregion

        #region authenticates the Request and returns the Customer Identifiction no for the User
        /// <summary>
        /// authenticates the Request
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">password</param>
        /// <returns>Jquery string with authentication status and the customerID</returns>
        public string authenticateRequestAllCustomers(string username, string password)
        {
            string requiredInfo = "";
            bool authenticationStatus = false;
            Int32[] custId = { 0 };
            var User = (from user in _entities.Users
                        where user.Username == username && user.Password == password
                        select user).First();
            if (User != null)
            {
                authenticationStatus = true;
                //User.Customers.Load();
                //custId = User.Customers.Select(c => c.ID).ToArray();
                custId = (from u in _entities.Users
                         where u.ID == User.ID 
                         from b in u.Brands 
                         from c in b.Customers 
                         select c.ID).ToArray();
            }
            userDataWithAllCustomers data = new userDataWithAllCustomers();
            data.AuthenticationStatus = authenticationStatus;
            data.customerIds = custId;
            JavaScriptSerializer sr = new JavaScriptSerializer();
            requiredInfo = sr.Serialize(data);
            return requiredInfo;

        }
        #endregion

        #region authenticates the Request and returns the Brand Identifiction no for the User
        /// <summary>
        /// authenticates the Request
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">password</param>
        /// <returns>Jquery string with authentication status and the brandID</returns>
        public string authenticateRequestWithBrand(string username, string password)
        {
            string requiredInfo = "";
            bool authenticationStatus = false;
            int brdId = 0;
            var User = (from user in _entities.Users
                        where user.Username == username && user.Password == password
                        select user).First();
            if (User != null)
            {
                authenticationStatus = true;
                //User.Customers.Load();
                //custId = User.Customers.Select(c => c.ID).ToArray();
                brdId = (from u in _entities.Users
                            from b in u.Brands
                            where u.Username==username && u.Password==password
                              select b.ID).First();
            }
            userDataWithBrand data = new userDataWithBrand();
            data.AuthenticationStatus = authenticationStatus;
            data.brandId = brdId;
            JavaScriptSerializer sr = new JavaScriptSerializer();
            requiredInfo = sr.Serialize(data);
            return requiredInfo;

        }
        #endregion
    
    }


    public class userData
    {
        public Boolean AuthenticationStatus { get; set; }
        public Int32 customerId { get; set; }

    }

    public class userDataWithAllCustomers
    {
        public Boolean AuthenticationStatus { get; set; }
        public Int32[] customerIds { get; set; }
    }

    public class userDataWithBrand
    {
        public Boolean AuthenticationStatus { get; set; }
        public int brandId { get; set; }
    }
}