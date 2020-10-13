using Azure.CosmosDB.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Factory
{
    public class AbstractDbFactory
    {
        public static DbContext GetUserCurrentDbContext()
        {
            //一次请求公用一个实例.上下文都可以做到切换
            //return new DataModelContainer();
            DbContext db = CallContext.GetData("UserContext") as UserContext;
            if (db == null)
            {
                //db = new UserContext();
                CallContext.SetData("UserContext", db);
            }
            return db;
        }
    }
}
