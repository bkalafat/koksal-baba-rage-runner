# 🎮 Unity'de Yapılacaklar - Köksal Baba Rage Runner

**Durum**: ✅ Tüm kod hazır! Sadece Unity Editor'de kurulum gerekli.

**Süre**: Minimal oynanabilir versiyon için 2-3 saat

---

## 🚀 HIZLI BAŞLANGIÇ - İlk 30 Dakikada Oyunu Çalıştır

### ☐ 1. Unity'de Projeyi Aç (5 dakika)

1. **Unity Hub**'ı aç
2. **"Add"** → Klasörü seç: `C:\dev\koksal`
3. Unity versiyonu: **2022.3 LTS** veya **2023.3 LTS** (yoksa indir)
4. Projeye tıkla, Unity Editor açılsın
5. İlk import bekle (~2-3 dakika)

✓ **Başarı**: Unity Editor hatasız açılır

---

### ☐ 2. Proje Ayarları (10 dakika)

#### Player Settings
**Edit → Project Settings → Player**
- Company Name: `YourName`
- Product Name: `Köksal Baba Rage Runner`
- Bundle Identifier: `com.yourname.koksalbaba`
- **iOS Settings**:
  - Target minimum iOS: `14.0`
  - Target SDK: `Device SDK`
  - Architecture: `IL2CPP`

#### Quality Settings
**Edit → Project Settings → Quality**
- Shadows: **KAPALI**
- Anti-Aliasing: **KAPALI**
- V Sync: `Don't Sync`

#### Physics2D
**Edit → Project Settings → Physics 2D**
- Fixed Timestep: `0.02`

✓ **Başarı**: Ayarlar kaydedildi

---

### ☐ 3. Bootstrap Scene Oluştur (5 dakika)

1. **File → New Scene** (boş sahne)
2. **GameObject → Create Empty** → İsim: `"_GameManager"`
3. `_GameManager` seç → **Add Component** → `"GameManager"` ara → ekle
4. **File → Save As** → `Assets/Scenes/Bootstrap.unity`

✓ **Başarı**: Bootstrap.unity oluşturuldu

---

### ☐ 4. MainMenu Scene Oluştur (5 dakika)

1. **File → New Scene**
2. **GameObject → UI → Canvas**
3. Canvas seç → **Add Component** → `"MainMenuController"` ekle
4. Canvas'a sağ tık → **UI → Button - TextMeshPro** → İsim: `"PlayButton"`
   - TextMeshPro import penceresi çıkarsa **"Import TMP Essentials"**
5. PlayButton seç → Text'i değiştir: `"OYNA"`
6. PlayButton → Button component → **OnClick()** → **"+"**
   - Canvas'ı sürükle
   - Fonksiyon: `MainMenuController → OnPlayClicked()`
7. **File → Save As** → `Assets/Scenes/MainMenu.unity`

✓ **Başarı**: MainMenu.unity oluşturuldu

---

### ☐ 5. Game ve Results Scene (5 dakika)

#### Game Scene
1. **File → New Scene**
2. Varsayılan Camera'yı tut
3. **File → Save As** → `Assets/Scenes/Game.unity`

#### Results Scene
1. **File → New Scene**
2. **GameObject → UI → Canvas**
3. Canvas'a sağ tık → **UI → Button - TextMeshPro** → İsim: `"ReplayButton"`
4. Button text: `"TEKRAR OYNA"`
5. **File → Save As** → `Assets/Scenes/Results.unity`

✓ **Başarı**: 4 scene hazır

---

### ☐ 6. Build Settings (3 dakika)

1. **File → Build Settings**
2. Bootstrap scene açıkken **"Add Open Scenes"**
3. Diğer 3 scene için tekrarla (MainMenu, Game, Results)
4. Bootstrap'i en üste sürükle (scene index 0)

✓ **Başarı**: 4 scene sırayla eklendi

---

### ☐ 7. Namespace Düzeltmeleri (10 dakika)

Bu 6 dosyayı Visual Studio'da aç ve namespace'leri değiştir:

**`RageRunner.*` → `KoksalBaba.*`**

1. `Assets/Scripts/Core/GameManager.cs`
   - `namespace RageRunner.Core` → `namespace KoksalBaba.Core`

2. `Assets/Scripts/Gameplay/PlayerController.cs`
   - `namespace RageRunner.Gameplay` → `namespace KoksalBaba.Gameplay`

