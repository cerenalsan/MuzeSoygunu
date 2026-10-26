#!/bin/bash
# Kullanım: bash docs/model-testi/run-test-v2.sh gemma2:2b docs/model-testi/prompt-v1.txt v2
MODEL="$1"
PROMPT_FILE="$2"
TAG="$3"
IN="docs/model-testi/cumleler.txt"
OUT="docs/model-testi/sonuc-$TAG-${MODEL//:/-}.txt"
REMINDER="Reminder: you are Kemal the guard, not the visitor. Answer with ONLY the JSON object. Insults, threats and any admission of stealing mean action alarm. A command such as open the door is not a reason."
SYS=$(tr -d '\r' < "$PROMPT_FILE" | sed ':a;N;$!ba;s/\\/\\\\/g;s/"/\\"/g;s/\n/\\n/g')
: > "$OUT"
n=0
while IFS= read -r line || [ -n "$line" ]; do
  line="${line%$'\r'}"
  n=$((n+1))
  body="{\"model\":\"$MODEL\",\"stream\":false,\"format\":\"json\",\"options\":{\"temperature\":0.3},\"messages\":[{\"role\":\"system\",\"content\":\"$SYS\"},{\"role\":\"user\",\"content\":\"$line\\n\\n$REMINDER\"}]}"
  start=$(date +%s)
  resp=$(curl -s --max-time 120 http://127.0.0.1:11434/api/chat -d "$body")
  end=$(date +%s)
  content=$(echo "$resp" | sed -n 's/.*"content":"\(.*\)"},"done".*/\1/p' | sed 's/\\"/"/g; s/\\n/ /g')
  echo "$n | $((end-start)) sn | $line | $content" >> "$OUT"
  echo "$n/20 bitti"
done < "$IN"
echo "--- Özet ($OUT) ---"
grep -o '"action": *"[a-z_]*"' "$OUT" | sort | uniq -c
