using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    // Class that represents currency
    public class Currency
    {
        public double Rate { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
    }

    public class CurrencyData
    {
      //  public bool Success { get; set; }
       // public long Timestamp { get; set; }
       // public string Base { get; set; }
       // public string Date { get; set; }
        public Dictionary<string, Currency> Rates { get; set; }
    }

    static void Main(string[] args)
    {
        Run();
    }

    static void Run()
    {
        string jsonFilePath = "currencies.json";

        var currencyData = LoadCurrencyData(jsonFilePath);
        if (currencyData == null)
        {
            Console.WriteLine("Failed to load currency data.");
            Console.WriteLine("Press any key to exit the program...");
            Console.ReadKey();
            return;
        }

        DisplayAvailableCurrencies(currencyData);

        string fromCurrency = GetCurrencyCode("Enter source currency code: ");
        string toCurrency = GetCurrencyCode("Enter target currency code: ");
        double amount = GetAmount();

        if (!currencyData.Rates.ContainsKey(fromCurrency) || !currencyData.Rates.ContainsKey(toCurrency))
        {
            Console.WriteLine("One or both currency codes are invalid.");
            Console.WriteLine("Press any key to exit the program...");
            Console.ReadKey();
            return;
        }

        double convertedAmount = ConvertCurrency(currencyData, fromCurrency, toCurrency, amount);
        DisplayConversionResult(currencyData, fromCurrency, toCurrency, amount, convertedAmount);
    }

    static CurrencyData LoadCurrencyData(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"JSON file not found: {filePath}");
            Console.WriteLine("Press any key to exit the program...");
            Console.ReadKey();
            return null;
        }

        try
        {
            string jsonData = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<CurrencyData>(jsonData, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading currency data: {ex.Message}");
            Console.WriteLine("Press any key to exit the program...");
            Console.ReadKey();
            return null;
        }
    }

    static void DisplayAvailableCurrencies(CurrencyData currencyData)
    {
        Console.WriteLine("Hello, and welcome to the Currency Converter program.");
        Console.WriteLine("We have a range of currencies available for conversion.");
        Console.WriteLine("The following currencies are available for conversion in our program: ");
        //Console.WriteLine("Available currencies:");
        foreach (var currency in currencyData.Rates)
        {
            Console.WriteLine($"{currency.Key}: {currency.Value.Name} ({currency.Value.Symbol})");
        }
    }

    static string GetCurrencyCode(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine().ToUpper();
    }

    static double GetAmount()
    {
        Console.Write("Enter amount to convert: ");
        while (true)
        {
            if (double.TryParse(Console.ReadLine(), out double amount))
            {
                return amount;
            }
            Console.WriteLine("Invalid amount. Please enter a numeric value:");
        }
    }

    static double ConvertCurrency(CurrencyData currencyData, string fromCurrency, string toCurrency, double amount)
    {
        double fromRate = currencyData.Rates[fromCurrency].Rate;
        double toRate = currencyData.Rates[toCurrency].Rate;
        return (amount / fromRate) * toRate;
    }

    static void DisplayConversionResult(CurrencyData currencyData, string fromCurrency, string toCurrency, double amount, double convertedAmount)
{
    string fromCurrencyName = currencyData.Rates[fromCurrency].Name;
    string toCurrencyName = currencyData.Rates[toCurrency].Name;

    Console.WriteLine($"{amount} {fromCurrency} ({fromCurrencyName}, {currencyData.Rates[fromCurrency].Symbol}) " +
                      $"is equal to {convertedAmount:F2} {toCurrency} ({toCurrencyName}, {currencyData.Rates[toCurrency].Symbol}).");
    Console.WriteLine("Thank you for using our program, and have a great day!");
    Console.WriteLine("Press any key to exit the program....");
    Console.ReadKey();
}
}




