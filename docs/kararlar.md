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
