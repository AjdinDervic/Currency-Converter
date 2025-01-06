using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    // Class that represents currency
    public class Currency
    {
        public double Rate { get; set; }  // Exchange rate for the currency
        public string Symbol { get; set; }  // Symbol of the currency (e.g., "$", "€")
        public string Name { get; set; }  // Full name of the currency (e.g., "US Dollar")
    }

    // Class to store all currency data
    public class CurrencyData
    {
        public Dictionary<string, Currency> Rates { get; set; }  // Dictionary holding all currency details
    }

    static void Main(string[] args)
    {
        Run();  // Entry point for the application
    }

    static void Run()
    {
        string jsonFilePath = "currencies.json";  // Path to the JSON file

        var currencyData = LoadCurrencyData(jsonFilePath);  // Load currency data from the JSON file
        if (currencyData == null)  // Check if data was loaded successfully
        {
            Console.WriteLine("Failed to load currency data.");
            Console.WriteLine("Press any key to exit the program...");
            Console.ReadKey();
            return;
        }

        DisplayAvailableCurrencies(currencyData);  // Display all available currencies

        while (true)  // Loop to allow multiple conversions
        {
            string fromCurrency = GetCurrencyCode("Enter source currency code: ", currencyData);  // Source currency
            string toCurrency = GetCurrencyCode("Enter target currency code: ", currencyData);  // Target currency
            double amount = GetAmount();  // Amount to convert

            // Validate currency codes
            if (!currencyData.Rates.ContainsKey(fromCurrency) || !currencyData.Rates.ContainsKey(toCurrency))
            {
                Console.WriteLine("One or both currency codes are invalid. Please try again.");
                continue;  // Restart loop for new input
            }

            // Perform currency conversion
            double convertedAmount = ConvertCurrency(currencyData, fromCurrency, toCurrency, amount);
            DisplayConversionResult(currencyData, fromCurrency, toCurrency, amount, convertedAmount);

            // Ask user if they want to continue or exit
            if (!GetContinueOption())
            {
                Console.WriteLine("Thank you for using the Currency Converter. Goodbye!");
                break;
            }
        }
    }

    static CurrencyData LoadCurrencyData(string filePath)
    {
        if (!File.Exists(filePath))  // Check if the file exists
        {
            Console.WriteLine($"JSON file not found: {filePath}");
            return null;
        }

        try
        {
            string jsonData = File.ReadAllText(filePath);  // Read the file content
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };  // Enable case-insensitive property names
            return JsonSerializer.Deserialize<CurrencyData>(jsonData, options);  // Deserialize JSON into a CurrencyData object
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading currency data: {ex.Message}");
            return null;  // Return null if an error occurs
        }
    }

    static void DisplayAvailableCurrencies(CurrencyData currencyData)
    {
        Console.WriteLine("Available currencies:");
        foreach (var currency in currencyData.Rates)
        {
            Console.WriteLine($"{currency.Key}: {currency.Value.Name} ({currency.Value.Symbol})");  // Print currency details
        }
        Console.WriteLine();  // Add a blank line for better readability
    }

    static string GetCurrencyCode(string prompt, CurrencyData currencyData)
    {
        while (true)
        {
            Console.Write(prompt);  // Display the prompt
            string code = Console.ReadLine().ToUpper();  // Read user input and convert to uppercase
            if (currencyData.Rates.ContainsKey(code))  // Check if the currency code exists
            {
                return code;
            }
            Console.WriteLine("Invalid currency code. Please try again.");  // Prompt again for valid input
        }
    }

    static double GetAmount()
    {
        Console.Write("Enter amount to convert: ");
        while (true)
        {
            if (double.TryParse(Console.ReadLine(), out double amount))  // Try to parse user input as a double
            {
                return amount;
            }
            Console.WriteLine("Invalid amount. Please enter a numeric value:");  // Prompt again if invalid input
        }
    }

    static double ConvertCurrency(CurrencyData currencyData, string fromCurrency, string toCurrency, double amount)
    {
        double fromRate = currencyData.Rates[fromCurrency].Rate;  // Get the source currency rate
        double toRate = currencyData.Rates[toCurrency].Rate;  // Get the target currency rate
        return (amount / fromRate) * toRate;  // Calculate the converted amount
    }

    static void DisplayConversionResult(CurrencyData currencyData, string fromCurrency, string toCurrency, double amount, double convertedAmount)
    {
        string fromCurrencyName = currencyData.Rates[fromCurrency].Name;  // Source currency name
        string toCurrencyName = currencyData.Rates[toCurrency].Name;  // Target currency name

        // Display the conversion result
        Console.WriteLine($"{amount} {fromCurrency} ({fromCurrencyName}, {currencyData.Rates[fromCurrency].Symbol}) " +
                          $"is equal to {convertedAmount:F2} {toCurrency} ({toCurrencyName}, {currencyData.Rates[toCurrency].Symbol}).");
    }

    static bool GetContinueOption()
    {
        Console.Write("\nDo you want to perform another conversion? (y/n): ");
        while (true)
        {
            string input = Console.ReadLine().ToLower();  // Read user input and convert to lowercase
            if (input == "y") return true;  // Continue
            if (input == "n") return false;  // Exit
            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");  // Prompt again if input is invalid
        }
    }
}
