using Azure.CosmosDB.ConsoleDemo.Common;
using Azure.CosmosDB.ConsoleDemo.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Azure.CosmosDB.ConsoleDemo
{
    class Program
    {
        //The Cosmos client instance
        private CosmosClient cosmosClient;

        //The database we will create
        private Database database;

        //The container we will create
        private Container container;

        public static async Task Main(string[] args)
        {

            Program p = new Program();
            await p.InitCosmosDB();
            await p.InitItem();
            await p.QueryItems();
        }

        public async List<UserModel> QueryItems()
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.LastName = 'Andersen'";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);


            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            
            List<UserModel> families = new List<UserModel>();


            var users = this.container.GetItemQueryIterator<UserModel>(queryDefinition);
            foreach (User user in users)
            {
                yield return user;
            }
                
            
        }



        public async Task InitCosmosDB()
        {
            var a = new Appsettings();

            this.cosmosClient = new CosmosClient(Appsettings.app("CosmosDB", "Endpoint"), Appsettings.app("CosmosDB", "Key"));

            #region Create CosmosDB
            this.database= await this.cosmosClient.CreateDatabaseIfNotExistsAsync(Appsettings.app("CosmosDB", "DataBase"));
            Console.WriteLine("Created Database:{0} Success\n", Appsettings.app("CosmosDB", "DataBase"));
            #endregion

            this.container = await this.database.CreateContainerIfNotExistsAsync(Appsettings.app("CosmosDB", "Container"), "/id");
            Console.WriteLine("Created Container:{0} Success\n", Appsettings.app("CosmosDB", "Container"));
        }


        public async Task InitItem()
        {
            //Create a UserModel object for the Andersen family
            var user1 = new UserModel
            {
                Id = "1",
                Name = "张无忌",
                Age = 12,
                Address = "北京市西城区鲍家街43号",
                Remark = "中央音乐学院"
            };

            var user2 = new UserModel { 
                Id = "2", 
                Name = "令狐冲", 
                Age = 20, 
                Address = "佛山市南海区灯湖东路6号", 
                Remark = "广发商学院" 
            };


            #region Add User1 Item
            ItemResponse<UserModel> user1Response = await this.container.CreateItemAsync<UserModel>(user1, new PartitionKey(user1.Id));

            Console.WriteLine("Created Item in database with id:{0} Operation consumed {1} RUs.\n", user1Response.Resource.Id, user1Response.StatusCode);
            #endregion

            #region Add User2 Item
            ItemResponse<UserModel> user2Response = await this.container.CreateItemAsync<UserModel>(user2, new PartitionKey(user2.Id));

            Console.WriteLine("Created Item in database with id:{0} Operation consumed {1} RUs.\n", user2Response.Resource.Id, user1Response.StatusCode);
            #endregion
        }


    }
}