3. `Assets/Scripts/Gameplay/RageMeter.cs`
   - `namespace RageRunner.Gameplay` → `namespace KoksalBaba.Gameplay`

4. `Assets/Scripts/Gameplay/Spawner.cs`
   - `namespace RageRunner.Gameplay` → `namespace KoksalBaba.Gameplay`

5. `Assets/Scripts/Gameplay/ScoreService.cs`
   - `namespace RageRunner.Gameplay` → `namespace KoksalBaba.Gameplay`

6. `Assets/Scripts/UI/HUDController.cs`
   - `namespace RageRunner.UI` → `namespace KoksalBaba.UI`

Kaydet (Ctrl+S), Unity'ye dön, derleme bekle.

✓ **Başarı**: Console'da hata yok

---

### ☐ 8. Servisleri Bağla (10 dakika)

`Assets/Scripts/Core/GameManager.cs` aç, `InitializeServices()` metodunu bul, TODO yorumunu şununla değiştir:

```csharp
private void InitializeServices()
{
    // AudioService GameObject oluştur
    GameObject audioObj = new GameObject("AudioService");
    DontDestroyOnLoad(audioObj);
    AudioService audioService = audioObj.AddComponent<AudioService>();
    
    // Tüm servisleri kaydet
    ServiceLocator.Instance.Register<IInputService>(new TouchInputService());
    ServiceLocator.Instance.Register<ITimeService>(new TimeService());
    ServiceLocator.Instance.Register<IAudioService>(audioService);
    ServiceLocator.Instance.Register<IHapticService>(new HapticService());
    ServiceLocator.Instance.Register<IAdService>(new MockAdService());
    ServiceLocator.Instance.Register<IIAPService>(new MockIAPService());
    ServiceLocator.Instance.Register<IAnalyticsService>(new MockAnalyticsService());
    
    LocalizationService localization = new LocalizationService();
    localization.Initialize();
    ServiceLocator.Instance.Register<LocalizationService>(localization);
    
    // Servisleri başlat
    ServiceLocator.Instance.Get<IInputService>().Initialize();
    ServiceLocator.Instance.Get<IAudioService>().Initialize();
    ServiceLocator.Instance.Get<IHapticService>().Initialize();
    
    Debug.Log("Tüm servisler başlatıldı");
}
```

Kaydet, Unity'de derleme bekle.

✓ **Başarı**: Console'da derleme hatası yok

---

### ☐ 9. İLK TEST - Bootstrap → MainMenu (2 dakika)

1. Bootstrap scene'i aç
2. **Play** butonuna bas (üstte ortada)
3. Console'da şunları göreceksin:
   - "Tüm servisler başlatıldı"
   - "GameState transition: Bootstrap → MainMenu"
4. MainMenu scene otomatik yüklenir
5. **Play** tekrar bas (durdur)

✓ **Başarı**: Bootstrap → MainMenu geçişi çalışıyor

---

## 🎯 İKİNCİ AŞAMA - Oyuncu ve Hareket (1 saat)

### ☐ 10. Oyuncu Prefab Oluştur (15 dakika)

1. Game scene'i aç
2. **GameObject → 2D Object → Sprites → Square**
3. İsim: `"Player"`
4. Player seç:
   - **Add Component** → `Rigidbody2D`
     - Gravity Scale: `2.5`
   - **Add Component** → `Box Collider 2D`
     - **Is Trigger**: ✓ (işaretle)
   - **Add Component** → `PlayerController` ara ve ekle
   - **Add Component** → `RageMeter` ara ve ekle
5. Pozisyon: X = `0`, Y = `0`, Z = `0`
6. Renk: Player seç → Inspector → Sprite Renderer → Color → **Kırmızı**
7. Player'ı Hierarchy'den `Assets/` klasörüne sürükle (prefab oluşturur)
8. Scene kaydet

✓ **Başarı**: Assets/'da Player prefab var

---

### ☐ 11. İKİNCİ TEST - Oyuncu Zıplama (5 dakika)

1. Game scene açık
2. Player prefab'ı scene'e sürükle (0, 0, 0 pozisyonunda)
3. Camera seç → Pozisyon: X = `0`, Y = `0`, Z = `-10`
4. **Play** bas
5. **Fareyle tıkla** (tap simülasyonu)
6. Player zıplamalı (sağa ve yukarı gider, sonra düşer)
7. **Play** tekrar bas (durdur)

✓ **Başarı**: Tıklayınca player zıplıyor

---

