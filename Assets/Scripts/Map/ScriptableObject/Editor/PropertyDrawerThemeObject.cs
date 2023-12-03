using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(ThemeObject))]
public class PropertyDrawerThemeObject : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, prop);

        var size = prop.FindPropertyRelative("size");
        var go = prop.FindPropertyRelative("go");
        var percent = prop.FindPropertyRelative("percent");

        GUILayout.BeginHorizontal();
        {
            Texture2D preview = AssetPreview.GetAssetPreview(go.objectReferenceValue);
            if (preview == null) preview = new Texture2D(32, 32);
            Rect previewRect = new Rect(position)
            {
                width = 64,
                height = 64
            };
            EditorGUI.DrawPreviewTexture(previewRect, preview);

            Rect sizeRect = new Rect(position)
            {
                x = position.x += 70.0f,
                width = position.width - 70.0f,
                height = EditorGUIUtility.singleLineHeight
            };
            size.vector2IntValue = EditorGUI.Vector2IntField(sizeRect, "Size", size.vector2IntValue);

            Rect goRect = new Rect(sizeRect)
            {
                y = sizeRect.y+EditorGUIUtility.singleLineHeight+2
            };
            go.objectReferenceValue = EditorGUI.ObjectField(goRect, "Tile", go.objectReferenceValue,
                typeof(GameObject), true);

            Rect percenRect = new Rect(goRect)
            {
                y = goRect.y + EditorGUIUtility.singleLineHeight+2
            };
            percent.floatValue = EditorGUI.Slider(percenRect, "percent", percent.floatValue, 0.0f, 1.0f);
        }
        GUILayout.EndHorizontal();
        EditorGUI.EndProperty();
    }
}
