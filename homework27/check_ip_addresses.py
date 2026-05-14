import subprocess

def get_list_of_ip_adresses():
    with open("list_ip_adresses.txt", "r") as file:
        return [line.strip() for line in file.readlines()]

def get_ping_results(lines):
    results = []
    for ip in lines:
        result = subprocess.run(["ping", "-n", "1", ip], capture_output=True, text=True)
        results.append(f"Target {ip} return code: {result.returncode}")
    return results

def save_results(results):
    with open('data.txt', 'w', encoding="utf-8") as file:
        for result in results:
            file.write(result + '\n')

lines = get_list_of_ip_adresses()

results = get_ping_results(lines)

save_results(results)

