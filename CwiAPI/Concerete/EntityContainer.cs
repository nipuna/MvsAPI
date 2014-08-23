using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CwiAPI.Entities;

namespace CwiAPI.Concrete
{
    public class EntityContainer
    {
        protected IOTAEntities _entities;

        public EntityContainer()
        {
            _entities = new IOTAEntities();
        }

    }
}