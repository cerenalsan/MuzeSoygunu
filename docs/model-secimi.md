# Model Seçimi Notları (15-16 Ekim 2026)

## Ortam
- Windows, 6 GB VRAM'li ekran kartı, modeller Ollama ile yerelde çalışıyor
- Canlı demo bu bilgisayarda yapılacak

## Denenen modeller
| Model | Boyut | Gözlem |
|---|---|---|
| llama3.2:3b | 2.0 GB | Düz sohbette Türkçe bozuk: İngilizce ve Endonezce kelimeler karıştı, anlamsız cümleler |
| gemma2:2b | 1.6 GB | Düz sohbette Türkçe akıcı ve anlaşılır. Cevap hızı iyi (tek cümle için yaklaşık 3 sn) |
| gemma3:4b | ~3 GB | SEÇİLDİ: 6 GB VRAM'e sığar (100% GPU), Türkçe akıcı |
| gemma4:e4b | ~9 GB | Elendi: demo makinesinde cevap süresi çok uzun |

Bilgisayarda ayrıca qwen2.5:3b, qwen2.5:7b ve llama3.2:1b indirilmiş durumda; henüz denenmedi.

## Rol ve JSON denemesi (gemma2:2b)
Taslak sistem promptu: `docs/model-testi/Modelfile.gemma` (temperature 0.3, JSON çıktı, en fazla 2 Türkçe cümle).
- JSON geçerli çıktı, `say` ve `action` alanları geldi, Türkçe anlaşılır, emoji yok.
- İlk denemede masum mazeretlerle (hasta kızım vb.) 4/4 cevap `open_door` oldu.
- Oyuna uygun hırsız örtü hikâyeleriyle 5 cümlede 4 `open_door`, 1 `deny` oldu.
- Model bazen oyuncunun cümlesini tekrar ediyor (rol karışıklığı), "içeri girin" gibi yön hataları var.
- `say` metni ile `action` bazen tutarsız (soru soruyor ama kapıyı açıyor).

