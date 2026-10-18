# Model Seçimi Notları (15-16 Ekim 2026)

## Ortam
- Windows, 6 GB VRAM'li ekran kartı, modeller Ollama ile yerelde çalışıyor
- Canlı demo bu bilgisayarda yapılacak

## Denenen modeller
| Model | Boyut | Gözlem |
|---|---|---|
| llama3.2:3b | 2.0 GB | Düz sohbette Türkçe bozuk: İngilizce ve Endonezce kelimeler karıştı, anlamsız cümleler |
| gemma2:2b | 1.6 GB | Düz sohbette Türkçe akıcı ve anlaşılır. Cevap hızı iyi (tek cümle için yaklaşık 3 sn) |

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
