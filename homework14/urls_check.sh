#!/bin/bash

INPUT_FILE="${1:-urls.txt}"
MAX_PARALLEL=2

declare -a PIDS

process_url(){
    	local url="$1"
    	echo "проверка: $url"
    	if [ -n "$RESULT" ]; then
    		echo "результат: $RESULT"
	fi
	sleep 1
	echo "стратус: OK"
}

if [ ! -f "$INPUT_FILE" ]; then
	echo "Файл не найден: $INPUT_FILE" >&2
	exit 1
fi

count=0

while IFS= read -r line; do
	line="$(echo ${line})"
	if [ -z "$line" ]; then
		continue
	fi
	process_url "$line" &
	PIDS+=($!)
	count=$((count+1))
	if [ "$count" -ge "$MAX_PARALLEL" ]; then
		wait "${PIDS[0]}"
		PIDS=("${PIDS[@]:1}")
	count=$((count-1))
	fi
done < "$INPUT_FILE"

wait "${PIDS[@]}"
echo "Готово."
