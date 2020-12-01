using Azure.CosmosDB.ConsoleDemo.Common;
using Microsoft.Azure.Cosmos;
using System;
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
            Console.WriteLine("Hello World!");


        }



        public async Task InitCosmosDB()
        {
            this.cosmosClient = new CosmosClient(Appsettings.app("CosmosDB", "Endpoint"), Appsettings.app("CosmosDB", "Key"));

            #region Create CosmosDB
            await this.cosmosClient.CreateDatabaseIfNotExistsAsync(Appsettings.app("CosmosDB", "DataBase"));
            Console.WriteLine("Created Database:{0} Success\n", Appsettings.app("CosmosDB", "DataBase"));
            #endregion

            this.container = await this.database.CreateContainerIfNotExistsAsync(Appsettings.app("CosmosDB", "Container"), "/id");
            Console.WriteLine("Created Container:{0} Success\n", Appsettings.app("CosmosDB", "Container"));
        }


    }
}
