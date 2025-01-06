const fs = require("fs"); // File system module to handle file operations

// Class to represent individual currency details
class Currency {
  constructor(rate, symbol, name) {
    this.rate = rate; // Exchange rate for the currency
    this.symbol = symbol; // Symbol representing the currency (e.g., "$", "€")
    this.name = name; // Full name of the currency (e.g., "Euro", "US Dollar")
  }
}

// Class to store all currencies and their details
class CurrencyData {
  constructor(rates) {
    this.rates = rates; // Object holding all currencies, indexed by their codes
  }
}

// Main function that manages the program flow
function run() {
  const jsonFilePath = "./currencies.json"; // Path to the currency data file

  const currencyData = loadCurrencyData(jsonFilePath); // Load currency data from the JSON file
  if (!currencyData) {
    console.log("Failed to load currency data."); // Log an error if data cannot be loaded
    return; // Exit the program if data is not available
  }

  console.log("Welcome to the Currency Converter!\n"); // Print a welcome message
  displayAvailableCurrencies(currencyData); // Show a list of all available currencies at the start

  while (true) {
    // Get input from the user for the source currency
    const fromCurrency = getCurrencyCode("Enter source currency code: ");
    const toCurrency = getCurrencyCode("Enter target currency code: ");
    const amount = getAmount(); // Get the amount the user wants to convert

    // Check if the entered currency codes are valid
    if (!currencyData.rates[fromCurrency] || !currencyData.rates[toCurrency]) {
      console.log("One or both currency codes are invalid."); // Notify the user of invalid input
      continue; // Restart the loop for new inputs
    }

    // Perform the currency conversion
    const convertedAmount = convertCurrency(
      currencyData,
      fromCurrency,
      toCurrency,
      amount
    );

    // Display the conversion result
    displayConversionResult(
      currencyData,
      fromCurrency,
      toCurrency,
      amount,
      convertedAmount
    );

    // Ask the user if they want to perform another conversion
    const continueOption = getContinueOption();
    if (continueOption === "n") break; // Exit the loop if the user types 'n'
  }

  console.log("Thank you for using the Currency Converter. Goodbye!"); // Print a farewell message when exiting
}

// Function to load currency data from a JSON file
function loadCurrencyData(filePath) {
  if (!fs.existsSync(filePath)) {
    console.log(`JSON file not found: ${filePath}`); // Notify if the file is missing
    return null; // Return null to indicate failure
  }

  try {
    const jsonData = fs.readFileSync(filePath, "utf-8"); // Read the content of the file
    const parsedData = JSON.parse(jsonData); // Parse the file content into a JavaScript object

    // Transform parsed data into Currency objects for structured access
    const rates = Object.fromEntries(
      Object.entries(parsedData.rates).map(([code, details]) => [
        code,
        new Currency(details.rate, details.symbol, details.name),
      ])
    );
    return new CurrencyData(rates); // Return an instance of CurrencyData
  } catch (err) {
    console.log(`Error loading currency data: ${err.message}`); // Log parsing errors
    return null; // Return null if an error occurs
  }
}

// Function to display all available currencies at the start
function displayAvailableCurrencies(currencyData) {
  console.log("Available currencies:"); // Print a header
  for (const [code, currency] of Object.entries(currencyData.rates)) {
    // Iterate over all currencies and print their details
    console.log(`${code}: ${currency.name} (${currency.symbol})`);
  }
  console.log(""); // Add a blank line for readability
}

// Function to prompt the user for a currency code
function getCurrencyCode(prompt) {
  const readline = require("readline-sync"); // Synchronous input module
  return readline.question(prompt).toUpperCase(); // Get input and convert to uppercase
}

// Function to prompt the user for the amount to convert
function getAmount() {
  const readline = require("readline-sync"); // Input module for terminal
  while (true) {
    // Loop until valid input is received
    const input = readline.question("Enter amount to convert: "); // Prompt for input
    const amount = parseFloat(input); // Try to parse input as a number
    if (!isNaN(amount)) return amount; // Return valid numeric input
    console.log("Invalid amount. Please enter a numeric value."); // Notify on invalid input
  }
}

// Function to calculate the converted amount
function convertCurrency(currencyData, fromCurrency, toCurrency, amount) {
  const fromRate = currencyData.rates[fromCurrency].rate; // Get the exchange rate for the source currency
  const toRate = currencyData.rates[toCurrency].rate; // Get the exchange rate for the target currency
  return (amount / fromRate) * toRate; // Apply the conversion formula
}

// Function to display the result of the conversion
function displayConversionResult(
  currencyData,
  fromCurrency,
  toCurrency,
  amount,
  convertedAmount
) {
  const fromCurrencyName = currencyData.rates[fromCurrency].name; // Name of the source currency
  const toCurrencyName = currencyData.rates[toCurrency].name; // Name of the target currency
  const fromSymbol = currencyData.rates[fromCurrency].symbol; // Symbol of the source currency
  const toSymbol = currencyData.rates[toCurrency].symbol; // Symbol of the target currency

  // Print a detailed summary of the conversion
  console.log(
    `${amount} ${fromCurrency} (${fromCurrencyName}, ${fromSymbol}) is equal to ${convertedAmount.toFixed(
      2
    )} ${toCurrency} (${toCurrencyName}, ${toSymbol}).`
  );
}

// Function to ask the user if they want to continue or exit
function getContinueOption() {
  const readline = require("readline-sync"); // Synchronous input module
  while (true) {
    const input = readline.question(
      "Do you want to perform another conversion? (y/n): "
    );
    if (input.toLowerCase() === "y" || input.toLowerCase() === "n") {
      return input.toLowerCase(); // Return valid input
    }
    console.log("Invalid option. Please enter 'y' or 'n'."); // Prompt again if invalid
  }
}

// Start the program
run();
