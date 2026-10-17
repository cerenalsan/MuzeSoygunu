Risk Notları

Risk 1: NavMeshPlus (2D NavMesh) Unity 6.3'te çalışır mı?



Tarih: 17 Ekim 2026 | Sorumlu: Ceren | Durum: İlk test geçti, GO/NO-GO kararı 18 Ekim



Ortam

Unity 6.3 LTS (6000.3.25f1), 2D Built-in RP

NavMeshPlus 0.2.23 (Package Manager, git URL ile)

Yapılan test



Düz bir odada (Square sprite zemin) NavMeshAgent’ın tıklanan noktaya yürümesi.



Sonuç

Bake başarılı, zemin üstünde mavi NavMesh alanı oluştu.

Ajan tıklanan noktaya yürüyor.

Alanın dışına tıklanınca ajan sınırda duruyor, dışarı çıkmıyor.

Karşılaştığımız sorunlar ve çözümleri

Hata: “NavMeshExtension requires a NavMeshSurface component”

Çözüm: Component sırası önemli. Önce Navigation Surface, sonra Navigation CollectSources2d eklenmeli.

Belirti: Bake sonrası mavi alan yok, Console’da “Sources 0”

Çözüm: Zemin objesine Navigation Modifier eklenip “Override Area” işaretlendi, Area = Walkable seçildi. Sonrasında “Sources 1”.

Ajan ayarı: Script’te updateRotation = false ve updateUpAxis = false gerekli, yoksa 2D’de ajan sprite’ı yan yatar.

Henüz test edilmeyenler (18 Ekim)

Engel (duvar) etrafından dolaşma (Navigation Modifier, Not Walkable)

Dinamik engel

5 ışınlı sensör taslağı

Karar



Şimdilik devam (GO) yönünde. Engel testi de geçerse kesinleşir. Plan B: 3D sahne + ortografik kamera + standart NavMesh.

