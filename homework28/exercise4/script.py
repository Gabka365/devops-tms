from jinja2 import Environment, FileSystemLoader
import os

os.chdir("exercise4")

env = Environment(loader=FileSystemLoader("."))

template = env.get_template("template.html")

users = [
    {"name": "Иван", "email": "ivan@example.com"},
    {"name": "Мария", "email": "maria@example.com"},
    {"name": "Павел", "email": "pavel@example.com"},
]

html_output = template.render(users=users)

print(html_output)
