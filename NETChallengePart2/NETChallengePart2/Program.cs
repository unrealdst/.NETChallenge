using System;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace NetChallengePart2
{
    class Program
    {
        private enum Commands { Get, Add, Quantity, Remove, Exit, Unknown };
        private static ConnectionService connectionService;

        static void Main(string[] args)
        {
            connectionService = new ConnectionService();
            Console.WriteLine("Welcome to Net Challenge shop, buy 1 pay for 2");
            Execute();
        }

        private static void Execute()
        {
            while (true)
            {
                var command = Menu();
                SelectAction(command);
            }
        }

        static private void SelectAction(Commands command)
        {
            switch (command)
            {
                case Commands.Remove:
                    Remove();
                    break;
                case Commands.Add:
                    Add();
                    break;
                case Commands.Quantity:
                    Quantity();
                    break;
                case Commands.Unknown:
                    Execute();
                    break;
                case Commands.Exit:
                    Environment.Exit(0);
                    break;
                case Commands.Get:
                    Get();
                    break;
            }
        }

        private static void Get()
        {
            var bucket = connectionService.GetAsync().Result;
            Console.WriteLine($"{Environment.NewLine}Items in your bucket:");
            bucket.Items.ForEach(x => Console.WriteLine($"Item: {x.Name}(id:{x.Id}), Amount: {x.Quantity}, Price for one: {x.PriceForUnit}"));
            Console.WriteLine($"Items: {bucket.Items.Count}");
        }

        private static void Remove()
        {
            Console.WriteLine("Remove item with id:");
            var itemId = Convert.ToInt32(Console.ReadLine());

            connectionService.Quantity(itemId, 0);
        }

        private static void Add()
        {
            var item = new ItemViewModel();
            Console.WriteLine("Add new item to your bucket");
            Console.WriteLine("Item name");
            item.Name = Console.ReadLine();
            Console.WriteLine("Item price");
            item.PriceForUnit = Convert.ToDecimal(Console.ReadLine());
            Console.WriteLine("Amount");
            item.Quantity = Convert.ToInt32(Console.ReadLine());

            connectionService.Add(item);
        }

        private static void Quantity()
        {
            Console.WriteLine("Change amount of items for item with id:");
            var itemId = Convert.ToInt32(Console.ReadLine()); 
            Console.WriteLine("How much");
            var quantity = Convert.ToInt32(Console.ReadLine());

            connectionService.Quantity(itemId, quantity);
        }

        static private Commands Menu()
        {
            Console.WriteLine($"{Environment.NewLine}What you want to do ?");
            Console.WriteLine($"{(int)Commands.Get} - See your bucket");
            Console.WriteLine($"{(int)Commands.Add} - Add new order to bucket");
            Console.WriteLine($"{(int)Commands.Quantity} - Chnage amount of items");
            Console.WriteLine($"{(int)Commands.Remove} - Clear item");
            Console.WriteLine($"{(int)Commands.Exit} - Exit");

            var action = Console.ReadKey();
            Debug.WriteLine($"{nameof(action)}: {action}");

            int result;
            if (Int32.TryParse(action.KeyChar.ToString(), out result))
            {
                if (Enum.IsDefined(typeof(Commands), result))
                {
                    return (Commands)result;
                }
                return Commands.Unknown;
            }
            return Commands.Unknown;
        }
    }
}
