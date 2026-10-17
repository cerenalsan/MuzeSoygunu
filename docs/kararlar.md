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
