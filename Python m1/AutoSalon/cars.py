cars = []

class Car:
    def __init__(self, brand, model, year, color, cost, price):
        self.brand = brand
        self.model = model
        self.year = year
        self.color = color
        self.__cost = cost
        self.__price = price

    @property
    def cost(self):
        return self.__cost

    @cost.setter
    def cost(self, value):
        if value >= 0:
            self.__cost = value
        else:
            self.__cost = 0

    @property
    def price(self):
        return self.__price

    @price.setter
    def price(self, value):
        if value >= 0:
            self.__price = value
        else:
            self.__price = 0

    def display(self):
        print("-" * 40)
        print(f"Виробник: {self.brand}")
        print(f"Модель: {self.model}")
        print(f"Рік випуску: {self.year}")
        print(f"Колір: {self.color}")
        print(f"Собівартість: {self.cost}")
        print(f"Ціна продажу: {self.price}")

def add_car():
    brand = input("Виробник: ")
    model = input("Модель: ")
    year = input("Рік випуску: ")
    color = input("Колір: ")
    cost = float(input("Собівартість: "))
    price = float(input("Ціна продажу: "))
    car = Car(brand, model, year, color, cost, price)
    cars.append(car)
    print("Автомобіль додано!")

def remove_car():
    model = input("Введіть модель авто: ")
    color = input("Введіть колір авто: ")
    year = input("Введіть рік випуску авто: ")
    for i in range(len(cars)):
        if cars[i].model == model and cars[i].color == color and cars[i].year == year:
            cars.pop(i)
            print("Автомобіль видалено!")
            return
    print("Автомобіль не знайдено.")

def show_cars():
    if len(cars) == 0:
        print("Список автомобілів порожній.")
    for car in cars:
        car.display()
