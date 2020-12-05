using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Azure.CosmosDB.ConsoleDemo.Model
{
    public class UserModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string Remark { get; set; }
    }
}
