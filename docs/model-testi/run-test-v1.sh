#!/bin/bash
# Kullanım: bash docs/model-testi/run-test-v1.sh gemma2:2b docs/model-testi/prompt-v1.txt v1
MODEL="$1"
PROMPT_FILE="$2"
TAG="$3"
IN="docs/model-testi/cumleler.txt"
OUT="docs/model-testi/sonuc-$TAG-${MODEL//:/-}.txt"
SYS=$(tr -d '\r' < "$PROMPT_FILE" | sed ':a;N;$!ba;s/\\/\\\\/g;s/"/\\"/g;s/\n/\\n/g')
: > "$OUT"
n=0
while IFS= read -r line || [ -n "$line" ]; do
  line="${line%$'\r'}"
  n=$((n+1))
  body="{\"model\":\"$MODEL\",\"stream\":false,\"format\":\"json\",\"options\":{\"temperature\":0.3},\"messages\":[{\"role\":\"system\",\"content\":\"$SYS\"},{\"role\":\"user\",\"content\":\"$line\"}]}"
  start=$(date +%s)
  resp=$(curl -s --max-time 120 http://127.0.0.1:11434/api/chat -d "$body")
  end=$(date +%s)
  content=$(echo "$resp" | sed -n 's/.*"content":"\(.*\)"},"done".*/\1/p' | sed 's/\\"/"/g; s/\\n/ /g')
  echo "$n | $((end-start)) sn | $line | $content" >> "$OUT"
  echo "$n/20 bitti"
done < "$IN"
echo "--- Özet ($OUT) ---"
echo "open_door: $(grep -c '"action": *"open_door"' "$OUT")"
echo "deny: $(grep -c '"action": *"deny"' "$OUT")"
echo "alarm: $(grep -c '"action": *"alarm"' "$OUT")"
