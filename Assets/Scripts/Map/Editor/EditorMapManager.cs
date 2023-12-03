using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(MapManager), true)]
public class EditorMapManager : Editor
{
    MapManager script;

    TreasureCountPanel treasurePanel;
    PercentPanel percentPanel;
    ThemeLoaderPanel themeLoaderPanel;
    GeneratePanel generatePanel;
    private void OnEnable()
    {
        script = (MapManager)target;
        treasurePanel = new TreasureCountPanel(script);
        percentPanel = new PercentPanel(script);
        themeLoaderPanel = new ThemeLoaderPanel(script);
        generatePanel = new GeneratePanel(script);
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.BeginVertical("Box");
        {
            script.resourceTable.n = EditorGUILayout.IntField("Size", script.resourceTable.n);
            script.player = (MapEntity) EditorGUILayout.ObjectField(
                "Player", script.player, typeof(MapEntity));
            EditorGUILayout.LabelField($"Tresure : {script.treasure}");
            script.mapTheme = (SOMapTheme)EditorGUILayout.ObjectField(
                "MapTheme", script.mapTheme, typeof(SOMapTheme));
            themeLoaderPanel.Draw();
        }
        EditorGUILayout.EndVertical();
        generatePanel.Draw();

        EditorGUILayout.BeginVertical("HelpBox");
        {
            treasurePanel.Draw();
            script.resourceTable.mapType = (MapType)EditorGUILayout.EnumPopup(
                    "mapType", script.resourceTable.mapType);
            percentPanel.Draw();
        }
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        Undo.RecordObject(script, "Record Map Manager");
    }

    public class Panel
    {
        protected MapManager script;
        public Panel(MapManager target)
        {
            script = target;
        }
        public virtual void Draw() { }
    }
    public class GeneratePanel : Panel
    {
        public GeneratePanel(MapManager target) : base(target)
        {
        }
        public override void Draw()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUILayout.TextArea(ConsoleText(script.generatorRet));

                EditorGUILayout.BeginHorizontal();
                {
                    
                    if (GUILayout.Button("Generate")) script.GenerateMap();
                    if (GUILayout.Button("Generate Entity")) script.GenerateEntity();
                    if (GUILayout.Button("Remove")) script.RemoveMap();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Load Map")) script.LoadMap();
                }
                EditorGUILayout.EndHorizontal();

                Rect rect = EditorGUILayout.BeginVertical();
                EditorGUI.ProgressBar(rect, script.generatorRet.progress, "Progress");
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        private string ConsoleText(MapManager.MapGenerateResult ret)
            => $"Generating Time : {ret.time}ms\n" +
            $"Start Position {ret.startPos.x}, {ret.startPos.y}\n" +
            $"Path : {ret.path}";
    }

    public class TreasureCountPanel : Panel
    {
        public enum ShuffleType
        {
            Fixed,
            Random
        }
        public ShuffleType shuffleType;
        public int count;
        public int minCnt, maxCnt;

        public TreasureCountPanel(MapManager target) : base(target)
        {
        }

        public override void Draw()
        {
            shuffleType = (ShuffleType)
                EditorGUILayout.EnumPopup("Value Type", shuffleType);
            if (shuffleType == ShuffleType.Fixed)
            {
                script.resourceTable.treasureCount = EditorGUILayout.IntField(
                    "Treasure Count", script.resourceTable.treasureCount);
            }
            else
            {
                EditorGUILayout.BeginHorizontal("Box");
                {
                    minCnt = EditorGUILayout.IntField("min", minCnt);
                    EditorGUILayout.LabelField("~", GUILayout.Width(20));
                    maxCnt = EditorGUILayout.IntField("max", maxCnt);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    public class PercentPanel : Panel
    {
        public PercentPanel(MapManager target) : base(target)
        {
        }

        public override void Draw()
        {
            EditorGUILayout.BeginVertical("Box");
            {
                if (GUILayout.Button("Normalize")) script.resourceTable.Normalize();
                foreach(MapObjectType mo in Enum.GetValues(typeof(MapObjectType)))
                {
                    script.resourceTable.percent[mo] = 
                        EditorGUILayout.Slider(mo.ToString(), script.resourceTable.percent[mo], 0.0f, 1.0f);
                }
            }
            EditorGUILayout.EndVertical();
        }
    }

    public class ThemeLoaderPanel : Panel
    {
        public ThemeLoaderPanel(MapManager target) : base(target)
        {
        }
        bool foldPanel;
        int themeNum;
        string themeName;
        public override void Draw()
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("Box");
            foldPanel = EditorGUILayout.Foldout(foldPanel, "Theme Loader");
            if (foldPanel)
            {
                themeName = EditorGUILayout.TextField("Theme Name", themeName);
                themeNum = EditorGUILayout.IntField("Theme num", themeNum);
                if (GUILayout.Button("Load"))
                {
                    SOMapTheme ret = MapResource.LoadTheme(
                        themeName, script.resourceTable.mapType, script.size, themeNum);
                    if (ret == null) Debug.Log("This theme is not existed.");
                    else script.mapTheme = ret;
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }
    }
}
