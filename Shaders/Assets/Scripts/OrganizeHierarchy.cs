using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

/// <summary>
/// Highlights specific objects in the Hierarchy with custom background and text colors.
/// Set highlighted objects in the hierarchy to editor only.
/// </summary>
[InitializeOnLoad]
public class HierarchyObjectColor
{
    // Define the mapping of object names to their colors.
    private static readonly Dictionary<string, (Color backgroundColor, Color textColor)> ObjectColors = new Dictionary<string, (Color, Color)>()
    {
        { "Gizmos", (new Color(0.5f, 0.3f, 0.8f), Color.white) },
        { "UI", (new Color(0.2f, 0.4f, 0.8f), Color.white) },
        { "Environment", (new Color(0.2f, 0.6f, 0.1f), Color.white) },
        { "Enemy", (new Color(0.8f, 0.3f, 0.2f), Color.white) }
    };

    private static readonly Vector2 Offset = new Vector2(20, 1);

    static HierarchyObjectColor()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null || !ObjectColors.ContainsKey(obj.name))
            return; // Skip if the object is null or not in the mapping.

        var (backgroundColor, textColor) = ObjectColors[obj.name];

        // Draw background color
        Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);
        EditorGUI.DrawRect(bgRect, backgroundColor);

        // Initialize GUIStyle inside the method to ensure it is created after Unity's skin is loaded
        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.Bold,
            normal = { textColor = textColor }
        };

        // Draw object name with the correct style
        Rect textRect = new Rect(selectionRect.position + Offset, selectionRect.size);
        EditorGUI.LabelField(textRect, obj.name, labelStyle);
    }
}
#endif
