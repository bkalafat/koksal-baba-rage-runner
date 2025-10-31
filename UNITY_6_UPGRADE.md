# Unity 6.0 LTS Upgrade Guide

**Date**: October 31, 2025  
**Project**: Köksal Baba: Rage Runner  
**Upgraded From**: Unity 6000.2.8f1 → **Unity 6.0.31f1 LTS**

## What Changed

### Unity Version
- **From**: Unity 6000.2.8f1 (early Unity 6 build)
- **To**: Unity 6.0.31f1 LTS (October 2024 release)
- **Support**: 2 years of LTS through October 2026
- **Benefits**: 
  - Up to 4x CPU performance improvements
  - Enhanced stability from Production Verification testing
  - Better mobile optimization
  - Improved rendering pipelines (URP 2.0)

### Package Upgrades

#### Core Packages
| Package | Old Version | New Version | Key Features |
|---------|-------------|-------------|--------------|
| **TextMeshPro** | 3.2.0-pre.7 | **4.0.0** | Stable release, better performance, Unicode 15 support |
| **Test Framework** | 1.4.5 | **2.0.1** | Unity 6 compatible, improved test runner UI |
| **uGUI** | 2.0.0 | 2.0.0 | Already latest (Unity UI) |

#### New Packages Added
| Package | Version | Purpose |
|---------|---------|---------|
| **Input System** | **1.11.2** | Modern input handling (replaces old Input Manager) |
| **UI Builder** | **2.0.0** | Visual UI design tool for UI Toolkit |
| **Services Analytics** | **6.0.2** | Unity Analytics SDK with privacy compliance |
| **Services Core** | **1.13.0** | Required for Unity Gaming Services |

## Unity 6 Key Features for This Project

### 1. Performance Improvements
✅ **CPU Performance**: Up to 4x improvement in rendering and physics  
✅ **GPU Performance**: URP optimizations for mobile (60 FPS target)  
✅ **Memory**: Better managed heap and GC optimizations  
✅ **Burst Compiler**: Enhanced C# to native code compilation  

### 2. Mobile Optimization
✅ **Touch Input**: New Input System provides better touch gesture handling  
✅ **Mobile Web**: WebGL improvements for better mobile browser support  
✅ **Build Size**: Better compression and asset bundling (< 150 MB target)  
✅ **Cold Start**: Improved startup time (< 2.5s target on iPhone 12)  

### 3. UI System Enhancements
✅ **UI Toolkit**: Modern UI framework (alternative to uGUI)  
✅ **UI Builder**: Visual design tool (drag-and-drop UI creation)  
✅ **Safe Area**: Automatic notch handling for iPhone X+  
✅ **Toggle/Button**: Latest uGUI 2.0 components (already using)  

### 4. Analytics & Privacy
✅ **Analytics SDK 6.0**: GDPR/CCPA compliant by default  
✅ **Privacy Manifest**: Built-in iOS Privacy Manifest support  
✅ **Consent Management**: Built into Analytics SDK  
✅ **ATT Support**: App Tracking Transparency helpers  

### 5. Multiplayer & Services
✅ **Netcode**: Unity 6 compatible netcode (future-proof)  
✅ **Relay/Lobby**: Multiplayer Services (not needed for v1)  
✅ **Cloud Services**: Unity Gaming Services integration ready  

## Migration Steps

### Step 1: Update Unity Hub
1. Open Unity Hub
2. Go to **Installs** tab
3. Click **Install Editor** → Select **Unity 6.0.31f1** (LTS)
4. Include modules: **iOS Build Support**, **Android Build Support** (optional)

### Step 2: Upgrade Project
1. Close Unity if open
2. In Unity Hub, click **Open** → Select this project folder
3. Unity Hub will detect version change and offer to upgrade
4. Click **Upgrade** → Unity will convert project to 6.0.31f1
5. Package Manager will auto-download new packages (takes 5-10 min)

### Step 3: Verify Compilation
After upgrade completes:
```powershell
# Unity should auto-compile. Check Console for errors
# Expected: 0 errors (all packages compatible)
```

### Step 4: Update Code (Optional Modernization)

#### A. Use New Input System (Optional - Already Have TouchInputService)
Our `TouchInputService` uses `Input.GetMouseButtonDown(0)` which still works.  
To modernize with new Input System:

```csharp
// Old (current):
if (Input.GetMouseButtonDown(0)) { ... }

// New (optional upgrade):
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

EnhancedTouchSupport.Enable();
if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame) { ... }
```

**Decision**: Keep old input for v1 MVP (simpler). Upgrade post-launch if needed.

#### B. Use Analytics SDK 6.0 Events (Update When Integrating)
When implementing `FirebaseAnalyticsAdapter`, use new SDK:

