import sqlite3
from api_package.api import get_crypto,get_finance,get_currency
from datetime import date

class Database:
    def __init__(self):
        self.connection = sqlite3.connect("./OmniTool.db")
        self.cursor = self.connection.cursor()

    def select(self):
        self.cursor.execute(f"SELECT * FROM Crypto WHERE created_on = Date('now')")
        return self.cursor.fetchall()

class Crypto(Database):
    def __init__(self):
        super().__init__()
        self.cursor.execute("""CREATE TABLE IF NOT EXISTS Crypto(
                            crypto_value VARCHAR (200) NOT NULL,
                            crypto_price REAL NOT NULL,
                            created_on TIMESTAMP NOT NULL )""")
    
    def add_value(self,value):
        creation_date = date.today()
        for elem in value:
            self.cursor.execute("INSERT INTO Crypto (crypto_value,crypto_price,created_on) VALUES(?,?,?)",(elem, value[elem],creation_date))
        self.connection.commit()

class Finance(Database):
    def __init__(self):
        super().__init__()
        self.cursor.execute("""CREATE TABLE IF NOT EXISTS Finance(
                            finance_value VARCHAR (200) NOT NULL,
                            finance_price REAL NOT NULL,
                            created_on TIMESTAMP NOT NULL )""")
    
    def add_value(self,value):
        creation_date = date.today()
        for elem in value:
            self.cursor.execute("INSERT INTO Finance (finance_value,finance_price,created_on) VALUES(?,?,?)",(elem, value[elem],creation_date))
        self.connection.commit()


class Currency(Database):
    def __init__(self):
        super().__init__()
        self.cursor.execute("""CREATE TABLE IF NOT EXISTS Currency(
                            currency_value VARCHAR (200) NOT NULL,
                            currency_price REAL NOT NULL,
                            created_on TIMESTAMP NOT NULL )""")
    
    def add_value(self,value):
        creation_date = date.today()
        for elem in value:
            self.cursor.execute("INSERT INTO Currency (currency_value,currency_price,created_on) VALUES(?,?,?)",(elem, value[elem],creation_date))
        self.connection.commit()

class DatabaseAdapter(Database):
    def __init__(self,crypto,finance,currency):
        super().__init__()
        self.crypto = crypto()
        self.finance = finance()
        self.currency = currency()
        self.connection.commit()
    
    def add(self):
        value_crypto =get_crypto()
        value_currency = get_currency()
        value_finance = get_finance()
        self.crypto.add_value(value_crypto)
        self.currency.add_value(value_currency)
        self.finance.add_value(value_finance)
    
    def select_all(self):
        crypto = self.cursor.execute(f"SELECT * FROM Crypto WHERE created_on = Date('now')").fetchall()
        finance = self.cursor.execute(f"SELECT * FROM Finance WHERE created_on = Date('now')").fetchall()
        currency = self.cursor.execute(f"SELECT * FROM Currency WHERE created_on = Date('now')").fetchall()
        return crypto,finance,currency


        



if __name__ == "__main__":
    db = DatabaseAdapter(Crypto,Finance,Currency)
    if db.select() == []:
        db.add()

    print(db.select_all())