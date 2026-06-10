# Custom GUI for Shader Graph
Custom ShaderGUI for Unity Shader Graph that automatically:
- Creates collapsible categories from property names.
- Hides category properties using boolean toggles.
- Orders categories using numeric prefixes.
- Removes Unity internal properties from the Inspector.
- Provides a cleaner material workflow inspired by Unreal Engine's Material Editor.

# Features
## Automatic Categories
Properties are grouped automatically using the following format:
```
01. Texture/Base Color
01. Texture/Base Map

02. Normal/Normal Map
02. Normal/Strength

03. Emission/Color
03. Emission/Intensity
```

Result:
```
▼ 01. Texture
    Base Color
    Base Map

▼ 02. Normal
    Normal Map
    Strength

▼ 03. Emission
    Color
    Intensity
```

# Toggle Categories
| Display Name | Reference |
|--------------|-----------|
| 01. Texture/Has Texture  | _hasTexture | 
| 01. Texture/Base Color  | _BaseColor | 
| 01. Texture/Base Map  | _BaseMap | 


When the toggle is disabled:
```
▼ 01. Texture
    Has Texture
```

When enabled:
```
▼ 01. Texture
    Has Texture
    Base Color
    Base Map
```

## Ordered Categories
```
01. Surface
02. Texture
03. Normal
04. Emission
05. Outline
06. Dissolve
```

Result:
```
▼ 01. Surface
▼ 02. Texture
▼ 03. Normal
▼ 04. Emission
▼ 05. Outline
▼ 06. Dissolve
```

## Hidden Unity Internal Properties

The inspector automatically hides Unity internal properties such as:
```
_QueueOffset
_QueueControl

unity_Lightmap
unity_LightmapsInd
unity_ShadowMask
```

# Shader Setup

Add a Custom Editor to your generated shader:
```
CustomEditor "AutoShaderGUI"
```

# Naming Convention
## Categories
Use the following format:
```
<Category>/<Property Name>
```

Example:
```
01. Texture/Base Color
01. Texture/Base Map

02. Normal/Normal Map
02. Normal/Strength
```

## Toggle Properties
By default, any property whose reference contains:
```
has
```

is treated as a category toggle.
Example:
```
_hasTexture
_hasNormal
_hasEmission
```

The keyword can be changed in the script:
```C#
private const string ToggleKeyword = "has";
```

# Requirements
- Unity 6+
- Shader Graph
- URP or HDRP

# License
This project is licensed under the MIT License.
The source code and assets included in this repository are provided under the terms of the MIT License.
Unity and related trademarks are the property of Unity Technologies.
