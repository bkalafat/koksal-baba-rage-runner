# 🎮 Unity'de Yapılacaklar - Köksal Baba Rage Runner

**Durum**: ✅ Tüm kod hazır! Sadece Unity Editor'de kurulum gerekli.

**Süre**: Minimal oynanabilir versiyon için 2-3 saat

---

## 🖥️ SİSTEM BİLGİLERİN

- **Geliştirme**: Windows 10
- **Test Cihazı**: iPhone 13 mini, iOS 18.0.1 (Ekim 2024)
- **Hedef Platform**: iOS 14.0+ (iPhone 13 mini tam uyumlu ✅)
- **Ekran Çözünürlüğü**: 2340x1080 (5.4" Super Retina XDR)
- **Not**: iOS'ta major versiyon maksimum 18'dir (2024), 26 değil!

---

## ⚠️ ÖNEMLİ: iOS Build İçin Windows 10 Sınırlamaları

### 🚫 Windows'tan Doğrudan iOS Build YAPILAMAZ
Unity'de iOS için build **yapamazsın** çünkü:
- iOS buildi için **Xcode** gerekli (sadece macOS'ta çalışır)
- Apple'ın kod imzalama (code signing) macOS'a özel

### ✅ Senin İçin 3 Çözüm:

#### **Seçenek 1: Unity Remote (Hızlı Test) - ÖNERİLEN İLK ADIM** ⭐
iPhone'unda oyunu anında test et (build yapmadan):

1. **iPhone'una Unity Remote 5 indir** (App Store'dan ücretsiz)
2. **Unity Editor'de**: Edit → Project Settings → Editor → Device: `Any iOS Device`
3. **iPhone'u USB ile bilgisayara bağla**
4. iPhone'da Unity Remote 5'i aç
5. Unity'de **Play** bas → iPhone ekranında oyun görünür! (tap çalışır)

**Artıları**: ✅ Saniyeler içinde test, ✅ Anında değişiklik görürsün  
**Eksileri**: ⚠️ Performans tam değil, ⚠️ IAP/Haptics çalışmaz

---

#### **Seçenek 2: Unity Cloud Build** (Bulutta Mac kiralama)
1. https://build.cloud.unity3d.com/ (ücretsiz plan var)
2. GitHub repo'nu bağla
3. iOS build konfigürasyonu oluştur
4. .ipa dosyasını indir → TestFlight'a yükle

---

#### **Seçenek 3: Arkadaşının Mac'i** (En Pratik)
- Projeyi GitHub'dan clone'lasın
- Unity + Xcode yüklesin
- Build → iOS → Xcode'da iPhone'una deploy et

---

### 🎯 Senin İçin Önerilen İş Akışı

1. **Şimdi**: Unity Editor'de tüm scene/prefab'ları oluştur (3 saat) → Unity Remote ile iPhone'da test
2. **MVP Tamam**: Unity Cloud Build veya arkadaşın Mac kullan
3. **Yayına Hazır**: TestFlight → Beta test → App Store

---

##  HIZLI BAŞLANGIÇ - İlk 30 Dakikada Oyunu Çalıştır

### ☐ 1. Unity'de Projeyi Aç (5 dakika)

1. **Unity Hub**'ı aç
2. **"Add"** → Klasörü seç: `C:\dev\koksal`
3. Unity versiyonu: **Unity 6 (6000.0.x)** veya **2022.3/2023.3 LTS** (yoksa indir)
   - Unity 6 tercih edilir (en yeni 2D özellikler)
4. Projeye tıkla, Unity Editor açılsın
5. İlk import bekle (~2-3 dakika)
6. **İlk açılışta**: "Enter Safe Mode" uyarısı çıkarsa **"Ignore"** tıkla (normal bir durumdur)

✓ **Başarı**: Unity Editor hatasız açılır, Console'da kırmızı hata yok

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

### ☐ 4. MainMenu Scene Oluştur (10 dakika)

1. **File → New Scene** → **2D (Built-in)** template seç
2. **GameObject → UI → Canvas**
3. Canvas seç:
   - **Canvas Scaler** component'e tıkla
   - UI Scale Mode: `Scale With Screen Size`
   - Reference Resolution: X = `1080`, Y = `1920` (iPhone portrait)
   - Match: `0.5` (genişlik ve yükseklik arası denge)
4. Canvas seç → **Add Component** → `"MainMenuController"` ara → ekle
5. Canvas'a sağ tık → **UI → Button - TextMeshPro** → İsim: `"PlayButton"`
   - **İlk TMP kullanımı**: "Import TMP Essentials" penceresi çıkar → **"Import TMP Essentials"** tıkla
6. PlayButton seç:
   - Pozisyon: Center (Anchor Presets'ten ortada seç)
   - Text (child object) seç → Text: `"OYNA"`
   - Font Size: `48`
7. PlayButton seç → Inspector'da **Button** component → **OnClick()** → **"+"**
   - Canvas'ı Hierarchy'den sürükle
   - Dropdown: `MainMenuController → OnPlayClicked()`
8. **File → Save As** → `Assets/Scenes/MainMenu.unity`

✓ **Başarı**: MainMenu.unity oluşturuldu, OYNA butonu ortada

---

### ☐ 5. Game ve Results Scene (10 dakika)

#### Game Scene
1. **File → New Scene** → **2D (Built-in)** template
2. Main Camera seç:
   - **Camera** component → Projection: `Orthographic` (zaten seçili olmalı)
   - Size: `5` (2D kamera zoom seviyesi)
   - Background: Açık mavi (gökyüzü rengi)
3. **File → Save As** → `Assets/Scenes/Game.unity`

#### Results Scene
1. **File → New Scene** → **2D (Built-in)** template
2. Main Camera'yı sil (UI için gerekmiyor)
3. **GameObject → UI → Canvas**
4. Canvas seç → Canvas Scaler:
   - UI Scale Mode: `Scale With Screen Size`
   - Reference Resolution: `1080 x 1920`
5. Canvas'a **Add Component** → `ResultsController` ekle
6. Canvas'a sağ tık → **UI → Button - TextMeshPro** → İsim: `"ReplayButton"`
   - Pozisyon: Alt ortada
   - Text (child): `"TEKRAR OYNA"`
   - Font Size: `36`
7. Canvas'a sağ tık → **UI → Text - TextMeshPro** → İsim: `"ScoreText"`
   - Pozisyon: Ortada
   - Text: `"0"`
   - Font Size: `72`
   - Alignment: Center
8. **File → Save As** → `Assets/Scenes/Results.unity`

✓ **Başarı**: 4 scene hazır (Bootstrap, MainMenu, Game, Results)

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

### ☐ 10. Oyuncu Prefab Oluştur (20 dakika)

1. Game scene'i aç
2. **GameObject → 2D Object → Sprites → Square** (Unity 6: GameObject → 2D Object → Sprite → Square)
3. İsim: `"Player"`
4. Player seç → Inspector'da:
   - **Transform**:
     - Position: X = `0`, Y = `0`, Z = `0`
     - Scale: X = `1`, Y = `2`, Z = `1` (dikdörtgen şekil için)
   - **Sprite Renderer**:
     - Color: **Kırmızı** (R=255, G=0, B=0)
     - Sorting Layer: Yeni layer oluştur: **"Player"**, Order in Layer: `10` (en üstte görünür)
5. Player'a component ekle:
   - **Add Component** → `Rigidbody2D`
     - Body Type: `Dynamic`
     - Gravity Scale: `2.5`
     - Collision Detection: `Continuous` (daha hassas çarpışma)
     - Constraints → Freeze Rotation: **Z** işaretle (takla atmasın)
   - **Add Component** → `Box Collider 2D`
     - **Is Trigger**: ✓ (işaretle)
     - Size: Otomatik ayarlanır, kontrol et
   - **Add Component** → `PlayerController` ara ve ekle
   - **Add Component** → `RageMeter` ara ve ekle
6. Player'ı Hierarchy'den **Project → Assets/** klasörüne sürükle
   - Prefab oluşturuldu mesajı görünecek
7. Scene'den Player'ı **SİLME** (test için lazım)
8. **File → Save Scene** (Ctrl+S)

✓ **Başarı**: Assets/'da Player.prefab var, Hierarchy'de Player var

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

### ☐ 16. HUD Canvas Oluştur (25 dakika)

1. Game scene'i aç
2. **GameObject → UI → Canvas** → İsim: `"HUD Canvas"`
3. HUD Canvas seç:
   - **Canvas** component:
     - Render Mode: `Screen Space - Overlay`
   - **Canvas Scaler** component:
     - UI Scale Mode: `Scale With Screen Size`
     - Reference Resolution: X = `1080`, Y = `1920`
     - Match: `0.5`
4. HUD Canvas seç → **Add Component** → `HUDController` ekle

#### Skor Text Oluştur
5. HUD Canvas'a sağ tık → **UI → Text - TextMeshPro** → İsim: `"ScoreText"`
   - **Rect Transform**:
     - Anchor Presets: Sol üst köşe (Alt tuşuna basılı tut, shift+alt ile pozisyon+pivot ayarla)
     - Pos X: `100`, Pos Y: `-100`
   - **TextMeshPro**:
     - Text: `"0"`
     - Font Size: `64`
     - Color: Beyaz
     - Alignment: Sol üst

#### Öfke Barı Oluştur
6. HUD Canvas'a sağ tık → **UI → Image** → İsim: `"RageBarBackground"`
   - Anchor: Alt ortada
   - Pos X: `0`, Pos Y: `150`, Width: `600`, Height: `40`
   - Color: Koyu gri (arka plan)
7. RageBarBackground'a sağ tık → **UI → Image** → İsim: `"RageBarFill"`
   - **Rect Transform**:
     - Anchor: Sol alt köşe (stretch left)
     - Left: `0`, Right: `0`, Top: `0`, Bottom: `0`
   - **Image** component:
     - Image Type: `Filled`
     - Fill Method: `Horizontal`
     - Fill Origin: `Left`
     - Fill Amount: `0.5` (test için yarım dolu)
     - Color: Kırmızı

#### Duraklat Butonu
8. HUD Canvas'a sağ tık → **UI → Button - TextMeshPro** → İsim: `"PauseButton"`
   - Anchor: Sağ üst köşe
   - Pos X: `-100`, Pos Y: `-100`
   - Width: `100`, Height: `100`
   - Text (child): `"II"`
   - Font Size: `48`

#### HUDController Bağlantıları
9. HUD Canvas seç → Inspector'da **HUDController** component:
   - **Score Text**: ScoreText'i Hierarchy'den sürükle
   - **Rage Bar Fill**: RageBarFill'i sürükle
   - **Rage Ready Prompt**: (şimdilik boş bırak, sonra ekleriz)
   - **Pause Button**: PauseButton'u sürükle
10. PauseButton seç → **Button** component → **OnClick()** → **"+"**
    - HUD Canvas'ı sürükle
    - Fonksiyon: `HUDController → OnPauseButtonClicked()`

11. **File → Save Scene**

✓ **Başarı**: HUD tam ekranda, skor sol üstte "0", öfke barı altta kırmızı, duraklat butonu sağ üstte

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

- ☐ Adım 1-9 (Hızlı Başlangıç): **~60 dakika**
- ☐ Adım 10-15 (Oyuncu ve Hareket): **~75 dakika**
- ☐ Adım 16-19 (HUD ve Spawner): **~60 dakika**

**Toplam**: ~3-3.5 saat ile **oynanabilir MVP oyun**!

**Not**: Unity 6 kullanıyorsan bazı adımlar daha hızlı olabilir (iyileştirilmiş UI araçları)

---

## 🆘 Sık Karşılaşılan Sorunlar

### 1. "GameManager script bulunamadı" / Kırmızı hatalar
**Çözüm**: Unity derlemeyi bitirsin (sağ alt köşe progress bar). 30-60 saniye bekle.

### 2. "Namespace 'RageRunner' could not be found"
**Çözüm**: Adım 7'de namespace değişimini yaptın mı? RageRunner.* → KoksalBaba.*
- Visual Studio'da 6 dosyayı değiştir, kaydet
- Unity'ye dön, otomatik derlenecek

### 3. Player hareket etmiyor / düşmüyor
**Kontroller**:
- ✓ Rigidbody2D ekli mi? (Inspector'da görmeli)
- ✓ Gravity Scale = 2.5 mi?
- ✓ Body Type: `Dynamic` mi?
- ✓ Constraints → Freeze Rotation Z işaretli mi?

### 4. "Input.GetMouseButtonDown not found" hatası
**Unity 6 için**: Eski Input System hala çalışıyor, sorun yok. Eğer hata alırsan:
- **Edit → Project Settings → Player → Other Settings**
- Active Input Handling: `Both` (veya `Input Manager (Old)`)

### 5. Buton çalışmıyor
**Kontroller**:
- ✓ Button component → OnClick() → "+" ile event eklendi mi?
- ✓ Doğru GameObject (Canvas) sürüklendi mi?
- ✓ Doğru fonksiyon seçildi mi? (ör: MainMenuController.OnPlayClicked())
- ✓ Script derlenmiş mi? (Console'da kırmızı hata yok)

### 6. Engeller spawn olmuyor
**Kontroller**:
- ✓ DifficultyCurve asset Spawner'a sürüklenmiş mi?
- ✓ Spawner'da Obstacle Prefabs size > 0 mı?
- ✓ StaticPole prefab sürüklenmiş mi?

### 7. TextMeshPro hataları
**Çözüm**: 
- **Window → TextMeshPro → Import TMP Essential Resources**
- Unity 6'da: Package Manager → TextMeshPro paketi yüklü mü kontrol et

### 8. Prefab'lar mavi değil / bağlantı kopuk
**Çözüm**: 
- Prefab'ı Assets/'a sürükledikten sonra Hierarchy'deki obje **mavi** olmalı
- Gri ise: Prefab bağlantısı kopmuş, tekrar sürükle

### 9. "Scene could not be loaded" / Build Settings
**Çözüm**:
- **File → Build Settings**
- 4 scene'i ekle: Bootstrap (index 0), MainMenu (1), Game (2), Results (3)
- Scene sırası çok önemli!

### 10. Unity 6'da Canvas Scaler farklı görünüyor
**Çözüm**: Normal! Unity 6'da UI sistemi iyileştirildi ama parametreler aynı:
- UI Scale Mode: `Scale With Screen Size`
- Reference Resolution: `1080 x 1920`
- Match: `0.5` veya `0` (ikisi de çalışır)

---

## ✅ TAMAMLADIĞINDA

```powershell
git add .
git commit -m "feat: Unity Editor entegrasyonu tamamlandı - MVP oynanabilir"
git push
```

**Başarılar! En zor kısım (kodlama) bitti. Bu sadece Unity'de tıklamalar! 🎮**

---

## 📚 Unity 6 İçin Ek Notlar

Eğer Unity 6 (6000.0.x) kullanıyorsan:

### Yeni 2D Özellikler
- **2D Sprite Shape**: Daha güzel engel tasarımı için kullanılabilir
- **2D Pixel Perfect**: Retro görünüm istersen aktifleştir (Camera component)
- **2D Animation**: Oyuncu koşu animasyonu eklemek için (opsiyonel)

### Performans İyileştirmeleri
- **Sprite Atlas**: Assets/Art/ klasöründe tüm sprite'ları topla, atlas oluştur
  - **Assets → Create → 2D → Sprite Atlas**
  - Tüm sprite'ları sürükle
  - Build boyutu %30-40 azalır!

### UI Toolkit (Yeni Sistem)
- UI Toolkit kullanmak istersen: Daha modern ama öğrenme eğrisi var
- Bu projede **uGUI (Canvas sistemi)** kullanıyoruz (daha yaygın, daha kolay)

### Input System
- Yeni Input System paketi Unity 6'da varsayılan
- Kodumuz eski Input System kullanıyor (Input.GetMouseButtonDown)
- Her ikisi de çalışır, **Active Input Handling: Both** seç
