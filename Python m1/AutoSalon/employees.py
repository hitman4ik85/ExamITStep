import re

employees = []

class Employee:
    def __init__(self, full_name, position, phone, email):
        self.__full_name = full_name
        self.__position = position
        self.__phone = phone
        self.__email = email

    @property
    def full_name(self):
        return self.__full_name

    @full_name.setter
    def full_name(self, value):
        if value.strip() != "":
            self.__full_name = value
        else:
            self.__full_name = "Невідомо"

    @property
    def position(self):
        return self.__position

    @position.setter
    def position(self, value):
        self.__position = value

    @property
    def phone(self):
        return self.__phone

    @phone.setter
    def phone(self, value):
        if value.isdigit() and len(value) == 10:
            self.__phone = value
        else:
            self.__phone = "0000000000"

    @property
    def email(self):
        return self.__email

    @email.setter
    def email(self, value):
        regex = r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,7}\b'
        if re.fullmatch(regex, value):
            self.__email = value
        else:
            self.__email = "no@email.com"

    def display(self):
        print("-" * 40)
        print(f"ПІБ: {self.full_name}")
        print(f"Посада: {self.position}")
        print(f"Телефон: {self.phone}")
        print(f"Email: {self.email}")

def add_employee():
    while True:
        full_name = input("ПІБ: ")
        if full_name.strip() != "":
            break
        print("ПІБ не повинен бути порожнім!")

    while True:
        position = input("Посада: ")
        if position.strip() != "":
            break
        print("Посада не повинна бути порожньою!")

    while True:
        phone = input("Телефон: ")
        if phone.isdigit() and len(phone) == 10:
            break
        print("Номер телефону має містити рівно 10 цифр!")

    regex = r'\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,7}\b'
    while True:
        email = input("Email: ")
        if re.fullmatch(regex, email):
            break
        print("Некоректний email! Введіть правильну адресу.")

    emp = Employee(full_name, position, phone, email)
    employees.append(emp)
    print("Працівника додано!")

def remove_employee():
    name = input("Введіть ПІБ для видалення: ")
    phone = input("Введіть номер телефону для підтвердження: ")
    for i in range(len(employees)):
        if employees[i].full_name == name and employees[i].phone == phone:
            employees.pop(i)
            print("Працівника видалено!")
            return
    print("Працівника не знайдено!")

def show_employees():
    if len(employees) == 0:
        print("Список працівників порожній.")
    for emp in employees:
        emp.display()
