from employees import add_employee, remove_employee, show_employees
from cars import add_car, remove_car, show_cars
from sales import add_sale, remove_sale, show_sales
from reports import *
from file_manager import save_data, load_data

while True:
    print("\n" + "-" * 40)
    print("1. Додати працівника")
    print("2. Видалити працівника")
    print("3. Переглянути працівників")
    print("4. Додати автомобіль")
    print("5. Видалити автомобіль")
    print("6. Переглянути автомобілі")
    print("7. Додати продаж")
    print("8. Видалити продаж")
    print("9. Переглянути всі продажі")
    print("10. Звіт: всі продажі")
    print("11. Звіт: продажі за дату")
    print("12. Звіт: продажі за працівником")
    print("13. Звіт: сумарний прибуток")
    print("14. Звіт: найпопулярніше авто")
    print("15. Звіт: найуспішніший продавець")
    print("16. Зберегти дані")
    print("17. Завантажити дані")
    print("0. Вихід")

    choice = input("Ваш вибір: ")

    if choice == "1":
        add_employee()
    elif choice == "2":
        remove_employee()
    elif choice == "3":
        show_employees()
    elif choice == "4":
        add_car()
    elif choice == "5":
        remove_car()
    elif choice == "6":
        show_cars()
    elif choice == "7":
        add_sale()
    elif choice == "8":
        remove_sale()
    elif choice == "9":
        show_sales()
    elif choice == "10":
        report_all_sales()
    elif choice == "11":
        report_by_date()
    elif choice == "12":
        report_by_employee()
    elif choice == "13":
        report_profit()
    elif choice == "14":
        report_top_car()
    elif choice == "15":
        report_top_seller()
    elif choice == "16":
        save_data()
    elif choice == "17":
        load_data()
    elif choice == "0":
        print("До побачення!")
        break
    else:
        print("Невірний вибір!")
