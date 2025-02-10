using System.Collections.Generic;

namespace ShoppingSystemProject
{
    internal class Program
    {
        static public List<string> CartItems = new List<string>();

        static public Dictionary<string, double> ItemPrices = new Dictionary<string, double>()
        {
            {"Camera", 1500 },
            {"Laptop", 3000 },
            {"TV", 2500 }
        };

        static Stack<string> Actions = new Stack<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("\"Welcome To The Shopping System\"");
            Console.WriteLine("----------------------------------");
            while (true)
            {
                Console.WriteLine("***********************");
                Console.WriteLine("* 1. Add item to cart *");
                Console.WriteLine("* 2. View cart items  *");
                Console.WriteLine("* 3. Remove item      *");
                Console.WriteLine("* 4. Checkout         *");
                Console.WriteLine("* 5. Undo last action *");
                Console.WriteLine("* 6. Exit             *");
                Console.WriteLine("***********************");


                Console.Write("Enter your choice number: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddItem();
                        break;
                    case 2:
                        ViewCart();
                        break;
                    case 3:
                        RemoveItem();
                        break;
                    case 4:
                        Checkout();
                        break;
                    case 5:
                        UndoLastAction();
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid code entered please try again");
                        break;
                }
            }
        }

        private static void AddItem()
        {
            Console.WriteLine("Available items");
            foreach (var item in ItemPrices)
            {
                Console.WriteLine($"Item: {item.Key}, Price: {item.Value}$");
            }
            Console.Write("Please enter product name: ");
            string cartItem = Console.ReadLine();

            if (ItemPrices.ContainsKey(cartItem))
            { 
                CartItems.Add(cartItem);
                Actions.Push($"Item {cartItem} added to cart");
                Console.WriteLine($"Item {cartItem} is added to your cart");
            }
            else
            {
                Console.WriteLine("Item is out of stock or not available");
            }

        }

        private static void ViewCart()
        {
            if (CartItems.Any())
            {
                IEnumerable<Tuple<string, double>> itemPricesCollection = GetCartPrices();
                foreach (var item in itemPricesCollection)
                {
                    Console.WriteLine($"Item: {item.Item1}, Price: {item.Item2}$");
                }
            }
            else
            {
                Console.WriteLine("Cart is empty");
            }
        }

        private static IEnumerable<Tuple<string, double>> GetCartPrices()
        {
            List<Tuple<string, double>> cartPrices = new List<Tuple<string, double>>();
            foreach (var item in CartItems)
            {
                double price = 0;
                bool foundItem = ItemPrices.TryGetValue(item, out price);
                if (foundItem)
                {
                    Tuple<string, double> ItemPrice = new Tuple<string, double>(item, price);
                    cartPrices.Add(ItemPrice);
                }
            }
            return cartPrices;
        }

        private static void RemoveItem()
        {
            ViewCart();
            if (CartItems.Any())
            {
                Console.Write("Please select item to remove: ");
                string itemToRemove = Console.ReadLine();

                if (CartItems.Contains(itemToRemove))
                {
                    CartItems.Remove(itemToRemove);
                    Actions.Push($"Item {itemToRemove} removed to cart");
                    Console.WriteLine($"Item {itemToRemove} removed successfully");
                }
                else
                {
                    Console.WriteLine("Item doesn't exist in the cart");
                }
            }
            else
            {
                Console.WriteLine("Cart is empty");
            }
        }

        private static void Checkout()
        {
            if(CartItems.Any())
            {
                double TotalPrice = 0;
                Console.WriteLine("Your cart items are:");
                ViewCart();
                IEnumerable<Tuple<string, double>> ItemAndPrices = GetCartPrices();
                foreach (var item in ItemAndPrices)
                {
                    TotalPrice += item.Item2;
                }
                Console.WriteLine($"Total Price: {TotalPrice}$");
                Console.WriteLine("Please proceed to payment, Thank you for shopping with us");
                CartItems.Clear();
                Actions.Push("Checkout");
            }
            else
            {
                Console.WriteLine("Cart is empty");
            }
        }

        private static void UndoLastAction()
        {
            if(Actions.Any())
            {
                string lastAction = Actions.Pop();
                Console.WriteLine($"Your last action is *{lastAction}*");
                string[] actionsArray = lastAction.Split();
                if (actionsArray.Contains("added"))
                {
                    CartItems.Remove(actionsArray[1]);
                }
                else if(actionsArray.Contains("removed"))
                {
                    CartItems.Add(actionsArray[1]);
                }
                else
                {
                    Console.WriteLine("Sorry, Checkout can not be undo please ask for refund");
                }
            }
        }
    }
}
