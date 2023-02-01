using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public partial class EditorMapModel : Editor
{
    public class GeneratePanel : InternalPanel<MapModel>
    {
        public GeneratePanel(MapModel script) : base(script)
        {
        }
        int iteration = 3000;
        bool isFoldEtc;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                EditorGUILayout.LabelField("[Generate]", EditorStyles.boldLabel);
                iteration = EditorGUILayout.IntField("Iteration", iteration);

                int szCount = script.StartPos.Count;
                int szRoute = script.SolveRoute.Count;
                int sz = Mathf.Min(szCount, szRoute);
                if (sz > 0)
                {
                    void DrawSolution(int i)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        {
                            EditorGUILayout.LabelField($"[Start Position] : {script.StartPos[i]}");
                            EditorGUILayout.LabelField($"[Solving Route] : {script.SolveRoute[i]}");
                        }
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.BeginVertical("HelpBox");
                    {
                        EditorGUI.indentLevel++;
                        DrawSolution(0);

                        isFoldEtc = EditorGUILayout.Foldout(isFoldEtc, "[ETC]");
                        if (isFoldEtc)
                        {
                            for (int i = 1; i < sz; i++)
                                DrawSolution(i);
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.BeginHorizontal();
                {
                    if (script._isProgress) GUI.enabled = false;
                    if (GUILayout.Button("Generate Map"))
                        script.GenerateMap(iteration);
                    GUI.enabled = true;
                    if (GUILayout.Button("Clear Map")) script.ClearMap();
                }
                EditorGUILayout.EndHorizontal();

                Rect rect = EditorGUILayout.BeginVertical();
                {
                    EditorGUI.ProgressBar(rect, script._progressGenerateMap, "Generating...");
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
