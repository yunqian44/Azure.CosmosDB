using Azure.CosmosDB.ConsoleDemo.Common;
using Azure.CosmosDB.ConsoleDemo.Model;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            await p.QueryItems(string.Empty);

            await p.ModifyItem("令狐冲");


            await p.DeleteItem("令狐冲");


            await p.QueryItems(string.Empty);


        }

        /// <summary>
        /// Query user base on name 
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public async Task<List<UserModel>> QueryItems(string name)
        {
            var sqlQueryText = "SELECT * FROM c where 1=1";

            if (!string.IsNullOrEmpty(name))
            {
                sqlQueryText += " and c.Name='" + name + "'";
            }

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

            var users = new List<UserModel>();
            var queryUsers = this.container.GetItemQueryIterator<UserModel>(queryDefinition);


            while (queryUsers.HasMoreResults)
            {
                FeedResponse<UserModel> currentResultSet = await queryUsers.ReadNextAsync();
                foreach (UserModel user in currentResultSet)
                {
                    Console.WriteLine("\tRead {0}\n", user.Name);
                    users.Add(user);
                }
            }
            return users;
        }


        public async Task InitCosmosDB()
        {
            var a = new Appsettings();

            #region Create CosmosClient
            this.cosmosClient = new CosmosClient(Appsettings.app("CosmosDB", "Endpoint"), Appsettings.app("CosmosDB", "Key")); 
            #endregion

            #region Create CosmosDB
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(Appsettings.app("CosmosDB", "DataBase"));
            Console.WriteLine("Created Database:{0} Success\n", Appsettings.app("CosmosDB", "DataBase"));
            #endregion

            #region Create Container 
            this.container = await this.database.CreateContainerIfNotExistsAsync(Appsettings.app("CosmosDB", "Container"), "/id"); 
            #endregion

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

            var user2 = new UserModel
            {
                Id = "2",
                Name = "令狐冲",
                Age = 20,
                Address = "佛山市南海区灯湖东路6号",
                Remark = "广发商学院"
            };

            #region Query User1 of '张无忌'
            var user1s = await QueryItems(user1.Name);
            #endregion


            #region Add User1 Item
            if (user1s.Count <= 0)
            {
                ItemResponse<UserModel> user1Response = await this.container.CreateItemAsync<UserModel>(user1, new PartitionKey(user1.Id));

                Console.WriteLine("Created Item in database with id:{0} Operation consumed {1} RUs.\n", user1Response.Resource.Id, user1Response.StatusCode);
            }
            #endregion


            #region Query User2 of '令狐冲'
            var user2s = await QueryItems(user2.Name);
            #endregion

            #region Add User2 Item
            if (user2s.Count <= 0)
            {
                ItemResponse<UserModel> user2Response = await this.container.CreateItemAsync<UserModel>(user2, new PartitionKey(user2.Id));

                Console.WriteLine("Created Item in database with id:{0} Operation consumed {1} RUs.\n", user2Response.Resource.Id, user2Response.StatusCode);
            }
            #endregion
        }

        /// <summary>
        /// Delete user base on name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task DeleteItem(string name)
        {
            var users= await QueryItems(name);

           var deleteResponse= await this.container.DeleteItemAsync<UserModel>(users.First().Id, new PartitionKey(users.First().Id));

            Console.WriteLine("delete'" + name + " item'{0}\n", deleteResponse.StatusCode == HttpStatusCode.NoContent ? "success" : "fail");
        }

        public async Task ModifyItem(string name)
        {
            var users = await QueryItems(name);

            var modifyUser = users.FirstOrDefault();
            modifyUser.Address = "上海市静安区石板街73弄";
           var modifyResponse= await this.container.ReplaceItemAsync<UserModel>(modifyUser, modifyUser.Id, new PartitionKey(modifyUser.Id));
            Console.WriteLine("name equal to '"+name+"',his family address modify {0}\n", modifyResponse.StatusCode==HttpStatusCode.OK?"success":"fail");
        }

    }
}
