#!/bin/bash

set -e

if ! command -v yc &> /dev/null; then
    echo "Ошибка: Yandex Cloud CLI не установлен"
    exit 1
fi

confirm() {
    read -r -p "${1} [y/N] " response
    case "$response" in
        [yY][eE][sS]|[yY]) return 0 ;;
        *) return 1 ;;
    esac
}

echo "=== Очистка ресурсов Yandex Cloud ==="
confirm "Продолжить?" || exit 0

echo "Удаление объектов из бакетов..."
for bucket in $(yc storage bucket list --format json | jq -r '.[].name'); do
    echo "Очистка бакета: $bucket"
    yc storage s3 rm "s3://$bucket/" --recursive || echo "Бакет $bucket пуст или не существует"
    yc storage bucket delete "$bucket" || echo "Не удалось удалить бакет $bucket"
done

echo "Удаление виртуальных машин..."
for vm in $(yc compute instance list --format json | jq -r '.[].id'); do
    echo "Удаление VM: $vm"
    yc compute instance delete "$vm" --async
done

echo "Удаление сервисных аккаунтов..."
for sa in $(yc iam service-account list --format json | jq -r '.[].id'); do
    echo "Удаление SA: $sa"
    yc iam service-account delete "$sa"
done

echo "✅ Очистка завершена!"