# osu-patcher (WIP)

Enable several features that peppy goddamn disabled while playing with the Relax/Autopilot mods.


This is for use on <ins>private servers **only**</ins> that allow patchers, such as [Akatsuki](https://akatsuki.gg/).\
*To use on servers other than Akatsuki, you will have to modify the Injector and recompile yourself!*

## Features
- **Show misses on hit-objects while playing Relax¹.**
- Automatically save Relax¹ scores locally
- Enable failing while playing Relax¹.
- ~~Show the real score while playing Relax¹ and on local scores. (Reverts 0.0x score multiplier client-side)~~ soon™️
- ~~Remove Sudden Death/Perfect mod incompatibility with Relax¹ client side~~ soon™️

¹: Refers to Relax *or* Autopilot.

## Usage

soon™️ once I get actions working

## Compiling

1. Install the .NET SDK in addition to the .NET Framework 4.6.2 developer pack.
2. Run `dotnet restore`
3. Run `dotnet build OsuInjector -c Release /p:Platform=x86` !!! Make sure to not use a Unix shell
4. Output will be in `OsuInjector\bin\x86\Release\net462\` (osu!.patcher.exe)

## heyyyyy

peppy please don't strike me down I promise you can't use this on bancho :3
