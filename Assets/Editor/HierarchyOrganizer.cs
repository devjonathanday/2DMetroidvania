using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public static class HierarchyOrganizer
{
    static HierarchyOrganizer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null && gameObject.name.StartsWith("Player", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, new Color32(180, 0, 0, 255));
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name);
        }
        else if (gameObject != null && gameObject.name.StartsWith("GameManager", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, new Color32(128, 128, 128, 255));
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name);
        }
        else if (gameObject != null && gameObject.name.StartsWith("LevelManager", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, new Color32(0, 180, 0, 255));
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name);
        }
        else if (gameObject != null && gameObject.name.StartsWith("Camera", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, new Color32(0, 128, 255, 255));
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name);
        }
        else if (gameObject != null && gameObject.name.StartsWith("GlobalAudio", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, new Color32(180, 180, 0, 255));
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name);
        }
        else if (gameObject != null && gameObject.name.StartsWith("InputManager", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, new Color32(40, 40, 40, 255));
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name);
        }
        else if (gameObject != null && gameObject.name.StartsWith("Canvas", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, new Color32(80, 80, 80, 255));
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name);
        }
    }
}
#endif