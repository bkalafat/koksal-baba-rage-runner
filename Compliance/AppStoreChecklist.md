# App Store Submission Checklist

Use this checklist to ensure compliance before submitting Köksal Baba: Rage Runner to the Apple App Store.

---

## ✅ Build and Technical

- [ ] **Build size < 150 MB:** Verify in Unity Build Report (target: 80-120 MB)
- [ ] **IL2CPP enabled:** Player Settings → iOS → Scripting Backend = IL2CPP
- [ ] **Minimum deployment target:** iOS 14.0 (Xcode project setting)
- [ ] **Target device:** iPhone only (no iPad builds at v1)
- [ ] **Orientation:** Portrait only (disable landscape in Player Settings)
- [ ] **64-bit architecture:** Verify arm64 in Build Settings
- [ ] **Code stripping:** Medium or High stripping level enabled

---

## ✅ Compliance and Privacy

- [ ] **PrivacyInfo.xcprivacy included:** Add to Xcode project, declare data collection
- [ ] **ATT prompt configured (if needed):** Info.plist NSUserTrackingUsageDescription set
- [ ] **No PII collection:** Verify no login, no device ID tracking
- [ ] **Privacy Policy created:** PrivacyPolicy.md included in submission materials
- [ ] **Third-Party Notices created:** ThirdPartyNotices.txt lists all SDKs
- [ ] **Analytics consent toggle:** Settings → Privacy Consent functional
- [ ] **NameRights.pdf attached:** Written permission for Köksal Baba and Riçıt IP usage

---

## ✅ App Store Connect Setup

- [ ] **App icon uploaded:** 1024x1024 PNG, no transparency, no rounded corners
- [ ] **Screenshots captured:**
  - [ ] 6.7" (iPhone 14 Pro Max): 1290x2796 pixels (required)
  - [ ] 5.5" (iPhone 8 Plus): 1242x2208 pixels (optional but recommended)
- [ ] **App Preview video (optional):** 15-30 second gameplay trailer
- [ ] **App Description written:**
  - [ ] tr-TR (Turkish) version
  - [ ] en-US (English) version
- [ ] **Keywords selected:** Relevant to runner, hyper-casual, comedic genres
- [ ] **Age rating set:** PEGI 7+ / ESRB Everyone (no violence, no profanity)
- [ ] **Pricing and availability:** Free with IAP (Remove Ads, Starter Pack)

---

## ✅ App Review Notes

- [ ] **IP authorization note added:** "We have written permission to use the names and likenesses of Köksal Baba and Riçıt. See attached NameRights.pdf in submission package."
- [ ] **Test account provided (if needed):** N/A (no login system)
- [ ] **Demo instructions:** "Tap screen to hop, collect yellow tokens to fill Rage Meter, tap when full to dash."

---

## ✅ Privacy Manifest (PrivacyInfo.xcprivacy)

Required fields:
- [ ] **NSPrivacyTracking:** YES (if ATT required for ad network)
- [ ] **NSPrivacyTrackingDomains:** List ad network domains (e.g., ["googlesyndication.com", "ironsrc.com"])
- [ ] **NSPrivacyCollectedDataTypes:** ["Analytics"] (if consent granted)
- [ ] **NSPrivacyCollectedDataTypeTracking:** NO (analytics are anonymous)

---

## ✅ Performance Validation

- [ ] **60 FPS steady:** Profiled for 3 minutes on iPhone 12 class device
- [ ] **Zero GC spikes > 2ms:** Profiler shows no GC.Alloc spikes during gameplay
- [ ] **Cold start < 2.5s:** Xcode Instruments Time Profiler validation
- [ ] **No crashes:** Full run (menu → game → results → revive → results) without exceptions
- [ ] **Memory < 300 MB:** Xcode Instruments Memory Profiler validation

---

## ✅ Functional Testing

- [ ] **Full gameplay loop works:** MainMenu → Playing → GameOver → Results
- [ ] **Interstitial ads show (mocked):** On GameOver, respects frequency cap
- [ ] **Rewarded video works (mocked):** Revive button grants one-time respawn
- [ ] **IAP purchase flow (mocked):** "Remove Ads" and "Starter Pack" trigger callbacks
- [ ] **Best score persists:** Across app restarts
- [ ] **Localization works:** tr-TR and en-US strings display correctly
- [ ] **Settings toggles work:** Sound, Haptics, Language, Privacy Consent
- [ ] **Cosmetics unlock:** With coins, equip persists across sessions

---

## ✅ Content Review

- [ ] **No realistic violence:** Death animations are comedic only
- [ ] **No offensive language:** All text reviewed for profanity (tr-TR and en-US)
- [ ] **No gambling mechanics:** No loot boxes, direct IAP purchases only
- [ ] **Family-friendly:** Content suitable for PEGI 7+ / ESRB Everyone rating

---

## ✅ Final Submission

- [ ] **Xcode Archive created:** Product → Archive in Xcode
- [ ] **IPA uploaded to App Store Connect:** Via Xcode Organizer or Transporter
- [ ] **Build processed:** Wait for App Store Connect to finish processing (15-30 minutes)
- [ ] **Submit for Review:** Select build, add App Review notes, click "Submit"

---

## ✅ Post-Submission

- [ ] **Monitor review status:** Check App Store Connect daily for status updates
- [ ] **Respond to review queries:** Reply within 24 hours if Apple requests clarification
- [ ] **Prepare for rejection scenarios:**
  - Missing PrivacyInfo.xcprivacy → Add and resubmit
  - IP concerns → Provide NameRights.pdf evidence
  - Performance issues → Profile and optimize, resubmit

---

## Contact for Questions

- Developer: [Your Company Name]
- Email: [support email]

---

**Notes:**
- Complete this checklist sequentially; do not skip steps.
- Keep NameRights.pdf and compliance documents in secure storage for future reference.
- Test on physical devices (iPhone XR, iPhone 12, iPhone 14 Pro) before submission.

**Last Updated:** [Date]
