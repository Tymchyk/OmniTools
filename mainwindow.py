# This Python file uses the following encoding: utf-8
import sys
import csv
import subprocess

from PySide6.QtWidgets import QApplication, QMainWindow
from PySide6.QtCore import QRect,QDate,Qt
from PySide6.QtGui import QTextCharFormat, QTextCursor
from api_package.db import DatabaseAdapter,Crypto,Finance,Currency

# Important:
# You need to run the following command to generate the ui_form.py file
#     pyside6-uic form.ui -o ui_form.py, or
#     pyside2-uic form.ui -o ui_form.py
from ui_form import Ui_MainWindow

class MainWindow(QMainWindow):
    def __init__(self, parent=None):
        super().__init__(parent)
        self.ui = Ui_MainWindow()
        self.ui.setupUi(self)
        self.ui.MenuBig.hide()
        self.ui.Homepage.setChecked(True)
        self.ui.Menu.clicked.connect(self.change_geometry)
        self.ui.Homepage.clicked.connect(lambda: self.ui.stackedWidget.setCurrentIndex(0))
        self.ui.Chats.clicked.connect(lambda: self.ui.stackedWidget.setCurrentIndex(2))
        self.ui.Tasks.clicked.connect(lambda: self.ui.stackedWidget.setCurrentIndex(1))
        self.set_values()
        self.ui.save.clicked.connect(lambda: self.save_task())
        self.ui.read.clicked.connect(lambda: self.read_task())
        self.selected_dates()


    def change_geometry(self):
        print(self.ui.MainPage.geometry())
        if self.ui.MainPage.geometry() == QRect(0, 0, 811, 541):
            self.ui.MainPage.setGeometry(10,0,811,541)
        elif self.ui.MainPage.geometry() == QRect(10, 0, 811, 541):
            self.ui.MainPage.setGeometry(0,0,811,541)
        self.ui.MenuBig.setGeometry(0,80,191,471)

    def set_values(self):
        db = DatabaseAdapter(Crypto,Finance,Currency)
        if db.select() == []:
            db.add()
        values = db.select_all()
        self.ui.name.setText(values[0][0][0].capitalize())
        self.ui.value1.setText(f"$ {values[0][0][1]:,.02f}")
        self.ui.name1.setText(values[0][1][0].capitalize())
        self.ui.value2.setText(f"$ {values[0][1][1]:,.02f}")
        self.ui.finance1.setText(values[1][0][0])
        self.ui.finance_price1.setText(f"$ {values[1][0][1]:,.02f}")
        self.ui.finance2.setText(values[1][1][0])
        self.ui.finance_price2.setText(f"$ {values[1][1][1]:,.02f}")
        self.ui.finance3.setText(values[1][2][0])
        self.ui.finance_price3.setText(f"$ {values[1][2][1]:,.02f}")
        self.ui.finance4.setText(values[1][3][0])
        self.ui.finance_price4.setText(f"$ {values[1][3][1]:,.02f}")
        self.ui.currency1.setText(f"{values[2][0][0]} - UAH")
        self.ui.currency_price1.setText(f"{values[2][0][1]:,.02f}")
        self.ui.currency2.setText(f"{values[2][1][0]} - UAH")
        self.ui.currency_price2.setText(f"{values[2][1][1]:,.02f}")
        self.ui.currency3.setText(f"{values[2][2][0]} - UAH")
        self.ui.currency_price3.setText(f"{values[2][2][1]:,.02f}")

    def save_task(self):
        date = self.ui.calendar.selectedDate()
        date_format = QTextCharFormat()
        date_format.setBackground(Qt.yellow)
        self.ui.calendar.setDateTextFormat(date, date_format)
        date_to_string =f"{date.year()}-{date.month()}-{date.day()}"
        value = self.ui.textEdit.toPlainText().split("\n")
        with open(f"tasks/{date_to_string}.csv", "w") as file:
            writer = csv.writer(file)
            for num,val in enumerate(value, start= 1):
                writer.writerow((num,val))

    def read_task(self):
        try:
            date = self.ui.calendar.selectedDate()
            date_to_string =f"{date.year()}-{date.month()}-{date.day()}"
            with open(f"tasks/{date_to_string}.csv","r") as file:
                reader = csv.reader(file)
                text = ""
                for row in reader:
                    text+=f"{row[0]}. {row[1]}\n"
                self.ui.textEdit.setPlainText(text)
        except FileNotFoundError:
            self.ui.textEdit.setPlainText("You don't add any task yet!")

    def selected_dates(self):
        result = subprocess.run(["ls"], cwd="tasks", stdout=subprocess.PIPE, text=True)
        output_lines = result.stdout.splitlines()
        for line in output_lines:
            year,month,day = map(int,line.replace(".csv","").split("-"))
            date = QDate(year, month, day)
            date_format = QTextCharFormat()
            date_format.setBackground(Qt.yellow)
            self.ui.calendar.setDateTextFormat(date, date_format)






if __name__ == "__main__":
    app = QApplication(sys.argv)
    widget = MainWindow()
    widget.show()
    sys.exit(app.exec())
