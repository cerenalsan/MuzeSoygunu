# Kararlar

Önemli tasarım ve süreç kararları. Her kayıtta tarih ve kararı veren yazılır.

## 16 Ekim 2026

- **IToggleable yalnızca lazerler içindir; çıkış kapısı için IOpenable eklendi.**
  "Hepsini kapat" çağrısı lazerler için güvenlik, kapı için kilitlenme demek olurdu (Liskov ihlali).
  IToggleable: IsOn, TurnOn, TurnOff. IOpenable: IsOpen, Open, Close. Bootcamp §4.2'den sapma.
  Öneren: Ceren, uygulayan: Zeynep.
- **IGameState'e plan dışı LastLoseReason alanı eklendi.**
  Sonuç ekranı kaybetme nedenini bu alandan okuyacak. Ekleyen: Ceren.
- **GameStateManager oyun açılırken kendi kendine oluşuyor (Bootstrap).**
  Hiçbir sahneye elle eklemek gerekmiyor, böylece sahne sahipliği çakışması olmuyor. Yazan: Ceren.
- **Branch düzeni: konu başına feature/... yerine kişi başına tek branch (ceren, zeynep).**
  Her PR küçük tutulacak (bir iş, bir PR); birleşince branch develop ile güncellenecek.

## 17 Ekim 2026

- **TextMesh Pro Essentials yalnızca Zeynep tarafından import edilir ve commit'lenir; diğer kişi import etmez, develop'tan çeker.**
  Sebep: aynı .meta dosyalarının iki yerde oluşup çakışması. "TMP Examples & Extras" ağır ve gereksiz olduğu için import edilmez.
  Ceren'in sahnelerinde doğrudan yazı gerekmez: ipucu metinleri (ör. "E ile etkileşim") IInteractable üzerinden Zeynep'in HUD'ında gösterilir.

## 18 Ekim 2026

- **Risk 2 (yerel LLM): GO. Çalışma modeli gemma2:2b.**
  Üç model (gemma2:2b, llama3.2:3b, qwen2.5:3b) aynı sistem promptu ve 20 cümleyle denendi (`docs/model-secimi.md`). Bootcamp ölçütünü (en az 18/20 geçerli JSON ve anlaşılır Türkçe) yalnızca gemma2:2b karşıladı. Unity'den Ollama'ya asenkron istek çalıştı, Ollama kapalıyken Unity donmadı.
  Açık risk: `action` kararları (itirafta alarm, prompt injection) üç modelde de zayıf; prompt taslak olduğu için nedeni bilinmiyor. 23 Ekim'deki gerçek promptla tekrar ölçülecek, yedek aday qwen2.5:3b (Türkçe daha zayıf, davranış daha iyi).
  Not: Gemma'nın sohbet şablonunda ayrı bir system rolü yok, sistem promptu ayrı bir user turu olarak gönderiliyor (`ollama show gemma2:2b --template`). Rolden çıkma gözlemiyle ilişkisi test edilmedi.

## 21 Ekim 2026

- **Scripts/Shared klasörüne MuzeSoygunu.Shared assembly definition eklendi.**
  Zeynep'in MuzeSoygunu.Llm assembly'sinin IGameState gibi ortak arayüzlere erişebilmesi için gerekti. Koşullar (Ceren): yalnızca asmdef ve .meta eklenir, hiçbir script taşınmaz ya da değişmez; Shared'da yalnızca arayüz ve enum kalır; Llm asmdef Shared'a referans verir. GameStateManager Assembly-CSharp'ta kaldığı için Llm içinden erişilemez: GuardNpc IGameState'i constructor ile dışarıdan alır, bağlantıyı Assembly-CSharp'taki bir sahne bileşeni yapar.

## 25 Ekim 2026

- **Konuşma geçmişi son 6 mesaj (3 tur), oyuncu tur sınırı 10.** Sınır aşılınca oyuncu `GuardRefused` ile kaybeder ve Ollama'ya istek atılmaz. Başarısız istekler geçmişe ve tur sayacına yazılmaz. Geçmişe modelin ham JSON cevabı yazılır.
- **OllamaClient isteğe `format: json` ekliyor.** Test scriptlerimiz ekliyordu, oyundaki istemci eklemiyordu.
- **Sistem promptu v1 `GuardPromptBuilder`'da.** Hatırlatma eki ölçümde belirgin fayda göstermediği için kapalı (`UseReminder = false`). Prompt dondurma 29 Ekim'de.

## 28 Ekim 2026

- **Model ısıtma:** 03_Cikis'e girince boş bir sohbet isteğiyle model belleğe yüklenir (`keep_alive` 10 dakika), süre `LlmSettings.WarmupTimeoutSeconds`'tan okunur. Yapılan testlerde ısınma sonrası ilk yanıt yaklaşık 1 saniyede alınmıştır; Ollama kapalı olduğunda ise sistem "ısınma başarısız" uyarısı verip kullanıcının Ollama'yı açmasını bildirir. Başarısızlıklar sessizce ele alınır, Ollama sorunları sağlık kontrolü ve istek hatalarıyla yönetilir.
- **Ekran ipuçları:** Ceren'in sahnelerine dokunmadan, oyun açılırken kendiliğinden oluşan `HudHints` bileşeniyle çalışır (aşamaya göre metin, `HintTexts`).

## Karar: LLM modeli gemma3:4b olarak belirlendi

## 29 Ekim 2026

- **Bağlam**
Önceki model `gemma2:2b` Türkçe'de kelime ve cümle hataları yapıyordu. Daha iyi aday arandı.

- **Denenen adaylar**
- `gemma3:4b`: demo makinesinde (6 GB VRAM) 100% GPU, Türkçe akıcı.
- `gemma4:e4b`: demo makinesinde çok yavaş, elendi.
- `gemma2:2b`: önceki model, baz çizgi olarak tutuldu.

- **Karar**
`gemma3:4b` kullanılacak. Model adı `LlmSettings.asset` içindeki `modelName` alanından değişir. Geri dönüş tek satırdır.

- **Sonuçlar**
JSON geçerliliği, `action` doğruluğu ve süre ölçümleri `docs/model-testi/sonuc-v2-gemma3-4b.txt` dosyasındadır.

## 1 Kasım 2026

- **Bağlam**

Build testinde güvenlik görevlisi karar vermeyip sürekli soru soruyordu. "Annem hastanede" gibi makul sebeplerde bile reddediyordu ve prompt'taki ilk örnek cümleyi aynen kopyalıyordu. Oyun kazanılabilir olmalıydı.

- **Denenen adaylar**
  - `v1`: `open_door` için "kim olduğunu, ne yaptığını, kim gönderdiğini" şartı koşuyordu. Soru sormanın sınırı yoktu.
  - `v2`: kuralları genişletildi, soru sayısı sınırlandı, örnekler çeşitlendirildi.

- **Karar**

Prompt `v2` kullanılacak. `open_door` için aile acili, kaza, hastalık, iş görevi gibi makul sebepler yeterli. Konuşma boyunca en fazla bir soru sorulur, aynı soru tekrarlanmaz. Sıkılma gibi zayıf sebepler reddedilir, tehdit ve itiraf alarm verir. JSON şeması (`say`, `action`) değişmedi. Geri dönüş: `GuardPromptBuilder.cs` içinde `Version` ve metin `v1`'e alınır.

- **Sonuçlar**

Metin `docs/model-testi/prompt-v2.txt` dosyasındadır. Kazanma ve reddetme senaryolarının sonuçları test sonrası eklenecek.
