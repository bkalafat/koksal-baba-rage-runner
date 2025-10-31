# Build Instructions for iOS

## Prerequisites
- Unity 6000.2.8f1 (Unity 6)
- Xcode 14.0 or later
- iOS 14.0+ deployment target
- Valid Apple Developer account

## Unity Build Settings
1. Open Unity Build Settings (File → Build Settings)
2. Select iOS platform
3. Configure settings:
   - Architecture: IL2CPP
   - Target minimum iOS Version: 14.0
   - Target SDK: Device SDK
   - Target Device: iPhone Only
   - Automatically Sign: Enable (select team)

## Build Steps
1. In Unity: File → Build Settings → Build
2. Choose output folder (e.g., `Build/iOS`)
3. Wait for build to complete (~5-10 minutes)
4. Open generated `.xcodeproj` in Xcode

## Xcode Post-Build Configuration
1. Add `PrivacyInfo.xcprivacy` to Xcode project (drag from Assets/Settings/)
2. Select project in navigator → Signing & Capabilities
   - Verify Team and Bundle ID
   - Add capability: Push Notifications (if using ad networks)
3. Build Settings:
   - Enable Bitcode: NO
   - Strip Debug Symbols During Copy: YES (Release only)
4. General → Frameworks, Libraries, and Embedded Content:
   - Verify all Unity frameworks are present
   - Add LevelPlay/AdMob SDK frameworks if using ads

## Testing
1. Build and Run on device via Xcode (⌘R)
2. Test cold start time (< 2.5s on iPhone 12)
3. Profile with Instruments (⌘I):
   - Check FPS (target 60 steady)
   - Check Memory (< 300 MB)
   - Check Time Profiler for GC spikes (< 2ms)

## Archive for App Store
1. Product → Archive
2. Wait for archiving to complete
3. Window → Organizer
4. Select archive → Distribute App
5. Choose App Store Connect
6. Upload and wait for processing (~30 minutes)

## Troubleshooting
- **Build size > 150 MB**: Check texture compression (ASTC 6x6), audio compression (Vorbis)
- **Crash on launch**: Check console logs, verify IL2CPP stripping level (Medium)
- **Ad SDK errors**: Verify frameworks are added, check adapter #define symbols
- **Privacy Manifest missing**: Ensure PrivacyInfo.xcprivacy is in Xcode project root
