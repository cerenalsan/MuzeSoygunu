## Entegrasyon Testi Sonuçları

| Test Senaryosu | Kim Oynadı / İzledi | Gözlemlenen Durum / Sonuç | Durum / Sorun Var mı? |
|---|---|---|---|
| 1. Güvenlik ile Sohbet (Normal Akış) | Zeynep (Oynadı) | Model (`gemma3:4b`) Türkçe yanıt verdi, JSON doğru ayrıştırıldı. | Sorun yok (Başarılı) |
| 2. Kapı Açma Denemesi (`open_door`) | Zeynep (Oynadı) | Doğru diyalog sonrasında kapı açıldı, sahne geçişi sağlandı. | Sorun yok (Başarılı) |
| 3. Ret ve Sınır Aşımları (`deny`) | Zeynep (Oynadı) | Ret sayacı arttı, sınır aşımında kaybetme ekranına yönlendirildi. | Sorun yok (Başarılı) |
| 4. Kamera / Lazer / Alarm Tetikleme | Zeynep (Oynadı) | Tehlike alanına girildiğinde alarm tetiklendi, oyun bitti. | Sorun yok (Başarılı) |
| 5. Ollama Kapalı Durumu | Zeynep (Oynadı) | Hata mesajı ve Tekrar Dene düğmesi ekranda göründü, çökme yaşanmadı. | Sorun yok (Başarılı) |

## Özet

- Kazanma denemeleri: 5 denemenin 4'ünde kazanıldı (modelin `open_door` vermesine bağlı).
- Kaybetme yolları: kamera, lazer ve alarm denendi.
- Açık P1 hata sayısı: 0