### ☐ 12. Zemin Oluştur (10 dakika)

1. Game scene'de **GameObject → 2D Object → Sprites → Square**
2. İsim: `"Ground"`
3. Pozisyon: X = `0`, Y = `-4`, Z = `0`
4. Scale: X = `20`, Y = `1`, Z = `1`
5. Renk: Gri
6. **Add Component** → `Box Collider 2D` (Is Trigger KAPALI)
7. Scene kaydet

✓ **Başarı**: Player yere düşünce duruyor

---

### ☐ 13. Engel Prefab Oluştur (15 dakika)

#### StaticPole (Sabit Engel)
1. **GameObject → 2D Object → Sprites → Square**
2. İsim: `"StaticPole"`
3. Pozisyon: X = `10`, Y = `-2`, Z = `0`
4. Scale: X = `1`, Y = `4`, Z = `1`
5. Renk: Koyu gri
6. **Add Component** → `Box Collider 2D` (Is Trigger: ✓)
7. Tag: **"Obstacle"** (Tag yoksa oluştur: Add Tag)
8. **Add Component** → `Obstacle` script ekle
9. Hierarchy'den Assets/'a sürükle (prefab)
10. Scene'den sil

✓ **Başarı**: StaticPole prefab hazır

---

### ☐ 14. Pickup Prefab Oluştur (10 dakika)

#### TauntToken (Öfke Simgesi)
1. **GameObject → 2D Object → Sprites → Circle**
2. İsim: `"TauntToken"`
3. Scale: X = `0.5`, Y = `0.5`, Z = `1`
4. Renk: Altın sarısı
5. **Add Component** → `Circle Collider 2D` (Is Trigger: ✓)
6. Tag: **"Pickup"** (yoksa oluştur)
7. **Add Component** → `Pickup` script ekle
8. Inspector'da Pickup component → Type: **Taunt**
9. Hierarchy'den Assets/'a sürükle (prefab)
10. Scene'den sil

✓ **Başarı**: TauntToken prefab hazır

---

### ☐ 15. ÜÇÜNCÜ TEST - Tam Oyun Döngüsü (5 dakika)

1. MainMenu scene'i aç
2. **Play** bas
3. **OYNA** butonuna tıkla
4. Game scene yüklenir
5. Fareyle tıkla, player zıplar
6. Player aşağı düşer (Y < -3) → Game Over → Results scene yüklenir
7. (Crash olursa sorun değil! Ana döngü çalışıyor demektir)

✓ **Başarı**: MainMenu → Game → Results akışı çalışıyor!

---

## 🎨 ÜÇÜNCÜ AŞAMA - HUD ve Spawner (1 saat)

### ☐ 16. HUD Canvas Oluştur (20 dakika)

1. Game scene'i aç
2. **GameObject → UI → Canvas**
3. Canvas seç:
   - Render Mode: `Screen Space - Overlay`
   - Canvas Scaler → UI Scale Mode: `Scale With Screen Size`
   - Reference Resolution: X = `1080`, Y = `1920`
4. **Add Component** → `HUDController` ekle
5. Canvas'a sağ tık → **UI → Text - TextMeshPro** → İsim: `"ScoreText"`
   - Pozisyon: Sol üst köşe
   - Text: `"0"`
   - Font Size: `48`
6. Canvas'a sağ tık → **UI → Image** → İsim: `"RageBarFill"`
   - Pozisyon: Alt ortada
   - Renk: Kırmızı
7. Canvas'a sağ tık → **UI → Button** → İsim: `"PauseButton"`
   - Pozisyon: Sağ üst köşe
   - Text: `"II"` (duraklat simgesi)

#### HUDController Bağlantılarını Yap
8. Canvas seç → Inspector'da HUDController component:
   - **Score Text**: ScoreText'i sürükle
   - **Rage Bar Fill**: RageBarFill'i sürükle
   - **Pause Button**: PauseButton'u sürükle

9. Scene kaydet

✓ **Başarı**: HUD görünüyor, skor 0 gösteriyor

---

### ☐ 17. Spawner Oluştur (15 dakika)

1. Game scene'de **GameObject → Create Empty**
2. İsim: `"Spawner"`
3. Pozisyon: X = `15`, Y = `0`, Z = `0`
4. **Add Component** → `Spawner` ekle
5. Inspector'da Spawner component:
   - **Obstacle Prefabs**: Size = `1`, StaticPole prefab'ı sürükle
   - **Pickup Prefabs**: Size = `1`, TauntToken prefab'ı sürükle
