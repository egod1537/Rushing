using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

using TurnData = MapBrain.TurnData;
public partial class EntityMapBrain : Editor
{
    public class QueueListPanel : InternalPanel<MapBrain>
    {

        private Dictionary<MapTurnType, UnityAction<TurnData>> panels;
        public QueueListPanel(MapBrain script) : base(script)
        {
            panels = new Dictionary<MapTurnType, UnityAction<TurnData>>();
            panels.Add(MapTurnType.Move, MovePanel);
            panels.Add(MapTurnType.Attack, AttackPanel);
            panels.Add(MapTurnType.Skill, SkillPanel);
            panels.Add(MapTurnType.Create, CreatePanel);
            panels.Add(MapTurnType.Death, DeathPanel);
        }
        bool isFold;
        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            {
                EditorGUI.indentLevel++;

                EditorGUILayout.BeginHorizontal();
                {
                    isFold = EditorGUILayout.Foldout(isFold, "[Queue]");
                    EditorGUILayout.IntField(script.processQueue.Count, GUILayout.Width(64));
                }
                EditorGUILayout.EndHorizontal();

                if (isFold)
                {
                    foreach (var data in script.processQueue)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        {
                            panels[data.turnType](data);
                        }
                        EditorGUILayout.EndVertical();
                    }
                }

                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
        }

        public void MovePanel(TurnData data)
        {
            Entity entity = data.caster.entity;
            Vector2Int pos = entity.pos, to = data.targetPosition;
            EditorGUILayout.LabelField($"[Move] | ({pos.x},{pos.y}) -> ({to.x},{to.y})");
        }
        public void AttackPanel(TurnData data)
        {

        }
        public void SkillPanel(TurnData data)
        {

        }
        public void CreatePanel(TurnData data)
        {
            Vector2Int to = data.targetPosition;
            EditorGUILayout.LabelField($"[Create] | {data.id} ({to.x},{to.y})");
        }
        public void DeathPanel(TurnData data)
        {
            Entity entity = data.caster.entity;
            Vector2Int to = entity.pos;
            EditorGUILayout.LabelField($"[Death] | {entity.id} ({to.x},{to.y})");
        }
    }
}
