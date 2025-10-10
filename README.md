# EpicLoot VR Fix

A Valheim VR mod that makes the EpicLoot Enchanter Table UI visible and functional in Valheim VR (VHVR).

## Description

This mod fixes the compatibility issue between EpicLoot and Valheim VR Mod (VHVR) where the Enchanter Table UI would not render properly in VR. It automatically detects when the Enchanter Table UI opens and integrates it with VHVR's rendering system, making enchanting items possible in VR gameplay.

## Features

- **Automatic UI Detection**: Detects when EpicLoot's Enchanter Table UI opens
- **VR Rendering Integration**: Forces the UI to render properly in VR using VHVR's rendering system
- **VR Controller Support**: Use VR menu/inventory buttons to close the UI
- **Proper Scaling**: Maintains correct UI scaling and positioning for VR
- **Safe Integration**: Clean integration with VHVR's existing VR GUI system

## Installation

1. Install [BepInEx]
2. Install [Valheim VR Mod (VHVR)]
3. Install [Epic Loot]
4. Install this mod
5. Launch the game

## Configuration

The mod includes a config file with the following options:

- `Enable VR Fix` (default: true) - Enable/disable the VR UI fixes
- `Debug Mode` (default: false) - Enable debug logging for troubleshooting

To modify config values, edit `BepInEx/config/EpicLootVRFix.cfg` or use the configuration manager mod.

## Usage

1. Play Valheim in VR with EpicLoot installed
2. Approach an Enchanter Table and interact with it
3. The UI should now be visible and properly positioned in VR
4. Use your VR controller's menu or inventory button to close the UI

## Compatibility

- Requires Valheim VR Mod (VHVR)
- Soft dependency on EpicLoot (mod will only activate if Epic Loot is present)
- Compatible with other UI mods that don't conflict with Epic Loot or VHVR

## Troubleshooting

If the Enchanter Table UI is not visible:

1. Ensure all dependencies are installed correctly
2. Check that EpicLoot is functioning in non-VR mode
3. Enable Debug Mode in the config file to see detailed logs
4. Check the BepInEx log for any error messages

## Source Code

The source code for this mod is available and uses Harmony patching for safe integration with both Epic Loot and VHVR.

## Changelog

### v1.0.0
- Initial release
- Basic VR UI rendering fix
- VR controller input support
- Configurable debug mode
