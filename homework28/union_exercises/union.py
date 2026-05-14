import os
from jinja2 import Environment, FileSystemLoader

#Exercise1
def multiply_numbers(a, b):
    return a * b

print(multiply_numbers(2, 6))

#Exercise2
file = open("union_exercises/text.txt", "w", encoding="utf-8")
file.write("Это тестовый файл для домашнего задания по программированию")
file.close()

file = open("union_exercises/text.txt", "r", encoding="utf-8")
result = file.readline()
file.close()
print(result)

#Exercise3
folder_name = "union_exercises/mydir"
if not os.path.exists(folder_name):
    os.mkdir(folder_name)

os.chdir("union_exercises/mydir")

file1 = open("file1.txt", "w", encoding="utf-8")
file2 = open("file2.txt", "w", encoding="utf-8")
file3 = open("file3.txt", "w", encoding="utf-8")


files = os.listdir(".")
print(files)

#Exercise4
os.chdir("..")
env = Environment(loader=FileSystemLoader("."))

template = env.get_template("template.html")

users = [
    {"name": "Иван", "email": "ivan@example.com"},
    {"name": "Мария", "email": "maria@example.com"},
    {"name": "Павел", "email": "pavel@example.com"},
]

html_output = template.render(users=users)

print(html_output)
