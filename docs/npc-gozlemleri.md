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
