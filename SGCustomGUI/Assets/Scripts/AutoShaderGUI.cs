using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AutoShaderGUI : ShaderGUI
{
    // Change this to whatever keyword you want
    private const string ToggleKeyword = "has";
    private static readonly Dictionary<string, bool> Foldouts = new();
    private static bool ShowAdvancedOptions = false;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Dictionary<string, List<MaterialProperty>> categories = new();

        //------------------------------------------
        // Group properties by category
        //------------------------------------------

        foreach (var prop in properties)
        {
            if (IsInternalProperty(prop))
            {
                continue;
            }

            string category = GetCategory(prop.displayName);

            if (!categories.ContainsKey(category))
            {
                categories.Add(category, new List<MaterialProperty>());
            }

            categories[category].Add(prop);

            if (!Foldouts.ContainsKey(category))
            {
                Foldouts.Add(category, true);
            }
        }

        //------------------------------------------
        // Draw categories
        //------------------------------------------

        foreach (var category in categories.OrderBy(x => GetCategoryOrder(x.Key)))
        {
            string categoryName = category.Key;

            EditorGUILayout.Space(5);

            Foldouts[categoryName] = EditorGUILayout.Foldout(Foldouts[categoryName],categoryName,true);

            if (!Foldouts[categoryName])
            {
                continue;
            }

            EditorGUI.indentLevel++;

            //------------------------------------------
            // Find category toggle
            //------------------------------------------

            MaterialProperty toggleProp = null;

            foreach (var prop in category.Value)
            {
                if (IsInternalProperty(prop))
                {
                    continue;
                }

                
                if (IsToggleProperty(prop))
                {
                    toggleProp = prop;
                    break;
                }
            }

            //------------------------------------------
            // Draw toggle first
            //------------------------------------------

            if (toggleProp != null)
            {
                materialEditor.ShaderProperty(toggleProp,GetPropertyName(toggleProp.displayName));
            }

            //------------------------------------------
            // Hide category if toggle is disabled
            //------------------------------------------

            bool categoryEnabled = true;

            if (toggleProp != null)
            {
                categoryEnabled = toggleProp.floatValue > 0.5f;
            }

            if (categoryEnabled)
            {
                foreach (var prop in category.Value)
                {
                    if (IsInternalProperty(prop))
                    {
                        continue;
                    }

                    if (prop == toggleProp)
                    {
                        continue;
                    }

                    materialEditor.ShaderProperty(prop,GetPropertyName(prop.displayName));
                }
            }

            EditorGUI.indentLevel--;
        }

        //------------------------------------------
        // Advanced Options
        //------------------------------------------

        EditorGUILayout.Space(10);

        ShowAdvancedOptions = EditorGUILayout.Foldout(ShowAdvancedOptions,"Advanced Options", true);

        if (ShowAdvancedOptions)
        {
            EditorGUI.indentLevel++;

            materialEditor.EnableInstancingField();

            materialEditor.DoubleSidedGIField();

            materialEditor.RenderQueueField();

            EditorGUI.indentLevel--;
        }
    }

    //--------------------------------------------------
    // Helpers
    //--------------------------------------------------

    private static bool IsToggleProperty(
        MaterialProperty prop)
    {
        return prop.name.ToLower().Contains(ToggleKeyword.ToLower());
    }

    private static string GetCategory(string displayName)
    {
        int slash = displayName.IndexOf('/');

        if (slash < 0) return "99. Misc";

        return displayName.Substring(0, slash);
    }

    private static string GetPropertyName(string displayName)
    {
        int slash = displayName.IndexOf('/');

        if (slash < 0) return displayName;

        return displayName.Substring(slash + 1);
    }

    private static int GetCategoryOrder(string category)
    {
        int dot = category.IndexOf('.');

        if (dot < 0) return 999;

        string number = category.Substring(0, dot);

        if (int.TryParse(number, out int result))
        {
            return result;
        }

        return 999;
    }

    private static readonly HashSet<string> HiddenProperties = new()
    {
        "_QueueOffset",
        "_QueueControl",

        "_SrcBlend",
        "_DstBlend",

        "_ZWrite",
        "_Cull"
    };

    private static bool IsInternalProperty(
        MaterialProperty prop)
    {
        if (prop.name.StartsWith("unity_")) return true;

        return HiddenProperties.Contains(prop.name);
    }
}