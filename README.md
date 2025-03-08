<p align="center">
  <img align="center" width="400" alt="osu! logo" src=".github/assets/logo.png">
</p>

# osu! patcher

Apply several fixes to osu! make playing Relax more enjoyable.

This is for use in offline play or private servers that allow modifications **only**.
Use at your own risk! Modifications are disallowed on most servers, even though this project this 
*does not provide an unfair advantage*.

Using this on official Bancho servers WILL get you banned.

## Features

### Relax
- Show misses on hit objects while playing Relax
- Re-enable combobreak sounds with Relax enabled
- Save all Relax scores to local leaderboards automatically
- Allow failing with Relax enabled
- Re-enable the low hp glow (with shaders on)

<sup>Note: Relax refers to Relax *or* Autopilot</sup>

### PP
- Show a live pp counter during gameplay and replays
- Switch between Bancho and ~~Akatsuki~~ (soon) pp calculators
- ~~Show PP on local leaderboards~~ (soon)

### Mods
- Always faintly show active mods during gameplay
- Auto restart when failed due to Sudden Death mod

### UI
- Increase the thumbnail opacity in song select
- Allow opening settings during gameplay with Ctrl+O
- Other miscellaneous fixes

<!-- ### Other -->
<!-- - Download from beatmap mirrors when offline -->

## Usage

Until I have a release ready, then go to the [latest actions build](ehttps://github.com/rushiiMachine/osu-patcher/actions?query=branch%3Amaster) 
for the `master` branch and download the attached artifact to extract. No automatic updater is included.

Only the `Stable` release stream is officially supported! `Cutting Edge` and `Beta` release streams
may have changes that cause errors or crashes. No support will be provided.

Latest tested `Stable` version: [`b20250309.2`](https://osu.ppy.sh/home/changelog/stable40/20250309.2).

Your antivirus may detect it as malware, however this is completely expected as it contains code to inject 
into processes. If you aren't convinced it isn't a false positive, feel free to build from source code.

## Compiling

1. Install the .NET SDK 8, the .NET Framework 4.5.2 developer pack, and Rust (rustup/cargo).
2. Run `dotnet build Osu.Patcher.Injector -c Release`
3. Output will be located in `./Osu.Patcher.Injector/bin/Release/net8.0/`

## How

This uses [ManagedInjector](https://github.com/holly-hacker/ManagedInjector) to inject a .NET DLL into an osu! process, which uses [Harmony](https://github.com/pardeike/Harmony) to hook methods/
rewrite IL instructions. To find obfuscated methods, "signatures" based on a portion of the IL instructions from the 
target method are used to locate it, and then patch it. Since this method doesn't rely on neither the Eazfuscator 
obfuscation key nor the obfuscated names, it should work on any version with matching IL even if the method names change.

## is this okay?

This was initially made after the Akatsuki private server's patcher broke for multiple months and no alternative
existed to fix the issues listed above. This project does not and never intends to bypass the
anti-cheat built into osu! (to allow modifications), and for that reason this project is only usable when osu! is
launched with a custom `-devserver` (for offline play, something like `-devserver example.com`).

I like improving the games I like (having submitted multiple PRs to osu!lazer as well), and given stable is essentially
dead in terms of bug-fixes and features I decided to write a utility to make the game I like better.
This is not a cheat, I don't make cheats.
