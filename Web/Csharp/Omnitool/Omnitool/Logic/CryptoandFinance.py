import requests
import sqlite3
from datetime import datetime
from datetime import timedelta
from passwords import api_key

class CryptoandFinance:

    def __init__(self):
        self.connection = sqlite3.connect("./app.db")
        self.cursor = self.connection.cursor()


    def crypto(self):
        response = requests.get("https://api.coingecko.com/api/v3/coins/markets?vs_currency=USD&ids=bitcoin,ethereum,polkadot")
        data = response.json()
        dict = {}
        for elem in data:
            dict[elem["id"]] = elem["current_price"]
        now = datetime.now()
        creation_date = datetime(now.year, now.month, now.day)
        print(dict)
        for elem in dict:
            self.cursor.execute("INSERT INTO Cryptos (Name,Price,CreatedAt) VALUES(?,?,?)",(elem, dict[elem],creation_date))
        self.connection.commit()

    def finance(self):
        symbols = ["AAPL", "MSFT", "AMZN", "GOOGL"]
        current_date = datetime.now() - timedelta(days=1)
        now = datetime.now()
        creation_date = datetime(now.year, now.month, now.day)
        for symbol in symbols:
            response = requests.get(f"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={api_key}")
            self.cursor.execute("INSERT INTO Finances (Name,Price,CreatedAt) VALUES(?,?,?)",(symbol,
                                                                                            float(response.json()["Time Series (Daily)"][current_date.strftime("%Y-%m-%d")]["4. close"]),
                                                                                            creation_date))
        self.connection.commit()
    
def main ():
    crytoandfinance = CryptoandFinance()
    crytoandfinance.crypto()
    crytoandfinance.finance()

main()