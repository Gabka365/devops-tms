file = open("exercise2/text.txt", "w", encoding="utf-8")
file.write("Это тестовый файл для домашнего задания по программированию")
file.close()

file = open("exercise2/text.txt", "r", encoding="utf-8")
result = file.readline()
file.close()

print(result)


