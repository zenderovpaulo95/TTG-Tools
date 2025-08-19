# TTG Tools by Den Em

TTG Tools is a powerful utility for modifying files from Telltale Games, including texts (.landb, .langdb, and .dlog for *Sam & Max*, and .prop containing texts in *The Walking Dead*), textures (.d3dtx) for PC, Xbox 360, PS4, PS3, and Nintendo Switch, fonts (.font), as well as extracting and creating archive files (.ttarch and .ttarch2). It also allows decryption of .lua and .lenc files used by Telltale Games.

This version of TTG Tools includes some modifications and has been uploaded with the permission of Den Em.

## Introduction

TTG Tools can unpack/repack langdb, landb, font (The Walking Dead: Season 4, The Walking Dead Definitive Collection, and Sam & Max remasters vector fonts only) and dlog (only for Sam & Max: The Devil's Playhouse, a.k.a Season 3 original) files. It can also decrypt/encrypt older langdb, font, and d3dtx files—the tool knows which files need encryption, so if it didn't encrypt, it doesn't need it. This can be done with the "Auto (De)Packer" window.

To extract/repack files, put your files into the "Input" folder. Needed files can be found in the "Output" folder. TTG Tools can extract the following files into these formats:

- `*.langdb`, `*.dlog`, `*.landb` → `*.txt` or `*.tsv`
- `*.d3dtx` → `*.dds` or `*.pvr` (`*.pvr` converts iOS, PS Vita (requires (de)swizzle with other tools), and Android files with PowerVR graphics)
- `*.font` → `*.ttf` (newest games only)

With the "Font Editor" you can edit bitmap fonts (supports encryption/decryption of older font files).  
With the "Text Editor" you can create files with original and translated strings. You can also replace strings in newer text files by replacing a second file with doubled strings (instructions provided later).  
With the "Archive Packer" you can build and repack archives.  
With the "Archive Unpacker" you can unpack archives (select the correct game from the list for proper extraction and file listing).

## First Run

When you first run the tool you'll see the next message:

<img width="351" alt="First Run Message" src="https://github.com/user-attachments/assets/4d2eae73-1cb5-4cab-b9c2-4a867a987e2c">

That means you need to set up input/output folders and configure ASCII encoding. If you're using a non-Windows 1252 encoding in some older versions, you have to set up your characters manually. In the latest versions, you can just set "NOT normal Unicode" if you are translating *Game of Thrones* or *Tales from the Borderlands*. For later games, you can use the "Normal Unicode" option. If you don't know the ASCII code for your language, check "I don't know ASCII code for my language!" and select your language from the list.

### List ASCII encodings for countries:

- **1250**: Polish, Czech, Slovak, Hungarian, Slovene, Serbo-Croatian (Latin script), Montenegrin, Romanian, Albanian, English, German, Luxembourgish.
- **1251**: Russian, Ukrainian, Belarusian, Bulgarian, Serbian Cyrillic, Macedonian, Bosnian Cyrillic, Rusyn.
- **1252**: English, Irish, Italian, Norwegian, Portuguese, Spanish, Swedish, German, Finnish, Icelandic, French, Faroese, Luxembourgish, Albanian, Estonian, Swahili, Tswana, Catalan, Basque, Occitan, Romansh, Dutch (except the Ĳ/ĳ character, substituted by IC/ÿ), and Slovene (except the č character, substituted by ç).
- **1253**: Greek, English
- **1254**: Turkish, English, Italian, French, German, Spanish, Portuguese, Danish, Swedish, Finnish, Norwegian, Luxembourgish, Tswana, Azeri (except the ə character, substituted by ä).
- **1255**: Hebrew, English
- **1256**: Arabic, Persian, Urdu, English, French (except capital letters with diacritics)
- **1257**: Estonian, Latvian, Lithuanian, (also supports Polish, Slovene, Swedish, Finnish, Norwegian, Danish, German, English, Māori)
- **1258**: Vietnamese, English, French, German, Spanish, Danish, Norwegian, Swedish, Finnish, Irish, Albanian, Luxembourgish, Tswana. With combining diacritics: Estonian, Italian, Portuguese, Yoruba, Guarani, Igbo, Devanagari transliteration

If you set up incorrect path for input/output folders you can get next message:

<img width="300" alt="Incorrect Path Message" src="https://github.com/user-attachments/assets/4510e7d2-8214-47c1-a0be-f07f25c0ef82">

When you set up input/output folders and encoding (in some versions you need set up your characters for "Tales from the Borderlands" and "Game of Thrones") you need restart application.

<img width="283" alt="Restart Message" src="https://github.com/user-attachments/assets/f6fb9a79-506c-4e80-a4a6-532105aa7a7a">

After restart you'll see next window:

<img width="282" alt="Main Window" src="https://github.com/user-attachments/assets/c24a25d9-0c52-49e6-bd8e-b1504d020e4a">

## How to use

### Auto(De)Packer

Auto(De)Packer – extract/replace `*.langdb`, `*.landb`, `*.dlog` (Sam and Max season 3 original only), `*.d3dtx` and `*.font` (for newest games) files.

Font editor – change bitmap fonts (you can replace coordinates and textures).

Archive packer – pack files into `*.ttarch` or `*.ttarch2` files.

Text editor – you can make text file with doubled strings (original + translated or original + original files). `*.txt` format supports only!

Settings – change some settings of the tool.

About – decryption of the tool and changes in versions.

<img width="818" alt="Auto(De)Packer Window" src="https://github.com/user-attachments/assets/3923c0ac-db80-4265-92c9-f4954c952c65">

In Auto(De)Packer window you can extract or replaces strings in `*.landb`, `*.langdb` and `*.dlog` files (dlog files for original's Sam and Max: The devil's playhouse only), extract/replace textures in `*.d3dtx` files and also you can decrypt/encrypt `*.landb`, `*.langdb`, `*.lua` and `*.d3dtx` files (in oldest `*.d3dtx` you can encrypt DDS header only (recommend if you want pack textures into archives) or full encrypt (recommend if you want just place your files nearby original resources) files). In method encryption you can set up 2 methods: "Versions 2-6" and "Versions 7-9". If you decrypt/encrypt files since "The Wolf among Us" you can set the second method. If you're encrypt lua files since game "Tales from the Borderlands" you need set up checkbox "Lua scripts for new engine". If you don't have encryption key from game list but you have an encryption key you can set option "Set custom key" and insert into nearby text box you custom key.

To extract/decrypt files just replace it into "Input" folder and press "Decrypt, Export" button. In output you'll get extracted/decrypted files (decrypt `*.font` and `*.lua` files only. Other files decrypt and automatically extract).

To replace files you need insert your modified files into "Input" folder. Make sure that `*.lndb`, `*.lngdb`, `*.d3dtx`, `*.dlog` (Sam and Max season 3 original only) and `*.font` (newest games only) files into "Input" folder too! Press "Encrypt, Pack, Import" button and you'll see the results in "Output" folder. Make sure that `*.d3dtx` and `*.dds/*.pvr`, `*.langdb/*.landb/*.dlog` and `*.txt/*.tsv`, `*.font` and `*.ttf` are in "input" folder to import files.

You can (de)swizzle textures for Nintendo Switch (supports textures DXT1 (BC1) and DXT5 (BC3) only. Supports latest versions TTG Tools only) and PS4 (not completely tested, some textures may crash game. Supports latest versions TTG Tools only).

To encrypt modified `*.lua` or `*.font` (oldest games) insert modified files into "Input" folder. Don't need original files. Press "Encrypt, Pack, Import" button and you'll see the results in "Output" folder.

If you encrypt files don't forget set up encryption key and method encryption!

In settings form Auto(De)Packer you can set next options:

<img width="442" alt="Auto(De)Packer Settings" src="https://github.com/user-attachments/assets/79670296-916e-4b6b-aa41-0d94aab09d14">

- "Use a real ID of text in LANDB, LANGDB files" recommend extract text with that option for voice actors only. Not recommend use that option for repack files.
- "Delete D3DTX after import" removes d3dtx files from "Input" folder after import `*.dds` or `*.pvr` files in Auto(De)Packer (optional).
- "Delete DDS after import" removes dds files from "Input" folder after import in Auto(De)Packer (optional).
- "Import actor names" replaces actor names in `*.landb`, `*.langdb` and `*.dlog` files after import from `*.txt` or `*.tsv` files (recommend to use by special technical issues like fix mistake FELCITY in episode 3 of Wallace and Gromit's grand adventure when instead Felicity's face you can see Wallace's face).
- "Sort strings" sorts duplicated strings (optional).
- "Clear messages in Auto (De)Packer" clears messages after new actions in Auto (De)Packer (optional. Supports since v.1.0.12).
- "Coding for new games (From 'Tales from the Borderlands' game)" this option need if your ASCII-coding is not 1252 (in oldest versions you need set up "Normal Unicode" if your ASCII coding is 1252. In newest versions it sets by default). That option need for "Tales from the Borderlands" and "Game of Thrones" only.
- Set save text format for `*.dlog/*.langdb/*.landb` files (in latest versions).
- "Ignore empty strings" ignores empty strings in `*.langdb/*.dlog/*.landb` files.

### Font editor

<img width="1010" alt="Font Editor" src="https://github.com/user-attachments/assets/7c2a3751-f968-4be2-a5ed-81598172378f">

Font editor can edit bitmap fonts. When you open font file, you'll see next parameters:

<img width="1010" alt="Font Editor Parameters" src="https://github.com/user-attachments/assets/9f1fbb39-5b88-4268-b38f-3c092e311435">

In first table you see textures and its height, width and texture size. In second table you'll see coordinates of textures. It might be different with fonts. In texture table you can left click button to select export textures, import textures and import font coordinates. In newest versions you can import coordinates and if textures nearby `*.fnt` file, tool can replace automatically it. Otherwise you need replace it yourself.

<img width="1008" alt="Font Editor Textures" src="https://github.com/user-attachments/assets/a511683b-95eb-44bd-b31a-98f9a1d508eb">

In coordinates table you can export and import coordinates of font (in newest versions you can export/import fonts in `*.fnt` format only and it can import automatically if will be nearby textures from `*.fnt` file) just right click on coordinates table and select option from context menu.

<img width="1009" alt="Font Editor Coordinates" src="https://github.com/user-attachments/assets/e266838f-5d2b-4d91-8f1c-bd6c8f28dd1c">

Since Poker Night 2 you can import/export fonts with Kerning or without Kerning coordinates. In oldest fonts Kerning doesn't support.
Font editor can export either PVR (if font from iOS, Android with PowerVR graphic chips or PS Vita) or DDS (other platforms). You can (de)swizzle textures for Nintendo Switch (supports textures DXT1 (BC1) and DXT5 (BC3) only. Supports latest versions TTG Tools only) and PS4 (not completely tested, some textures may crash game. Supports latest versions TTG Tools only). Coordinates exports in `*.fnt` format in latest versions TTG Tools.

### Archive packer

<img width="876" alt="Archive Packer" src="https://github.com/user-attachments/assets/c91b3399-438f-47cb-b0a4-d88c73961c43">

With Archive packer you can pack/repack archives `*.ttarch` and `*.ttarch2`. First you need set paths resources and output archive. Then you need know which version uses you game (for oldest ttarch files you need see in ttarchext tool. For newer games version 1 uses since The Wolf among us until Minecraft: Story mode, version 2 uses since Minecraft: Story mode). In TTARCH (old games) you can select next options:
"Compress archive" (since version 3 (versions 3, 4 uses zlib.net library)) compress archive if you need, but if files have encrypted header the tool automatically build archive without compression. For versions 8 you can select zlib or deflate compress algorithm (check zlib version for episode 4 Wallace and Gromit, other games use deflate version).
"Encrypt archive" encrypt headers or compressed archives.
If you're build archives for mobile platforms or a some consoles you can build archive without encryption lua files by click checkbox "Don't encrypt Lua". You can select encryption key from game list or set your custom key encryption. For games like "Tales of Monkey Island" (tested on PC) recommend check "Xmode (For some old archives)".

<img width="878" alt="Archive Packer Options" src="https://github.com/user-attachments/assets/4aa43ddf-7641-42ff-bef0-a07da0325d71">

For "TTARCH2 (newer games)" you can select version archive (version 1 uses since The Wolf among us until Minecraft: Story mode, version 2 uses since Minecraft: Story mode). If you're build archive for games since "Tales from the Borderlands" you need check "Encrypt Lua for new games" if you build archives for PC/Mac versions. For other platforms you can set "Don't encrypt Lua" and don't check "Encrypt Lua for new games".
After set options you can click "Build archive".
If you build ttarch files you can just replace your files into "Pack" folder.
If you build ttarch2 files you need make lua file for successful load. Before "Minecraft: Story mode" you have to use hex editor (PC/Mac/Android versions only but for other platforms you can edit with notepad). Since "Minecraft: Story mode" you can just decrypt lua file, edit it and encrypt it back by renaming lua file.

### Archive unpacker

<img width="977" alt="Archive Unpacker" src="https://github.com/user-attachments/assets/2268bef1-0d76-431f-94c6-57518609f872">

In latest version added archive packer form. In this form you can unpack see file list in archive and unpack files. You can filter file list with filter. Select from game list and then open ttarch/ttarch2 files.

<img width="983" alt="Archive Unpacker Loaded" src="https://github.com/user-attachments/assets/34068397-2e43-41e9-b49c-af367f1d7fe3">

When it loaded you can set next options:
1) Decrypt lua scripts if you want unpack decrypted scripts (tool doesn't decompile scripts).
2) Search files by name.

<img width="487" alt="Archive Unpacker Options 1" src="https://github.com/user-attachments/assets/a92060fd-81d7-43eb-ae60-5dd499ff75e6">

<img width="484" alt="Archive Unpacker Options 2" src="https://github.com/user-attachments/assets/8589504f-3715-4bb3-a4c5-08fb9ccd2d17">

You can unpack all files or selected files

<img width="507" alt="Unpack Options" src="https://github.com/user-attachments/assets/a5118f5c-d926-4ab2-938c-7b2efd6cb3b6">

After that you can modify it with Auto(De)Packer or Font editor.

## Download

<a href="https://github.com/zenderovpaulo95/TTG-Tools/releases/tag/v.1.14.9" style="display:inline-block;padding:10px 20px;background-color:#28a745;color:white;border-radius:5px;text-decoration:none;font-weight:bold;">⬇ Download TTG Tools v1.14.9</a>

## Compilation

For correct compilation, you need to download `zlib.net.dll` from [here](http://www.componentace.com/download/download.php?editionid=25) and place it in the `zlib` folder.

## Special Thanks

- Den Em for allowing the source code to be uploaded
- aluigi for the source code of `ttarchext`
- Taylor Hornby for the C# source code of Blowfish encryption
- gdkchan, Stella/AboodXD for the Nintendo Switch swizzle method
- daemon1 and tge for the PS4 swizzle algorithm
- Josh Tamely for the Oodle wrapper
- Hajin Jang for the Zlib wrapper
- Nemiroff for fixing a bug in the Font Editor
- Krisp for adding Xbox texture support and font editing with Swizzle

## Features

TTG Tools makes it easier to translate and modify Telltale Games and Skunkape Games. It currently supports (at least tested on) the following games:

- Telltale Texas Hold'em
- Bone: Out from Boneville
- Bone: The Great Cow Race
- Sam & Max: Save the World (2006)
- Sam & Max: Beyond Time and Space (2007)
- Sam & Max: The Devil's Playhouse (2010)
- Strong Bad's Cool Game for Attractive People
- Wallace & Gromit's Grand Adventures
- Tales of Monkey Island
- Hector: Badge of Carnage
- Puzzle Agent 1
- Poker Night at the Inventory
- Back to the Future: The Game
- Puzzle Agent 2
- Jurassic Park: The Game
- Law & Order: Legacies
- The Walking Dead: Season One
- Poker Night 2
- The Wolf Among Us
- The Walking Dead: Season Two
- Tales from the Borderlands (2015 & 2021 versions)
- Game of Thrones: The Telltale Series
- Minecraft: Story Mode - Season One
- The Walking Dead: Michonne
- Batman: The Telltale Series
- The Walking Dead: A New Frontier
- Marvel's Guardians of the Galaxy: The Telltale Series
- Minecraft: Story Mode - Season Two
- Batman: The Enemy Within
- The Walking Dead: The Final Season
- The Walking Dead: The Telltale Definitive Series
- Sam & Max: Save the World - Remastered
- Sam & Max: Beyond Time and Space - Remastered
- Sam & Max: The Devil's Playhouse - Remastered

