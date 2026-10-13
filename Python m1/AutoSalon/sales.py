from employees import employees
from cars import cars

# Список для зберігання усіх об'єктів продажів
sales = []

# Клас Sale зберігає інформацію про продаж: працівник, авто, дата, ціна
class Sale:
    def __init__(self, employee, car, date, real_price):
        self.employee = employee
        self.car = car
        self.date = date
        self.__real_price = real_price

    @property
    def real_price(self):
        return self.__real_price

    @real_price.setter
    def real_price(self, value):
        if value >= 0:
            self.__real_price = value
        else:
            self.__real_price = 0

    # Виводить інформацію про конкретний продаж
    def display(self):
        print("-" * 40)
        print(f"Працівник: {self.employee.full_name}")
        print(f"Автомобіль: {self.car.model} ({self.car.brand})")
        print(f"Дата продажу: {self.date}")
        print(f"Ціна продажу: {self.real_price}")

# Додає новий продаж до списку sales
def add_sale():
    emp_name = input("ПІБ працівника: ")
    car_model = input("Модель авто: ")
    date = input("Дата продажу (рррр-мм-дд): ")
    real_price = float(input("Реальна ціна продажу: "))

    # Пошук працівника за ПІБ
    emp = None
    for e in employees:
        if e.full_name == emp_name:
            emp = e
            break
    if emp is None:
        print("Працівника не знайдено.")
        return

    # Пошук автомобіля за моделлю
    car = None
    for c in cars:
        if c.model == car_model:
            car = c
            break
    if car is None:
        print("Автомобіль не знайдено.")
        return

    # Створення об'єкта продажу і додавання до списку
    sale = Sale(emp, car, date, real_price)
    sales.append(sale)
    print("Продаж додано!")

# Видаляє продаж за датою (перший, який знайде)
def remove_sale():
    date = input("Введіть дату продажу для видалення: ")
    for i in range(len(sales)):
        if sales[i].date == date:
            sales.pop(i)
            print("Продаж видалено!")
            return
    print("Продаж не знайдено.")

# Виводить усі продажі зі списку
def show_sales():
    if len(sales) == 0:
        print("Продажів немає.")
    for s in sales:
        s.display()
