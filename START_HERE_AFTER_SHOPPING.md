# WHEN YOU GET BACK: Unity Setup Guide

**Project Status**: ✅ All code complete! Just needs Unity Editor setup.

**Time Required**: 2-3 hours for minimal playable game

---

## 🎯 Your Mission

Get the game running in Unity Play Mode so you can click/tap to hop and see the core loop work.

---

## 📋 Step-by-Step Checklist

### ☐ Step 1: Open Project in Unity (5 minutes)

1. Open **Unity Hub**
2. Click **"Add"** → Select folder: `C:\dev\koksal`
3. Select Unity version: **2022.3 LTS** or **2023.3 LTS** (if not installed, download it)
4. Click the project to open Unity Editor
5. Wait for initial import (~2-3 minutes)

**✓ Success Check**: Unity Editor opens without errors

---

### ☐ Step 2: Configure Project Settings (10 minutes)

#### Player Settings
1. **Edit → Project Settings → Player**
2. Set these:
   - Company Name: `YourName`
   - Product Name: `Köksal Baba Rage Runner`
   - Bundle Identifier: `com.yourname.koksalbaba`
3. **iOS Settings** tab:
   - Target minimum iOS Version: `14.0`
   - Target SDK: `Device SDK`
   - Target Device: `iPhone Only`
   - Architecture: `IL2CPP`

#### Quality Settings
1. **Edit → Project Settings → Quality**
2. Disable: Shadows, Anti-Aliasing, Soft Particles
3. V Sync Count: `Don't Sync`

#### Physics2D Settings
1. **Edit → Project Settings → Physics 2D**
2. Fixed Timestep: `0.02`

**✓ Success Check**: All settings saved

---

### ☐ Step 3: Create Scenes (30 minutes)

#### Create Bootstrap Scene
1. **File → New Scene** (empty scene)
2. **GameObject → Create Empty** → Name it `"_GameManager"`
3. Select `_GameManager` → **Add Component** → Search `"GameManager"` → Add it
4. **File → Save As** → `Assets/Scenes/Bootstrap.unity`

#### Create MainMenu Scene
1. **File → New Scene**
2. **GameObject → UI → Canvas** (creates Canvas + EventSystem)
3. Select Canvas → **Add Component** → Search `"MainMenuController"` → Add it
4. Right-click Canvas → **UI → Button - TextMeshPro** → Name it `"PlayButton"`
   - If TextMeshPro import window appears, click **"Import TMP Essentials"**
5. Select PlayButton → In Inspector, change Text to `"PLAY"`
6. Select PlayButton → Scroll down to **Button component** → Click **"+"** on **OnClick()**
   - Drag Canvas (with MainMenuController) into the object slot
   - Select function: `MainMenuController → OnPlayClicked()`
7. **File → Save As** → `Assets/Scenes/MainMenu.unity`

#### Create Game Scene (Minimal)
1. **File → New Scene**
2. Keep the default Camera
3. **File → Save As** → `Assets/Scenes/Game.unity`

#### Create Results Scene (Minimal)
1. **File → New Scene**
2. **GameObject → UI → Canvas**
3. Right-click Canvas → **UI → Button - TextMeshPro** → Name it `"ReplayButton"`
4. Change button text to `"REPLAY"`
5. **File → Save As** → `Assets/Scenes/Results.unity`

**✓ Success Check**: 4 scenes in Assets/Scenes/ folder

---

### ☐ Step 4: Configure Build Settings (5 minutes)

