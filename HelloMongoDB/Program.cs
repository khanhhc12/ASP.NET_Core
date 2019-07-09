using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleTables;
using MongoDB.Bson;

namespace HelloMongoDB
{
    class Program
    {
        static void Main(string[] args)
        {
            // mongod --dbpath d:\KhanhHC2\Programs\Workspace\mongodb
            DataAccess dataAccess = new DataAccess();
            bool isExit = false;
            while (!isExit)
            {
                try
                {
                    Console.WriteLine("");
                    Console.WriteLine("1. find");
                    Console.WriteLine("2. insert");
                    Console.WriteLine("3. update");
                    Console.WriteLine("4. delete");
                    Console.WriteLine("5. exit");
                    Console.Write("key: ");
                    string key = Console.ReadLine();
                    switch (key)
                    {
                        case "1":
                            Console.Write("name: ");
                            string findName = Console.ReadLine();
                            var list = dataAccess.GetProducts()
                                .Where(p => p.Name.Contains(findName, StringComparison.CurrentCultureIgnoreCase));
                            ConsoleTable.From<Product>(list).Write();
                            break;
                        case "2":
                            Console.Write("name: ");
                            string insertName = Console.ReadLine();
                            Console.Write("price: ");
                            int insertPrice = int.Parse(Console.ReadLine());
                            Console.Write("description: ");
                            string insertDescription = Console.ReadLine();
                            dataAccess.Create(new Product
                            {
                                Name = insertName,
                                Price = insertPrice,
                                Description = insertDescription
                            });
                            ConsoleTable.From<Product>(dataAccess.GetProducts()).Write();
                            break;
                        case "3":
                            Console.Write("id: ");
                            string updateId = Console.ReadLine();
                            var updateObj = dataAccess.GetProduct(ObjectId.Parse(updateId));
                            ConsoleTable.From<Product>(new List<Product> { updateObj }).Write();
                            Console.Write("name: ");
                            string updateName = Console.ReadLine();
                            Console.Write("price: ");
                            string updatePrice = Console.ReadLine();
                            Console.Write("description: ");
                            string updateDescription = Console.ReadLine();
                            updateObj.Name = updateName == "" ? updateObj.Name : updateName;
                            updateObj.Price = updatePrice == "" ? updateObj.Price : int.Parse(updatePrice);
                            updateObj.Description = updateDescription == "" ? updateObj.Description : updateDescription;
                            dataAccess.Update(updateObj.Id, updateObj);
                            ConsoleTable.From<Product>(dataAccess.GetProducts()).Write();
                            break;
                        case "4":
                            Console.Write("id: ");
                            dataAccess.Remove(ObjectId.Parse(Console.ReadLine()));
                            ConsoleTable.From<Product>(dataAccess.GetProducts()).Write();
                            break;
                        case "5":
                            isExit = true;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
