using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.CosmosDB.ConsoleDemo.Model
{
    public class UserModel
    {
        public string PartitionKey { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string Remark { get; set; }
    }
}
