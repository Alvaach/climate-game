# climate-game
Game about climate and sustainability, 2D

wassup

## Setup

### Unity Smart Merge

Unity Smart Merge prevents conflicts in Unity YAML files (scenes, prefabs, assets). The `.gitattributes` file is already configured — each team member just needs to register the merge driver once on their machine.

1. Find your Unity version:
   ```
   ls "C:/Program Files/Unity/Hub/Editor/"
   ```

2. Register the merge driver (replace `<VERSION>` with your Unity version, e.g. `2022.3.45f1`):
   ```
   git config --global merge.unityyamlmerge.name "Unity SmartMerge"
   git config --global merge.unityyamlmerge.driver "'C:/Program Files/Unity/Hub/Editor/<VERSION>/Editor/Data/Tools/UnityYAMLMerge.exe' merge -p %O %B %A %A"
   git config --global merge.unityyamlmerge.recursive binary
   ```

