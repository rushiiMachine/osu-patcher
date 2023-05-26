# osu-patcher (WIP)

Enable several features that peppy goddamn disabled while playing with the Relax/Autopilot mods.


This is for use on <ins>private servers that allow modifications **only**</ins>. **Use at your own discretion!!**\
Modifications are disallowed on most servers, even though *this does not provide an unfair advantage*.

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

## How

This uses [ManagedInjector](https://github.com/holly-hacker/ManagedInjector) to inject a .NET DLL into an osu! process,
which uses [Harmony](https://github.com/pardeike/Harmony) to hook and/or rewrite IL instructions. To find obfuscated methods,
"signatures" based on a portion of the IL instructions from the target method are used to locate it, and then patch it.
Since this method doesn't rely on neither the Eazfuscator encryption key nor the obfuscated names, it should work on all
versions assuming the method bodies did not change.

## heyyyyy

peppy please don't strike me down I promise you can't use this on bancho :3
