using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.CosmosDB.Models
{
    public class UserModel
    {
        [Key]
        public string PartitionKey { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string Remark { get; set; }
    }
}
