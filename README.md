# EpicLoot VR Fix

A Valheim VR mod that fixes EpicLoot UI compatibility issues with Valheim VR (VHVR).

## Description

This mod fixes compatibility issues between **EpicLoot** and **Valheim VR Mod (VHVR)** where certain UI elements would not render correctly in VR.

Originally the mod fixed the **Enchanter Table UI**, but it now also fixes **EpicLoot item tooltips**, which were invisible in VR due to the way EpicLoot constructs its tooltip UI.

The mod uses Harmony patching to automatically adjust the UI hierarchy and rendering layers so that VHVR can correctly display these interfaces in VR.

## Features

- **Automatic UI Detection**  
  Detects when EpicLoot UI elements appear and ensures they are VR-compatible.

- **Enchanter Table VR Fix**  
  Ensures the EpicLoot Enchanter Table UI renders correctly in VR.

- **Tooltip Rendering Fix**  
  Fixes EpicLoot item tooltips that were previously invisible in VR.

- **VR Rendering Integration**  
  Ensures UI elements are correctly integrated with VHVR's VR GUI system.

- **VR Controller Support**  
  Allows normal VR interaction with EpicLoot UI.

- **Proper UI Layer Handling**  
  Ensures UI elements use the correct rendering layer required by VHVR.

## Installation

1. Install **BepInEx**
2. Install **Valheim VR Mod (VHVR)**
3. Install **EpicLoot**
4. Install this mod
5. Launch the game

## Configuration

The mod includes a config file with the following options:

- `Enable VR Fix` (default: true)  
  Enables or disables the VR UI fixes.

- `Debug Mode` (default: false)  
  Enables detailed logging for troubleshooting.

The config file is located at: `BepInEx/config/EpicLootVRFix.cfg`


## Compatibility

- Requires **Valheim VR Mod (VHVR)**
- Soft dependency on **EpicLoot**
- Should not conflict with other UI mods

## Troubleshooting

If UI elements are not visible in VR:

1. Ensure all dependencies are installed correctly.
2. Verify that EpicLoot works correctly in non-VR mode.
3. Enable **Debug Mode** in the config file.
4. Check the **BepInEx log** for messages from `EpicLootVRFix`.

## Source Code

This mod uses **Harmony patching** to safely integrate with both EpicLoot and the Valheim VR Mod without modifying either mod directly.

## Changelog

### v1.0.0
- Initial release
- Basic VR UI rendering fix
- VR controller input support
- Configurable debug mode

### v1.0.1
- Fixed EpicLoot tooltip rendering in VR (Tested with EpicLoot v0.12.11)
- Removed nested tooltip canvas created by Jotunn
- Corrected UI layer so VHVR can render tooltip text

