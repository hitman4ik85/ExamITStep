from employees import employees, Employee
from cars import cars, Car
from sales import sales, Sale

# Зберігає всі дані у .txt файли
def save_data():
    # Збереження працівників
    with open("employees.txt", "w", encoding="utf-8") as f:
        for emp in employees:
            f.write(f"{emp.full_name};{emp.position};{emp.phone};{emp.email}\n")

    # Збереження автомобілів
    with open("cars.txt", "w", encoding="utf-8") as f:
        for car in cars:
            f.write(f"{car.brand};{car.model};{car.year};{car.color};{car.cost};{car.price}\n")

    # Збереження продажів (ПІБ працівника, модель авто, дата, ціна)
    with open("sales.txt", "w", encoding="utf-8") as f:
        for s in sales:
            f.write(f"{s.employee.full_name};{s.car.model};{s.date};{s.real_price}\n")

    print("Дані збережено у .txt файли.")

# Завантажує всі дані з .txt файлів у списки
def load_data():
    # Очищення списків перед завантаженням
    employees.clear()
    cars.clear()
    sales.clear()

    # Завантаження працівників
    try:
        with open("employees.txt", "r", encoding="utf-8") as f:
            for line in f:
                parts = line.strip().split(";")
                emp = Employee(parts[0], parts[1], parts[2], parts[3])
                employees.append(emp)
    except:
        print("Файл employees.txt не знайдено.")

    # Завантаження автомобілів
    try:
        with open("cars.txt", "r", encoding="utf-8") as f:
            for line in f:
                parts = line.strip().split(";")
                car = Car(parts[0], parts[1], parts[2], parts[3], float(parts[4]), float(parts[5]))
                cars.append(car)
    except:
        print("Файл cars.txt не знайдено.")

    # Завантаження продажів
    try:
        with open("sales.txt", "r", encoding="utf-8") as f:
            for line in f:
                parts = line.strip().split(";")
                # Пошук працівника по ПІБ
                emp = next((e for e in employees if e.full_name == parts[0]), None)
                # Пошук автомобіля по моделі
                car = next((c for c in cars if c.model == parts[1]), None)
                # Створення об'єкта продажу, якщо працівник і авто знайдені
                if emp and car:
                    sale = Sale(emp, car, parts[2], float(parts[3]))
                    sales.append(sale)
    except:
        print("Файл sales.txt не знайдено.")

    print("Дані завантажено.")