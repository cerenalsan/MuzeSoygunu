# LLM Hata Testleri

Sandbox_Zeynep sahnesinde LlmSandboxTester ile, gemma2:2b modeliyle denendi. Mesajlar TurkishLlmErrorMessages sınıfından geliyor.

## 20 Ekim 2026

| Hata türü | Nasıl üretildi | Ekranda görünen mesaj | Gözlem |
|---|---|---|---|
| ModelNotFound | LlmSettings'te Model Name `olmayan-model` yapıldı | [Sistem] Model bulunamadı. Model adını ve yüklü olup olmadığını kontrol edin. | Ollama HTTP 404 döndü: `model 'olmayan-model' not found`. Oyun çökmedi. |
| Timeout | Model bellekten boşaltıldı (`ollama stop`), Timeout Seconds 1 yapıldı | [Sistem] Model zamanında cevap vermedi. Lütfen tekrar deneyin. | `İstek 1 saniyede cevaplanmadı.` Süre ayar dosyasından okunuyor, kodda sabit değil. |
| ServerUnreachable | Ollama kapatıldı | [Sistem] Ollama sunucusuna ulaşılamıyor. Ollama'nın açık olduğundan emin olun. | Mesaj yaklaşık 3 saniyede geldi (17 Ekim'de yaklaşık 10 saniye ölçülmüştü, süreler elle tahmin edildi). Buton tekrar tıklanabilir oldu. |

## Henüz denenmeyenler
- InvalidResponse (geçersiz cevap): ayrıştırıcı birim testlerle kapsandı (9 test), ancak sandbox'ta ayrıştırıcı henüz bağlı değil. GuardNpc yazılınca (21 Ekim) ekranda denenecek.
- İstek sırasında kare süresinin Profiler ile ölçümü.

## 23 Ekim 2026: InvalidResponse gerçek akışta görüldü

03_Cikis sahnesinde, GuardDialogueController ve gerçek Ollama ile: model `"action": "ask"` döndürdü. JsonNpcReplyParser bilinmeyen değeri reddetti, oyuncuya "[Sistem] Modelden anlaşılmayan bir cevap geldi. Lütfen tekrar deneyin." gösterildi, Console'a `InvalidResponse: Bilinmeyen ya da eksik "action" değeri: ask` yazıldı. Oyun çökmedi, sahte NPC cevabı gösterilmedi. Sebep araştırılmadı (v0 prompt'taki "ask a question" ifadesiyle ilgisi test edilmedi).
