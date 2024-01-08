# osu-patcher (WIP)

Enable several features that peppy goddamn disabled while playing with the Relax/Autopilot mods.


This is for use in offline play or private servers <ins>that allow modifications **only**</ins>. **Use at your own discretion!!**\
Modifications are disallowed on most servers, even though this project this *does not provide an unfair advantage*.

## Features

- **Show misses on hit-objects while playing Relax.**
- Automatically save Relax scores locally
- Enable failing while playing Relax.
- ~~Show the real score while playing Relax and on local scores. (Reverts 0.0x score multiplier client-side)~~ soon™️
- ~~Remove Sudden Death/Perfect mod incompatibility with Relax client side~~ soon™️

<sup>Relax refers to Relax *or* Autopilot</sup>

## Usage

soon™️ once I get actions working

## Compiling

1. Install the .NET SDK in addition to the .NET Framework 4.6.2 developer pack.
2. Run `dotnet restore`
3. Run `dotnet build OsuInjector -c Release /p:Platform=x86` !!! Make sure to not use a Unix shell
4. Output will be in `OsuInjector\bin\x86\Release\net462\` (osu!.patcher.exe)

## How

This uses [ManagedInjector](https://github.com/holly-hacker/ManagedInjector) to inject a .NET DLL into an osu! process, which uses [Harmony](https://github.com/pardeike/Harmony) to hook methods/
rewrite IL instructions. To find obfuscated methods, "signatures" based on a portion of the IL instructions from the 
target method are used to locate it, and then patch it. Since this method doesn't rely on neither the Eazfuscator 
obfuscation key nor the obfuscated names, it should work on any version even if the method names change.

## is this okay?

This was initially made after the Akatsuki private server team's patcher broke for multiple months and didnt provide
any alternative to fix the issues listed [above](#features). This project does not and never intends to bypass the
anti-cheat built into osu! (to prevent modifications), and for that reason this project is only usable when osu! is
launched with a custom `-devserver` (for offline play, something like `-devserver example.com`).

I like improving the games I like (ref: multiple PRs to lazer), and given stable is essentially dead in terms of
bug-fixes and features (cough cough [#features](#features)) I decided to write a private utility to make the game I 
like better. I am not a cheat developer, end of story.
