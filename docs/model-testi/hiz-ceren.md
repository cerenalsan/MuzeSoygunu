# Model Hız Ölçümü (Ceren'in Bilgisayarı)

**Tarih:** 18 Ekim 2026 | **Sorumlu:** Ceren

## Ortam
- RAM: 24 GB
- İşlemci: yalnızca CPU (`ollama ps` çıktısında iki model de "100% CPU")
- Ollama sürümünün llamacpp çalıştırıcısı, bağlam 4096

## Yöntem
`ollama run <model> --verbose` ile aynı cümle iki kez gönderildi:

> Sen bir müze muhafızısın. Sadece JSON yaz: {"say": "...", "action": "open_door|deny|alarm"}. Oyuncu: Kapıyı açar mısın, ben temizlik görevlisiyim.

Not: Bu ölçüm yalnızca hız içindir. `format: json` kullanılmadı, cevap kalitesi ölçülmedi (kalite testi Zeynep'in 20 cümlelik tablosunda).

## Sonuçlar

| Model | Deneme | Toplam süre | Üretilen token | Üretim hızı (token/sn) |
|---|---|---|---|---|
| llama3.2:3b | 1 | 2.42 sn | 22 | 15.71 |
| llama3.2:3b | 2 | 2.60 sn | 27 | 15.58 |
| gemma2:2b | 1 | 1.90 sn | 22 | 18.54 |
| gemma2:2b | 2 | 2.22 sn | 22 | 18.25 |

Bellekte kapladığı yer (`ollama ps`): llama3.2:3b 2.6 GB, gemma2:2b 1.9 GB.

## Gözlemler
- Kısa bir JSON cevabı (20-30 token) CPU'da bile yaklaşık 2-2.5 saniyede geliyor. Gemma yaklaşık %15-18 daha hızlı.
- `load duration` birkaç milisaniye çıktı: model önceki testten beri bellekteydi. Soğuk başlangıç (modelin ilk kez yüklenmesi) bu ölçümde yok. Zeynep'in gördüğü ~10 saniyenin model yüklemesinden gelip gelmediği henüz doğrulanmadı.
- Ollama modeli varsayılan olarak 5 dakika kullanılmazsa bellekten çıkarıyor. Oyunda ilk cevabın yavaş gelmemesi için `keep_alive` ve oyun açılışında bir ısınma isteği değerlendirilecek.
- Llama `action` alanına seçenek listesini olduğu gibi kopyaladı (`open_door|deny|alarm`, `deny|alarm`), yani geçersiz değer üretti. Gemma `say` alanına "1" yazdı ve cevabı json kod bloğuna sardı. İkisi de yer tutucu içeren prompt örneğinden etkileniyor olabilir. Gerçek promptta tam bir örnek cevap verilecek (23 Ekim).

## Sonraki adımlar
- Soğuk başlangıç süresini ölç (`ollama stop <model>` sonrası ilk istek).
- 23 Ekim'de gerçek promptla tekrar ölç.
