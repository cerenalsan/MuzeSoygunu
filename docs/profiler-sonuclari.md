# Profiler Sonuçları (26 Ekim 2026)

Unity Profiler, Play Mode, Editor'da, 03_Cikis sahnesinde gerçek Ollama (gemma2:2b) ile mesaj gönderilirken kaydedildi. Görüntü: `docs/ekran-goruntuleri/profiler-istek.png`.

## Gözlem
- CPU grafiğinde kare süresinin büyük kısmı Rendering'den geliyor; Scripts belirgin bir yer tutmuyor.
- Birkaç kare 66 ms (15 FPS) civarına çıkıyor, çoğu kare 33 ms'nin altında görünüyor.
- Ölçüm Editor'da ve Profiler kaydı açıkken alındı; bu ikisi kare süresini artırır. Build'de ayrıca ölçülmedi.

## Sınırlılık
- Sıçrama anlarının mesaj isteğiyle aynı ana denk gelip gelmediği bu kayıttan çıkarılamadı, bu yüzden "istek sırasında kare hızı düşmüyor" sonucuna varılmadı. İstek kodu `async/await` ile yazılmıştır ve Ollama kapalıyken ya da yavaşken arayüzün tepki verdiği elle gözlendi (yazı kutusu, "Düşünüyor..." ve buton davranışı).
