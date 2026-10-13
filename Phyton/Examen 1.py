from pymongo import MongoClient

connect = MongoClient("mongodb+srv://hitman4ik:namtih4ik@hitokbd.gshfkix.mongodb.net/?retryWrites=true&w=majority&appName=hitokBD")
db = connect["NewDB"]
categories_collection = db['categories']
products_collection = db['products']

def add_category():
    category_name = input("\nВведіть назву нової категорії: ")
    categories_collection.insert_one({"name": category_name})
    print("Категорія успішно додана.")

def get_all_categories():
    categories = categories_collection.find()
    print("Категорії товарів:\n")
    for category in categories:
        print(category['name'])

def delete_category():
    category_name = input("\nВведіть назву категорії для видалення: ")
    category = categories_collection.find_one({"name": category_name})
    if category:
        categories_collection.delete_one({"name": category_name})
        print("Категорія успішно видалена.")
    else:
        print("Не вірно введена назва категорії.")

def add_product():
    try:
        category_name = input("\nВведіть назву категорії товару: ")
        if not categories_collection.find_one({"name": category_name}):
            raise Exception("Невірно введена назва категорії")
        
        product_name = input("Введіть назву товару: ")
        if not product_name:
            raise Exception("Назва товару не може бути пустою")

        product_description = input("Введіть опис товару: ")
        if not product_description:
            raise Exception("Опис товару не може бути пустим")

        product_quantity = int(input("Введіть кількість товару: "))
        if product_quantity <= 0:
            raise Exception("Кількість товару повинна бути більше 0")

        products_collection.insert_one({
            "category": category_name,
            "name": product_name,
            "description": product_description,
            "quantity": product_quantity
        })
        print("Товар успішно додано до категорії.")
    except ValueError:
        print("Введено некоректне значення для кількості товару.")
    except Exception as e:
        print(e)

def get_products_by_category():
    category_name = input("\nВведіть назву категорії: ")
    category = categories_collection.find_one({"name": category_name})
    if category:
        products = products_collection.find({"category": category_name})
        print(f"Товари у категорії '{category_name}':\n")
        for product in products:
            print(f"{product['name']}")
    else:
        print("Не вірно введено назву категорії.")

def view_product_info():
    category_name = input("\nВведіть назву категорії: ")
    product_name = input("Введіть назву товару: ")
    product = products_collection.find_one({"category": category_name, "name": product_name})
    if product:
        print(f"\nНазва: {product['name']}")
        print(f"Опис: {product['description']}")
        print(f"Кількість: {product['quantity']} штук")
    else:
        print("Такого товару не знайдено.")

def delete_product():
    try:
        category_name = input("\nВведіть назву категорії: ")
        product_name = input("Введіть назву товару: ")

        category = categories_collection.find_one({"name": category_name})
        product = products_collection.find_one({"category": category_name, "name": product_name})

        if not category:
            raise Exception("Не вірно введена назва категорії")
        elif not product:
            raise Exception("Не вірно введена назва товару")

        products_collection.delete_one({"category": category_name, "name": product_name})
        print("Товар успішно видалено з категорії.")
    except Exception as e:
        print(e)

def main():
    while True:
        print("\nМеню:")
        print("1. Переглянути всі категорії товарів")
        print("2. Додати нову категорію товару")
        print("3. Видалити категорію")
        print("4. Додати новий товар до категорії")
        print("5. Видалити товар з категорії")
        print("6. Переглянути всі товари в категорії")
        print("7. Переглянути повну інформацію про товар у категорії")
        print("8. Вийти з програми\n")

        choice = input("Виберіть опцію: ")

        if choice == "1":
            get_all_categories()
        elif choice == "2":
            add_category()
        elif choice == "3":
            delete_category()
        elif choice == "4":
            add_product()
        elif choice == "5":
            delete_product()
        elif choice == "6":
            get_products_by_category()
        elif choice == "7":
            view_product_info()
        elif choice == "8":
            print("До побачення!")
            break
        else:
            print("Неправильний вибір. Будь ласка, виберіть опцію зі списку.")

if __name__ == "__main__":
    main()