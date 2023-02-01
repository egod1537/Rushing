using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using static MapProbabilityTable;

public partial class EditorMapProbabilityTable : Editor
{
    public class ObjectPanel : InternalPanel<MapProbabilityTable>
    {
        public ObjectPanel(MapProbabilityTable script) : base(script)
        {
            foldPanels = new Dictionary<MapEntityLayer, bool>();
            foreach (MapEntityLayer layer in Enum.GetValues(typeof(MapEntityLayer)))
                foldPanels.Add(layer, false);
        }
        Dictionary<MapEntityLayer, bool> foldPanels;
        MapEntityID selectedID;
        MapEntityLayer selectedLayer;
        float percent;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Object Probability]", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical("Box");
                {
                    EditorGUILayout.LabelField("[Setting]", EditorStyles.boldLabel);

                    selectedLayer = (MapEntityLayer)EditorGUILayout.EnumPopup("Layer", selectedLayer);
                    selectedID = (MapEntityID)EditorGUILayout.EnumPopup("ID", selectedID);
                    percent = EditorGUILayout.Slider("Percent", percent, 0.0f, 1.0f);

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Add"))
                        {
                            if (!script.db[selectedLayer].ContainsKey(selectedID))
                                script.db[selectedLayer].Add(selectedID, percent);
                        }
                        if (GUILayout.Button("Normalize"))
                            script.Normalize();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                foreach (MapEntityLayer layer in Enum.GetValues(typeof(MapEntityLayer)))
                {
                    EditorGUILayout.BeginVertical("HelpBox");
                    {
                        EditorGUI.indentLevel++;
                        foldPanels[layer] = EditorGUILayout.Foldout(foldPanels[layer], $"[{layer}]");
                        if (foldPanels[layer]) DrawPanel(script.db[layer]);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }

        public void DrawPanel(PercentDictionary db)
        {
            EditorGUILayout.BeginVertical("Box");
            {
                PercentDictionary tempDictionary = new PercentDictionary();
                foreach (var value in db)
                {
                    MapEntityID id = value.Key;

                    Rect rect = EditorGUILayout.BeginVertical("HelpBox");
                    {
                        Texture2D previewTex = AssetPreview.GetAssetPreview(MapEntityDB.Load(id));
                        Rect rextTex = new Rect(rect)
                        {
                            x = rect.x + 8,
                            y = rect.y + 8,
                            width = 64,
                            height = 64
                        };
                        if (previewTex != null)
                            GUI.DrawTexture(rextTex, ScaleTexture(previewTex, 64, 64));

                        EditorGUILayout.BeginHorizontal();
                        {
                            EditorGUILayout.Space(72);
                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.EnumPopup("ID", value.Key);
                                float per = EditorGUILayout.Slider("Percent", value.Value, 0.0f, 1.0f);
                                if (!GUILayout.Button("Delete"))
                                    tempDictionary.Add(value.Key, per);
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }
                db.CopyFrom(tempDictionary);
            }
            EditorGUILayout.EndVertical();
        }
        private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }
    }
}
