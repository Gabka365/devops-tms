import os

folder_name = "exercise3/mydir"
if not os.path.exists(folder_name):
    os.mkdir(folder_name)

os.chdir("exercise3/mydir")

file1 = open("file1.txt", "w", encoding="utf-8")
file2 = open("file2.txt", "w", encoding="utf-8")
file3 = open("file3.txt", "w", encoding="utf-8")


files = os.listdir(".")
print(files)