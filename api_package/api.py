from api_package.passwords import api_finance,api_currency
from datetime import date, timedelta
import requests


def get_finance():
    api_key = api_finance
    data = {}
    symbols = ["AAPL", "MSFT", "AMZN", "GOOGL"]
    current_date = date.today() - timedelta(days=1)
    for symbol in symbols:
        response = requests.get(f"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={api_key}")
        data[symbol] = float(response.json()["Time Series (Daily)"][current_date.strftime("%Y-%m-%d")]["4. close"])
    return data
    

def get_currency():
    response = requests.get(f"https://openexchangerates.org/api/latest.json?app_id={api_currency}")
    dict = {}
    symbols = ["UAH","EUR","GBP"]
    data = response.json()["rates"]
    for symbol in symbols:
        if symbol == "UAH":
            dict["USD"] = data[symbol]
        else:
            dict[symbol] = data["UAH"] /data[symbol]
    return dict


def get_crypto():
    # If we want to get list of all coins with id,symbol,names and etc.
    # response = requests.get("https://api.coingecko.com/api/v3/coins/list")
    dict = {}
    response = requests.get("https://api.coingecko.com/api/v3/coins/markets?vs_currency=USD&ids=bitcoin,ethereum,polkadot")
    data = response.json()
    for elem in data:
        dict[elem["id"]] = elem["current_price"]

    return dict


if __name__ == "__main__":
    print(get_finance())