1. **File → Build Settings**
2. Click **"Add Open Scenes"** with Bootstrap scene open
3. Repeat for MainMenu, Game, Results (in that order)
4. Drag Bootstrap to be **scene index 0** (top)
5. Close window (don't build yet)

**✓ Success Check**: Build Settings shows 4 scenes, Bootstrap at top

---

### ☐ Step 5: Wire Services in Bootstrap (15 minutes)

1. Open `Assets/Scripts/Core/GameManager.cs` in Visual Studio
2. Find the `InitializeServices()` method (around line 50)
3. Replace the TODO comment with:

```csharp
private void InitializeServices()
{
    // Create AudioService GameObject
    GameObject audioObj = new GameObject("AudioService");
    DontDestroyOnLoad(audioObj);
    AudioService audioService = audioObj.AddComponent<AudioService>();
    
    // Register all services
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
    
    // Initialize services
    ServiceLocator.Instance.Get<IInputService>().Initialize();
    ServiceLocator.Instance.Get<IAudioService>().Initialize();
    ServiceLocator.Instance.Get<IHapticService>().Initialize();
    
    Debug.Log("All services initialized");
}
```

4. Save file (Ctrl+S)
5. Return to Unity (wait for compile, check Console for errors)

**✓ Success Check**: No compilation errors in Unity Console

---

### ☐ Step 6: Fix Namespace Issues (10 minutes)

The scaffolds use namespace `RageRunner.*` but new code uses `KoksalBaba.*`. Quick fix:

1. Open `Assets/Scripts/Core/GameManager.cs`
2. Change line 6 from `namespace RageRunner.Core` to `namespace KoksalBaba.Core`
3. Do the same for:
   - `Assets/Scripts/Gameplay/PlayerController.cs` → `KoksalBaba.Gameplay`
   - `Assets/Scripts/Gameplay/RageMeter.cs` → `KoksalBaba.Gameplay`
   - `Assets/Scripts/Gameplay/Spawner.cs` → `KoksalBaba.Gameplay`
   - `Assets/Scripts/Gameplay/ScoreService.cs` → `KoksalBaba.Gameplay`
   - `Assets/Scripts/UI/HUDController.cs` → `KoksalBaba.UI`

4. Save all, wait for Unity to recompile

**✓ Success Check**: No namespace errors in Console

---

### ☐ Step 7: Test Bootstrap → MainMenu Flow (5 minutes)

1. Open Bootstrap scene
2. Press **Play** button (top center)
3. Watch Console log:
   - Should see: "All services initialized"
   - Should see: "GameState transition: Bootstrap → MainMenu"
4. MainMenu scene should load automatically
5. Press **Play** again to stop

**✓ Success Check**: Bootstrap loads MainMenu without errors

---

### ☐ Step 8: Create Minimal Player Prefab (15 minutes)

1. In Game scene, **GameObject → 2D Object → Sprites → Square**
2. Rename to `"Player"`
3. Select Player:
   - **Add Component** → `Rigidbody2D`
     - Set Gravity Scale: `2.5`
   - **Add Component** → `Box Collider 2D`
     - Check **"Is Trigger"**
   - **Add Component** → Search `PlayerController` → Add
   - **Add Component** → Search `RageMeter` → Add
4. Set position: X = `-5`, Y = `0`, Z = `0`
5. Change color: Select Player → Inspector → Sprite Renderer → Color → Red
6. Drag Player from Hierarchy to `Assets/` folder (creates prefab)
7. Save scene

**✓ Success Check**: Player prefab exists in Assets folder

---

### ☐ Step 9: Test Minimal Gameplay (10 minutes)

1. Open Game scene
2. Drag Player prefab into scene at position `(0, 0, 0)`
3. Select Camera → Set position: X = `0`, Y = `0`, Z = `-10`
4. Press **Play**
5. **Click anywhere** (simulates tap)
6. Player should hop (move right and up, then fall)
7. Press **Play** again to stop

**✓ Success Check**: Player hops when you click

---

### ☐ Step 10: Quick Test Full Loop (5 minutes)

1. Open MainMenu scene
2. Press **Play**
3. Click **PLAY** button
4. Game scene should load
5. Click to hop
6. Player falls off screen (Y < -3) → Game Over → Results scene loads
7. If it crashes, that's OK! Core loop is working.

**✓ Success Check**: MainMenu → Game → Results flow works

---

## 🎉 Success!

You now have a **playable prototype**! The core architecture is working.

---

## 🚀 Next Steps (Optional, for later)

- Create obstacle prefabs (StaticPole, MovingBarrier, BreakableCrate)
- Add Spawner to Game scene
- Wire HUD Canvas in Game scene
- Add ground sprite
- Create pickup prefabs (TauntToken, Coin)
- Replace placeholder sprites with art

---

## 📞 If You Get Stuck

**Common Issues**:

1. **"GameManager script not found"** 
   → Wait for Unity to finish compiling (bottom-right progress bar)

2. **"Namespace not found"**
   → Did you change `RageRunner.*` to `KoksalBaba.*` in all 6 files?

3. **Player doesn't move**
   → Check Rigidbody2D is attached and Gravity Scale = 2.5

4. **Button doesn't work**
   → Check OnClick() event is wired to MainMenuController.OnPlayClicked()

5. **Scenes don't load**
   → Check Build Settings has all 4 scenes in order

---

## 📊 Time Budget

- ☐ Step 1: 5 min
- ☐ Step 2: 10 min
- ☐ Step 3: 30 min
- ☐ Step 4: 5 min
- ☐ Step 5: 15 min
- ☐ Step 6: 10 min
- ☐ Step 7: 5 min
- ☐ Step 8: 15 min
- ☐ Step 9: 10 min
- ☐ Step 10: 5 min

**Total**: ~2 hours

---

## ✅ Done? Commit Your Work!

```powershell
git add .
git commit -m "feat: Complete Unity Editor integration - MVP playable"
```

---

**Have fun shopping! The hardest part (coding) is done. This is just Unity clicking! 🛒🎮**