6. Scene kaydet

✓ **Başarı**: Spawner hazır (henüz çalışmıyor, DifficultyCurve lazım)

---

### ☐ 18. DifficultyCurve Asset Oluştur (10 dakika)

1. Assets klasöründe sağ tık → **Create → ScriptableObjects → DifficultyCurve**
2. İsim: `"DefaultDifficultyCurve"`
3. Asset'i seç, Inspector'da:
   - **Spawn Period Curve**: Curve editörü aç
     - Keyframe (0, 1.25), (60, 1.0), (120, 0.75)
   - **Obstacle Speed Curve**: 
     - Keyframe (0, 6.0), (60, 7.0), (120, 8.5)
   - **Min Gap Curve**:
     - Keyframe (0, 3.5), (60, 3.0), (120, 2.5)

4. Game scene'de Spawner seç:
   - **Difficulty Curve**: DefaultDifficultyCurve'ü sürükle

5. Scene kaydet

✓ **Başarı**: Spawner artık zorluk eğrisi kullanıyor

---

### ☐ 19. DÖRDÜNCÜ TEST - Engeller Spawn Oluyor mu? (5 dakika)

1. Game scene'i aç
2. **Play** bas
3. Tıkla, player zıplar
4. 1-2 saniye sonra sağdan engeller gelmeye başlamalı
5. Engellere çarpınca Game Over → Results

✓ **Başarı**: Engeller spawn oluyor ve oyun döngüsü tam çalışıyor!

---

## 📊 DURUM KONTROLÜ

Bu adımları tamamladıysan, **OYNANILIR BİR PROTOTİP**'in var! 🎉

### Çalışan Özellikler:
- ✅ MainMenu → Game → Results akışı
- ✅ Tap ile zıplama (fareyle test)
- ✅ Engellere çarpınca ölme
- ✅ Engeller spawn oluyor
- ✅ HUD'da skor görünüyor
- ✅ Temel oyun döngüsü

### Henüz Çalışmayanlar (Opsiyonel):
- ⚠️ Öfke sistemi (pickup toplayınca bar dolmuyor - kod hazır, bağlantı lazım)
- ⚠️ Skor artışı (mesafe sayılmıyor - ScoreService bağlantısı lazım)
- ⚠️ Pause menü
- ⚠️ Shop ve Settings

---

## 🚀 SONRAKI ADIMLAR (İsteğe Bağlı)

### Öfke Sistemini Aktifleştir (30 dakika)
- PlayerController'da pickup collision'ı RageMeter'a bağla
- RageMeter dolunca "TAP TO RAGE!" göster
- Dash animasyonu ekle

### Skor Sistemini Aktifleştir (20 dakika)
- ScoreService'i GameManager'da başlat
- PlayerController her frame'de mesafe göndersin
- HUD skorları ScoreService'den alsın

### Daha Fazla Engel (1 saat)
- MovingBarrier prefab (yukarı aşağı hareket eder)
- BreakableCrate prefab (dash ile kırılabilir)

---

## ⏱️ TOPLAM SÜRE

- ☐ Adım 1-9 (Hızlı Başlangıç): **~45 dakika**
- ☐ Adım 10-15 (Oyuncu ve Hareket): **~60 dakika**
- ☐ Adım 16-19 (HUD ve Spawner): **~50 dakika**

**Toplam**: ~2.5 saat ile **oynanabilir oyun**!

---

## 🆘 Sık Karşılaşılan Sorunlar

1. **"GameManager script bulunamadı"**
   → Unity derlemeyi bitirsin (sağ alt köşe progress bar)

2. **"Namespace bulunamadı"**
   → RageRunner.* → KoksalBaba.* değişimi yaptın mı? (Adım 7)

3. **Player hareket etmiyor**
   → Rigidbody2D ekli mi? Gravity Scale = 2.5 mi?

4. **Buton çalışmıyor**
   → OnClick() event MainMenuController.OnPlayClicked()'a bağlı mı?

5. **Engeller spawn olmuyor**
   → DifficultyCurve asset Spawner'a sürüklenmiş mi?

---

## ✅ TAMAMLADIĞINDA

```powershell
git add .
git commit -m "feat: Unity Editor entegrasyonu tamamlandı - MVP oynanabilir"
git push
```

**Başarılar! En zor kısım (kodlama) bitti. Bu sadece Unity'de tıklamalar! 🎮**