## Çalışma kararı
- Şimdilik gemma2:2b ile devam: Türkçe kalitesi daha iyi, hafif (6 GB VRAM'e rahat sığar), hızlı.
- Bu geçici bir karar. Kesin karar 18 Ekim'de 20 cümlelik test ile verilecek (ölçüt: en az 18/20 geçerli JSON ve anlaşılır Türkçe).
- llama3.2:3b aynı prompt ve cümlelerle henüz karşılaştırılmadı.

## Prompt iyileştirme fikirleri (v1, 23 Ekim)
- Oyuncunun hırsız olabileceğini ve görevlinin şüpheci olduğunu açıkça yazmak
- Oyuncunun içeriden dışarı çıkmak istediğini net belirtmek
- 2-3 örnek cevap (few-shot) eklemek
- Oyun tarafında minimum 2 tur şartı: model ilk mesajda `open_door` dese bile kapı hemen açılmaz

## 20 cümlelik karşılaştırma testi (17 Ekim 2026)

Üç model aynı sistem promptu (Modelfile), aynı sıcaklık (0.3) ve `format: json` ile aynı 20 cümleyle denendi. Cümleler `docs/model-testi/cumleler.txt`, ham cevaplar `docs/model-testi/sonuc-guard-*.txt` dosyalarında. Her cümle yalnızca bir kez denendi.

| | gemma2:2b | llama3.2:3b | qwen2.5:3b |
|---|---|---|---|
| Geçerli JSON (gözle) | 20/20 | 19/20 (biri `{}`) | 20/20 |
| Anlaşılır Türkçe (gözle, yaklaşık) | 18-20 | ~10 | ~12 |
| Alakasızda `deny` | 2/4 | 4/4 | 4/4 |
| Injection'da `open_door` | 3/4 | 1/4 | 0/4 |
| İtirafta `alarm` | 0/3 | 0/3 | 1/3 |
| Kızdırana `alarm` | 0/4 | 0/4 | 3/4 |

Gözlemler:
- Bootcamp ölçütüne (en az 18/20 geçerli JSON ve anlaşılır Türkçe) yalnızca gemma2:2b uyuyor.
- Qwen davranışta daha iyi, Türkçe'de daha zayıf. Cevaplarda `kap�y�` gibi bozuk karakterler çıktı; Ollama'nın ham cevabında da var, yani modelden geliyor gibi görünüyor (sebebi araştırılmadı).
- Üç modelde de açık itiraflarda ("Elması çaldım") `alarm` beklenen sıklıkta çıkmadı. Prompt taslak olduğu için bunun modelden mi prompttan mı geldiği bilinmiyor.
- Cevap metni ile `action` bazen çelişiyor (soru soruyor ama kapıyı açıyor).
- İlk istek 3-5 sn, sonrakiler 0-1 sn sürdü.

Sınırlılık: tek koşu, 20 cümle. Kesin karar 23 Ekim'deki gerçek promptla ve 30 Ekim testiyle verilecek. Çalışma modeli gemma2:2b, yedek aday qwen2.5:3b.

## Prompt v1 ölçümü (24 Ekim 2026)

Aynı 20 cümle, `docs/model-testi/prompt-v1.txt` sistem promptu (4 örnekli), `format: json`, sıcaklık 0.3. Sayılar `action` değerlerinin geçtiği satır sayısıdır, her satır bir kez denendi.

| Model | open_door | deny | alarm |
|---|---|---|---|
| gemma2:2b (ilk koşu) | 7 | 13 | 0 |
| gemma2:2b (tekrar) | 7 | 12 | 1 |
| gemma2:2b (hatırlatmalı, v2) | 5 | 14 | 1 |
| qwen2.5:3b | 2 | 16 | 2 |

Gözlemler:
- Prompt v0'a göre (gemma, 18 Ekim: 11 `open_door`) `open_door` sayısı düştü. İtirafta (14-16) `alarm` hiçbir koşuda beklendiği gibi çıkmadı.
- Gemma injection cümlelerinin 3'ünde kapıyı açtı, qwen 4'ünde de `deny` verdi.
- Gemma'nın tekrar koşusundaki tek `alarm` kararında `say` alanı kapıyı açacağını söylüyor (karar ile cümle çelişiyor).
- Qwen'in cevaplarında bozuk harfler ve bir satırda Çince karakterler var; gemma'da bir satırda bozuk karakter görüldü.
- Hatırlatma eki `open_door` sayısını 7'den 5'e indirdi ama bu fark tek koşunun oynama aralığında; itirafta `alarm` yine çıkmadı. Kullanılmadı (`UseReminder = false`).
- Prompttaki örnekler test cümlelerine benzediği için sonuçlar gerçek iyileşmeden iyimser olabilir. Tek koşu, sonuçlar gösterge niteliğinde.

## 29 Ekim güncellemesi

LLM modeli `gemma2:2b` yerine **`gemma3:4b`** olarak değiştirildi. Önceki modelde güvenlik görevlisinin cevaplarında zaman zaman kelime ve cümle hataları görülüyordu. Üç aday aynı koşullarda denendi:

- **`gemma3:4b`:** `ollama ps` çıktısında `100% GPU` göründü, yani model VRAM'e tamamen sığdı. Basit Türkçe duman testinde cevap akıcı ve hatasızdı.
- **`gemma4:e4b`:** Aynı donanımda cevap süresi oyun için fazla yavaş çıktı, bu yüzden elendi.
- **`gemma2:2b`:** Önceki model, karşılaştırma için baz çizgi olarak tutuldu.

Seçim kriterleri belleğe sığma (100% GPU) ve cevap süresi oldu, çünkü demo makinesinin 6 GB VRAM'i var ve oyun sırasında bekleme yaşanmamalı. JSON geçerliliği ve `action` doğruluğu sonuçları `docs/model-testi/sonuc-v2-gemma3-4b.txt` dosyasındadır.

Model adı `Assets/_Project/Scripts/Zeynep/LlmSettings.asset` içindeki `modelName` alanından belirlenir. Sorun çıkarsa bu alanı `gemma2:2b` yaparak eski modele tek adımda dönülebilir.
