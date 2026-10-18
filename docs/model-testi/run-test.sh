#!/bin/bash
# Kullanım: bash docs/model-testi/run-test.sh guard-gemma
MODEL="$1"
IN="docs/model-testi/cumleler.txt"
OUT="docs/model-testi/sonuc-$MODEL.txt"
: > "$OUT"
n=0
while IFS= read -r line || [ -n "$line" ]; do
  line="${line%$'\r'}"
  n=$((n+1))
  body="{\"model\":\"$MODEL\",\"stream\":false,\"format\":\"json\",\"messages\":[{\"role\":\"user\",\"content\":\"$line\"}]}"
  start=$(date +%s)
  resp=$(curl -s --max-time 120 http://127.0.0.1:11434/api/chat -d "$body")
  end=$(date +%s)
  content=$(echo "$resp" | sed -n 's/.*"content":"\(.*\)"},"done".*/\1/p' | sed 's/\\"/"/g; s/\\n/ /g')
  echo "$n | $((end-start)) sn | $line | $content" >> "$OUT"
  echo "$n/20 bitti"
done < "$IN"
