using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;


public class Program()
{

    public static async Task<decimal> getCADPrice(decimal price)
    {
        decimal currentPriceCAD = price * (decimal)1.38;
        return currentPriceCAD;
                
    }
    public static async Task getCryptoPrice(string val)
    {
        string urlAPI = $"https://api.coingecko.com/api/v3/simple/price?ids={val}&vs_currencies=usd";

        using HttpClient client = new HttpClient();

        try
        {
            var response = await client.GetStringAsync(urlAPI);

            using JsonDocument doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            var currentPrice = root.GetProperty(val).GetProperty("usd").GetDecimal();

            if (currentPrice.Equals(null))
            {
                Console.WriteLine("No Results Found"); //if none are found, no results found is shown, and this
                                                       //triggers the else statement in the Main()
            }
            else
            {

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"{val} Price: $" + currentPrice + " USD");
                decimal cadPrice = await getCADPrice(currentPrice);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{val} Price: $" + Math.Round(cadPrice, 2) + " CAD");


            }

        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Error: " + ex);
            Console.ForegroundColor = ConsoleColor.White;

        }

    }

    public static async Task askForAnotherCrypto()
    {
        await Task.Delay(1000);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Please enter another cryptocurrency:");
    }

    public static async Task Main(string[] args)
    {
        bool running = true;
        
        Console.WriteLine("Hello to the Crypto Tracker!");
        Console.WriteLine("Please enter a cryptocurrency:");
        while (running)
        {
            var userInput = Console.ReadLine();
            if (userInput == "QQQ")
            {
                running = false;
            }
            else
            { 
                await Task.WhenAll(getCryptoPrice(userInput), askForAnotherCrypto());
            }

            
        }

    }
}
