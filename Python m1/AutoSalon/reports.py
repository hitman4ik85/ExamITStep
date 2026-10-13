from sales import sales

# Виводить усі продажі, якщо вони є
def report_all_sales():
    if len(sales) == 0:
        print("Продажів немає.")
    for s in sales:
        s.display()

# Виводить продажі за вказаною датою
def report_by_date():
    date = input("Введіть дату (рррр-мм-дд): ")
    found = False
    for s in sales:
        if s.date == date:
            s.display()
            found = True
    if not found:
        print("Продажів на цю дату не знайдено.")

# Виводить продажі конкретного працівника
def report_by_employee():
    name = input("Введіть ПІБ працівника: ")
    found = False
    for s in sales:
        if s.employee.full_name == name:
            s.display()
            found = True
    if not found:
        print("Продажів цього працівника не знайдено.")

# Підраховує прибуток за вказаний період
def report_profit():
    start = input("Початкова дата (рррр-мм-дд): ")
    end = input("Кінцева дата (рррр-мм-дд): ")
    total = 0
    for s in sales:
        if start <= s.date <= end:
            total += s.real_price - s.car.cost
    print(f"Сумарний прибуток: {total}")

# Визначає найпопулярнішу модель авто за вказаний період
def report_top_car():
    start = input("Початкова дата (рррр-мм-дд): ")
    end = input("Кінцева дата (рррр-мм-дд): ")
    stat = {}
    for s in sales:
        if start <= s.date <= end:
            model = s.car.model
            if model in stat:
                stat[model] += 1
            else:
                stat[model] = 1
    if len(stat) == 0:
        print("Продажів немає.")
        return
    top_model = max(stat, key=stat.get)
    print(f"Найпопулярніший автомобіль: {top_model} ({stat[top_model]} продажів)")

# Визначає найуспішнішого працівника за сумою продажів
def report_top_seller():
    start = input("Початкова дата (рррр-мм-дд): ")
    end = input("Кінцева дата (рррр-мм-дд): ")
    stat = {}
    for s in sales:
        if start <= s.date <= end:
            name = s.employee.full_name
            if name in stat:
                stat[name] += s.real_price
            else:
                stat[name] = s.real_price
    if len(stat) == 0:
        print("Продажів немає.")
        return
    top_name = max(stat, key=stat.get)
    print(f"Найуспішніший продавець: {top_name} (на суму {stat[top_name]})")