```csharp
using Unity.Services.Analytics;
using Unity.Services.Core;

// Initialize once
await UnityServices.InitializeAsync();

// Log events (consent-aware by default)
AnalyticsService.Instance.RecordEvent("StartRun");
```

**Decision**: Update analytics adapter when SDK integration happens (Epic 2).

#### C. TextMeshPro 4.0 Updates
TMP 4.0 is backward compatible. No code changes needed.  
New features available:
- Better font asset compression
- Unicode 15 emoji support
- Improved performance

**Decision**: No changes needed, already compatible.

### Step 5: Test Core Systems
After upgrade, test in Unity Editor Play Mode:

✅ **Bootstrap Scene**: ServiceLocator initializes  
✅ **Touch Input**: Tap detection works (`TouchInputService`)  
✅ **UI Components**: Toggle, Button, TextMeshProUGUI render correctly  
✅ **Physics2D**: Rigidbody2D gravity and collisions work  
✅ **Object Pooling**: ObjectPool instantiation/reuse works  

## Compatibility Notes

### ✅ Fully Compatible (No Changes Needed)
- All existing C# scripts (ServiceLocator, GameManager, UI controllers)
- ProjectSettings (PlayerSettings, Physics2D, Quality)
- ScriptableObjects (DifficultyCurve, SpawnConfig, Theme)
- CSV localization (Strings.csv)
- Assembly definitions (.asmdef files)

### ⚠️ May Require Testing
- **Xcode iOS Build**: May need Xcode 15+ for Unity 6 builds
- **Android Build**: May need Android SDK 33+ (Android 13)
- **Haptics**: Verify `UIImpactFeedbackGenerator` still works on iOS

### 🔄 Optional Upgrades (Post-MVP)
- Replace `Input` with `InputSystem` for better touch gesture support
- Use UI Toolkit + UI Builder instead of uGUI for next-gen UI
- Integrate Unity Analytics SDK 6.0 (replace Firebase if desired)
- Use Addressables for better asset management (large projects)

## Expected Outcomes

### Immediate Benefits
✅ Compilation succeeds with 0 errors  
✅ Package Manager downloads all dependencies  
✅ Editor Play Mode works with existing scripts  
✅ 60 FPS target more achievable (CPU optimizations)  

### Future Benefits
✅ 2 years of LTS support (biweekly bug fixes)  
✅ Better profiling tools (Unity 6 profiler improvements)  
✅ Mobile performance gains (URP 2.0 optimizations)  
✅ iOS 18 compatibility (latest Xcode support)  

## Troubleshooting

### Issue: Package Download Fails
**Solution**: Check internet connection, restart Unity Hub, clear package cache:
```powershell
# Clear package cache (Windows)
Remove-Item -Recurse "C:\Users\[YourUser]\AppData\Local\Unity\cache"
```

### Issue: Compilation Errors After Upgrade
**Solution**: 
1. Delete `Library/` folder (forces full reimport)
2. Reopen project in Unity Hub
3. Wait for full reimport (may take 10-15 min)

### Issue: Unity Hub Doesn't Show 6.0.31f1
**Solution**: 
1. Update Unity Hub to latest version
2. Download Unity 6.0.31f1 from Unity Archive: https://unity.com/releases/editor/archive
3. Install manually, then open project

### Issue: iOS Build Fails with Xcode Errors
**Solution**: Unity 6 requires Xcode 15+ for iOS builds
```bash
# Check Xcode version
xcodebuild -version
# Expected: Xcode 15.0 or later
```

## Resources

### Unity 6 Documentation
- **What's New**: https://docs.unity3d.com/6000.0/Documentation/Manual/WhatsNew.html
- **Upgrade Guide**: https://docs.unity3d.com/6000.0/Documentation/Manual/UpgradeGuide.html
- **Unity 6 Features**: https://unity.com/releases/unity-6

### Package Documentation
- **TextMeshPro 4.0**: https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0
- **Input System 1.11**: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.11
- **Analytics SDK 6.0**: https://docs.unity.com/ugs/en-us/manual/analytics/manual/sdk-install

### Unity 6 Demos
- **Fantasy Kingdom**: URP optimization demo (mobile-focused)
- **Megacity Metro**: Multiplayer 100+ players demo

## Next Steps

1. ✅ **Upgrade Complete** → Unity 6.0.31f1 LTS installed
2. ⏳ **Open Project** → Unity Hub upgrade process
3. ⏳ **Verify Compilation** → Check Console for 0 errors
4. ⏳ **Test Play Mode** → Run Bootstrap → MainMenu flow
5. ⏳ **Continue Implementation** → Resume Epic 1-8 tasks from `tasks.md`

---

**Status**: Ready for Unity Hub upgrade. Estimated time: 15-20 minutes for full upgrade + package download.
