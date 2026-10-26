# NPC Gözlemleri (sistem promptu v0 taslak, gemma2:2b)

Sandbox_Zeynep sahnesinde GuardSandboxBridge ile, gerçek Ollama ve gerçek GameStateManager ile denendi.

## 22 Ekim 2026

| Deneme | Sonuç |
|---|---|
| İnandırıcı mesaj (bakım teknisyeni) | `open_door`: aşama Won, kapı açık |
| Alakasız mesaj art arda | `deny`: ret sayacı 1, 2, 3 arttı, 4. retten sonra aşama Lost, sonra "Diyalog sona erdi" |
| Hakaret ve itiraf cümleleri (3 deneme) | Üçünde de `deny`, `alarm` hiç gelmedi. Aşama GuardDialogue, ret 3/3, kapı kapalı |

## Gözlemler
- Model bazen oyuncunun cümlesini tekrar ediyor (güvenlik görevlisi gibi konuşmuyor).
- `alarm` kararı ekranda hiç görülmedi. Kod yolu (AlarmAction, Lose(GuardAlarm)) birim testle doğrulandı, ancak model bu kararı üretmedi.
- Prompt taslak (v0). 23 Ekim'de örneklerle cilalanıp aynı cümlelerle yeniden ölçülecek.

## 25 Ekim 2026 (prompt v1, konuşma geçmişi, tur sınırı)

03_Cikis sahnesinde gerçek Ollama (gemma2:2b) ile:
- Geçmiş: "Adım Ahmet." sonrası "Benim adım neydi?" sorusuna güvenlik Ahmet adıyla karşılık verdi (önceki mesaj modele gidiyor).
- "Hırsızlık yaptım" cümlesine `alarm` kararı geldi (22 Ekim'deki v0 denemelerinde hiç `alarm` görülmemişti).
- Tur sınırı (10 oyuncu mesajı) ekranda denendi: 11. mesajda "[Sistem] Güvenlik görevlisi sabrını yitirdi..." çıktı ve 04_Sonuc açıldı, kayıp nedeni "seni dışarı bırakmadı" yazdı.
- Cevap kalitesi zayıf: konuşma ilerledikçe güvenlik aynı soruyu tekrarlıyor ("neden buradasın?"), oyuncu adını söyleyince de aynı cümleyi sürdürüyor. Sebebi araştırılmadı (modelin kendi önceki cevaplarını kopyalaması ya da prompt örneklerinin etkisi olabilir, test edilmedi).
- Tek koşu denemeler; model davranışı tutarsız kalmaya devam ediyor (20 cümlelik ölçümler `model-secimi.md`'de).
