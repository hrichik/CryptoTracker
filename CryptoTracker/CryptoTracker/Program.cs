using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;


public class Program() 
{

    public static async Task<decimal> getCADPrice(decimal price) //function to take usd value to convert to cad
    {
        decimal currentPriceCAD = price * (decimal)1.38; //multiplies decimal * decimal in order to find cad
        return currentPriceCAD; //in the future i will implement an api to get the live conversion rate between cad and usd
                
    }
    public static async Task getCryptoPrice(string val)
    {
        string urlAPI = $"https://api.coingecko.com/api/v3/simple/price?ids={val}&vs_currencies=usd"; //api url

        using HttpClient client = new HttpClient(); //new httpclient is set up

        try
        {
            var response = await client.GetStringAsync(urlAPI); //get response based on url and val within url

            using JsonDocument doc = JsonDocument.Parse(response); //parses the document into C# acceptable structured object
            var root = doc.RootElement; //gets the root of the json tree, so the outermost {}

            var currentPrice = root.GetProperty(val).GetProperty("usd").GetDecimal(); //

            if (currentPrice.Equals(null))
            {
                Console.WriteLine("No Results Found"); //copied this from my other code but this is never actually used here since
                                                       //errors are handled by the exception
            }
            else
            {

                Console.ForegroundColor = ConsoleColor.Blue; //changes to blue for usd
                Console.WriteLine($"{val} Price: $" + currentPrice + " USD");
                decimal cadPrice = await getCADPrice(currentPrice); //await on cadprice to complete
                Console.ForegroundColor = ConsoleColor.Red; //change to red for cad
                Console.WriteLine($"{val} Price: $" + Math.Round(cadPrice, 2) + " CAD"); //round to two decimal spots


            }

        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; //yellow for errors
            Console.WriteLine("Error: " + ex);
            Console.ForegroundColor = ConsoleColor.White; //makes sure console returns to white instead of staying yellow
 
        }

    }

    public static async Task askForAnotherCrypto()
    {
        await Task.Delay(1000);
        Console.ForegroundColor = ConsoleColor.White; //makes sure its white prior to writing line
        Console.WriteLine("Please enter another cryptocurrency:");
        //this method asks for another cryptocurrency, i just wanted to explore the functionalities of 
        //async and await so i made this its own function
    }

    public static async Task Main(string[] args)
    {
        bool running = true; //boolean used to loop program
        
        Console.WriteLine("Hello to the Crypto Tracker!");
        Console.WriteLine("Please enter a cryptocurrency:");
        while (running) //while true
        {
            var userInput = Console.ReadLine(); //read user input
            if (userInput == "QQQ") //if QQQ is inputted the program ends
            {
                running = false;
            }
            else //otherwise, await is called on both the cryptoprice function and askforanothercrypto
            { 
                await Task.WhenAll(getCryptoPrice(userInput), askForAnotherCrypto());
            }

            
        }

    }
}
