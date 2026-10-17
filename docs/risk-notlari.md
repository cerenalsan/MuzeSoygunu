# Risk Notları

## Risk 1: NavMeshPlus (2D NavMesh) Unity 6.3'te çalışır mı?

**Tarih:** 17 Ekim 2026 | **Sorumlu:** Ceren | **Durum:** İlk test geçti, GO/NO-GO kararı 18 Ekim

### Ortam
- Unity 6.3 LTS (6000.3.25f1), 2D Built-in RP
- NavMeshPlus 0.2.23 (Package Manager, git URL ile)

### Yapılan test
Düz bir odada (Square sprite zemin) NavMeshAgent'ın tıklanan noktaya yürümesi, ardından zemine engel (duvar) konarak dolanma testi.

### Sonuç
- Bake başarılı, zemin üstünde mavi NavMesh alanı oluştu.
- Ajan tıklanan noktaya yürüyor.
- Alanın dışına tıklanınca ajan sınırda duruyor, dışarı çıkmıyor.
- Engel testi (Navigation Modifier, Override Area, Not Walkable): ajan zemindeki engelin etrafından dolanıyor.

### Karşılaştığımız sorunlar ve çözümleri
1. **Hata:** "NavMeshExtension requires a NavMeshSurface component"
   **Çözüm:** Component sırası önemli. Önce Navigation Surface, sonra Navigation CollectSources2d eklenmeli.
2. **Belirti:** Bake sonrası mavi alan yok, Console'da "Sources 0"
   **Çözüm:** Zemin objesine Navigation Modifier eklenip "Override Area" işaretlendi, Area = Walkable seçildi. Sonrasında "Sources 1".
3. **Ajan ayarı:** Script'te `updateRotation = false` ve `updateUpAxis = false` gerekli, yoksa 2D'de ajan sprite'ı yan yatar.
4. **Görsel sorun:** Engel sprite'ı zeminle aynı Order in Layer değerindeyse zeminin altında kalıp görünmüyor. Engel için Order in Layer = 1 verildi (NavMesh etkilenmez, sadece çizim sırası).

cat > docs/risk-notlari.md << 'EOF'
# Risk Notları

## Risk 1: NavMeshPlus (2D NavMesh) Unity 6.3'te çalışır mı?

**Tarih:** 17 Ekim 2026 | **Sorumlu:** Ceren | **Durum:** İlk test geçti, GO/NO-GO kararı 18 Ekim

### Ortam
- Unity 6.3 LTS (6000.3.25f1), 2D Built-in RP
- NavMeshPlus 0.2.23 (Package Manager, git URL ile)

### Yapılan test
Düz bir odada (Square sprite zemin) NavMeshAgent'ın tıklanan noktaya yürümesi, ardından zemine engel (duvar) konarak dolanma testi.

### Sonuç
- Bake başarılı, zemin üstünde mavi NavMesh alanı oluştu.
- Ajan tıklanan noktaya yürüyor.
- Alanın dışına tıklanınca ajan sınırda duruyor, dışarı çıkmıyor.
- Engel testi (Navigation Modifier, Override Area, Not Walkable): ajan zemindeki engelin etrafından dolanıyor.

### Karşılaştığımız sorunlar ve çözümleri
1. **Hata:** "NavMeshExtension requires a NavMeshSurface component"
   **Çözüm:** Component sırası önemli. Önce Navigation Surface, sonra Navigation CollectSources2d eklenmeli.
2. **Belirti:** Bake sonrası mavi alan yok, Console'da "Sources 0"
   **Çözüm:** Zemin objesine Navigation Modifier eklenip "Override Area" işaretlendi, Area = Walkable seçildi. Sonrasında "Sources 1".
3. **Ajan ayarı:** Script'te `updateRotation = false` ve `updateUpAxis = false` gerekli, yoksa 2D'de ajan sprite'ı yan yatar.
4. **Görsel sorun:** Engel sprite'ı zeminle aynı Order in Layer değerindeyse zeminin altında kalıp görünmüyor. Engel için Order in Layer = 1 verildi (NavMesh etkilenmez, sadece çizim sırası).

### Henüz test edilmeyenler (18 Ekim)
- Dinamik engel
- 5 ışınlı sensör taslağı

### Karar
Şimdilik **devam (GO)** yönünde. Engel testi geçti. Dinamik engel ve sensör taslağı da sorunsuzsa karar kesinleşir. Plan B: 3D sahne + ortografik kamera + standart NavMesh.

## Risk 2: Unity'den yerel Ollama'ya asenkron istek ve Ollama kapalıyken davranış

**Tarih:** 17 Ekim 2026 | **Sorumlu:** Zeynep | **Durum:** İlk test geçti, GO/NO-GO kararı 18 Ekim (20 cümlelik JSON testi)

### Ortam
- Unity 6.3 LTS (6000.3.25f1)
- Ollama yerelde, model gemma2:2b, adres http://127.0.0.1:11434
- Sandbox_Zeynep sahnesi: yazı kutusu, buton, durum metni ve LlmSandboxTester scripti

### Yapılan test
Yazı kutusundaki mesaj OllamaClient ile asenkron olarak Ollama'ya gönderildi. İstek sürerken durum metninde "Düşünüyor..." gösterildi ve buton pasif yapıldı.

### Sonuç
- Ollama açıkken: zincir uçtan uca çalıştı (yazı, istek, cevap, ekranda gösterim). Cevaplar Türkçe geldi, ğ ş ı İ ç ö ü karakterleri doğru göründü. Cevap süresi yaklaşık 10 saniye (ilk istek model yüklemesini içerebilir, tam ölçüm 30 Ekim'de).
- Ollama kapalıyken: "[Sistem] Ollama sunucusuna ulaşılamıyor" mesajı yaklaşık 10 saniye sonra gösterildi. Unity donmadı, Console'da kırmızı hata çıkmadı, buton tekrar tıklanabilir oldu.
- Ollama tekrar açılınca istek normal şekilde cevaplandı.

### Gözlemler
- Düz sohbette model rolden çıkabiliyor (örn. "kapıyı açarım" gibi yön karışıklığı). Sandbox'taki sistem promptu tek cümle, gerçek prompt 23 Ekim'de yazılacak.
- Kapalı sunucuda hata mesajının 10 saniye sonra gelmesinin sebebi araştırılmadı.

### Henüz test edilmeyenler
- Zaman aşımı (30 sn) yolu
- Yanlış model adı (ModelNotFound)
- İstek sırasında kare süresinin Profiler ile ölçümü
- 20 cümlelik JSON ve Türkçe testi (18 Ekim)
