# Kullanılan asset'ler

Bu dosya, projede kullanılan üçüncü taraf görsel, ses ve diğer asset'lerin kaynağını ve lisansını listeler.

## Görseller

| Asset | Kaynak | Lisans | Nerede kullanıldı | Dosya |
|---|---|---|---|---|
| Kenney Topdown Shooter (tilesheet) | https://opengameart.org/content/topdown-shooter (Kenney, kenney.nl) | CC0 (Public Domain) | Salon 1 ve Salon 2 zemin döşemesi | `Assets/_Project/Art/Kenney/TopdownShooter/tilesheet_complete.png` |
| Kenney Topdown Shooter (Hitman 1) | aynı paket | CC0 | Oyuncu (hırsız) sprite'ı | `Assets/_Project/Art/Kenney/TopdownShooter/Karakterler/hitman1_stand.png` |
| Kenney Topdown Shooter (Man Blue) | aynı paket | CC0 | Projede bulunuyor, oyunda kullanılmıyor | `Assets/_Project/Art/Kenney/TopdownShooter/Karakterler/manBlue_stand.png` |
| Kenney Topdown Shooter (Robot 1) | aynı paket | CC0 | Ajan (arkadaş yapay zekâ) sprite'ı | `Assets/_Project/Art/Kenney/TopdownShooter/Karakterler/robot1_stand.png` |

Lisans metni: `Assets/_Project/Art/Kenney/TopdownShooter/License.txt`

## Kendi yaptığımız görsel nesneler

Giriş holündeki kapı, kör nokta halısı, kadife bank, sütunlar, heykel ve vitrinler; lazer salonundaki engeller, vitrin, tezgâh, elmas, lazer ışınları ve şalter paneli Unity'nin yerleşik şekilleri (kare, daire) ve renklerle bu projede oluşturuldu. Bu nesneler için üçüncü taraf asset kullanılmadı.

## Sesler

| Asset | Kaynak | Lisans | Nerede kullanıldı | Dosya |
|---|---|---|---|---|
| Buton tıkı | https://freesound.org/people/Jaszunio15/sounds/421475/ | CC0 | Menü, sonuç ve diyalog butonları | `Assets/_Project/Audio/click.wav` |
| Alarm | https://freesound.org/people/nikkolaus/sounds/442563/ | CC0 | Güvenlik alarm çaldığında (en fazla 3 sn) | `Assets/_Project/Audio/alarm.wav` |
| Kazanma | https://freesound.org/people/MLaudio/sounds/615099/ | CC0 | Oyun kazanıldığında | `Assets/_Project/Audio/win.wav` |
| Kaybetme | https://freesound.org/people/suntemple/sounds/253174/ | CC0 | Oyun kaybedildiğinde | `Assets/_Project/Audio/lose.wav` |
| Adım | https://freesound.org/people/Fission9/sounds/521590/ | CC0 | Oyuncu yürürken | `Assets/_Project/Audio/step.wav` |

Ses dosyaları `Assets/_Project/Resources/SoundBank.asset` üzerinden `AudioManager` ile çalınır. Ses kapalı ya da dosya eksikken oyun hata vermeden sessiz çalışır (F8 ile sessize alınır).

## Paketler ve araçlar

| Paket / araç | Kaynak | Lisans | Nerede kullanıldı |
|---|---|---|---|
| NavMeshPlus | https://github.com/h8man/NavMeshPlus | MIT | 02_Lazer'de ajanın NavMesh ile yol bulması (2D NavMesh) |
| TextMesh Pro | Unity paketi (Package Manager) | Unity paket lisansı | Diyalog paneli yazıları |
| Input System | Unity paketi (Package Manager) | Unity paket lisansı | Klavye girişi |
| Ollama | https://ollama.com | MIT | Yerel dil modeli sunucusu |
| gemma3:4b | Google, Ollama üzerinden | Gemma Kullanım Koşulları | Güvenlik NPC'sinin cevapları |

