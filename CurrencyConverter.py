import json
import os

# Class to store currency data
class CurrencyData:
    def __init__(self, rates):
        self.rates = rates  # Dictionary of currency rates and details

# Function to load currency data from a JSON file
def load_currency_data(file_path):
    if not os.path.exists(file_path):  # Check if the file exists
        print(f"JSON file not found: {file_path}")
        return None

    try:
        # Open and parse the JSON file with UTF-8 encoding
        with open(file_path, 'r', encoding='utf-8') as file:
            data = json.load(file)
            return CurrencyData(data['rates'])  # Return loaded data as a CurrencyData object
    except Exception as ex:
        print(f"Error loading currency data: {ex}")  # Handle any file reading/parsing errors
        return None

# Function to display all available currencies
def display_available_currencies(currency_data):
    print("\nAvailable currencies:")  # Header for the list of currencies
    for currency, info in currency_data.rates.items():  # Iterate through all currencies
        print(f"{currency}: {info['name']} ({info['symbol']})")  # Print currency code, name, and symbol
    print("")  # Add a blank line for better readability

# Function to get a valid currency code from the user
def get_currency_code(prompt, currency_data):
    while True:
        code = input(prompt).upper()  # Convert user input to uppercase
        if code in currency_data.rates:  # Check if the code exists in the rates
            return code
        print("Invalid currency code. Please try again.")  # Prompt again for valid input

# Function to get a valid numeric amount from the user
def get_amount():
    while True:
        try:
            amount = float(input("Enter amount to convert: "))  # Parse user input as a float
            return amount
        except ValueError:
            print("Invalid amount. Please enter a numeric value.")  # Handle invalid input

# Function to calculate the converted amount between currencies
def convert_currency(currency_data, from_currency, to_currency, amount):
    from_rate = currency_data.rates[from_currency]['rate']  # Get the rate for the source currency
    to_rate = currency_data.rates[to_currency]['rate']  # Get the rate for the target currency
    return (amount / from_rate) * to_rate  # Perform the conversion calculation

# Function to display the result of the currency conversion
def display_conversion_result(currency_data, from_currency, to_currency, amount, converted_amount):
    from_currency_name = currency_data.rates[from_currency]['name']
    to_currency_name = currency_data.rates[to_currency]['name']
    print(f"\n{amount} {from_currency} ({from_currency_name}, {currency_data.rates[from_currency]['symbol']}) "
          f"is equal to {converted_amount:.2f} {to_currency} ({to_currency_name}, {currency_data.rates[to_currency]['symbol']}).")

# Function to ask the user if they want to continue or exit
def get_continue_option():
    while True:
        option = input("\nDo you want to perform another conversion? (y/n): ").lower()
        if option in ["y", "n"]:  # Ensure input is 'y' or 'n'
            return option
        print("Invalid input. Please enter 'y' or 'n'.")  # Handle invalid input

# Main function to control program flow
def main():
    json_file_path = "currencies.json"  # Path to the JSON file with currency data
    currency_data = load_currency_data(json_file_path)  # Load currency data
    if currency_data is None:  # Exit if data could not be loaded
        return

    display_available_currencies(currency_data)  # Display the list of available currencies

    while True:  # Loop to allow multiple conversions
        from_currency = get_currency_code("Enter source currency code: ", currency_data)
        to_currency = get_currency_code("Enter target currency code: ", currency_data)
        amount = get_amount()

        # Perform conversion and display the result
        converted_amount = convert_currency(currency_data, from_currency, to_currency, amount)
        display_conversion_result(currency_data, from_currency, to_currency, amount, converted_amount)

        # Ask user if they want to continue or exit
        if get_continue_option() == "n":
            print("\nThank you for using the Currency Converter. Goodbye!")
            break

# Start the program if the script is executed directly
if __name__ == "__main__":
    main()
