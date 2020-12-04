using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



[System.Serializable]
public class OptionColor {
    public bool active;
    public Color color = Color.red;
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(OptionColor))]
    class MyPropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var activeWidth = 16;
            var activeRect = new Rect(position.x, position.y, activeWidth, position.height);
            var colorRect = new Rect(position.x + activeWidth, position.y, position.width - activeWidth, position.height);

            var activeProp = property.FindPropertyRelative("active");
            EditorGUI.PropertyField(activeRect, activeProp, GUIContent.none);
            if (activeProp.boolValue) {
                EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("color"), GUIContent.none);
            }

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
#endif
}

public static class SpawnerUtils {
    public static Mesh disc = new Mesh();
    static SpawnerUtils() {
        int step = 32;
        float radius = 0.5f;
        var vertices = new Vector3[step + 2];
        var triangles = new int[step * 3];
        for (int index = 0; index <= step; index++) {
            float angle = Mathf.PI * 2f * (float)index / (float)step;
            vertices[index + 1] = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle), 0f);
            if (index < step) {
                triangles[index * 3 + 0] = 0;
                triangles[index * 3 + 1] = 1 + index + 1;
                triangles[index * 3 + 2] = 1 + index;
            }
        }
        disc.vertices = vertices;
        disc.triangles = triangles;
        disc.normals = Enumerable.Repeat(Vector3.back, step + 2).ToArray();
    }
}

public class SequenceSpawnerLink : MonoBehaviour {
    public SequenceSpawner spawner;
}

[ExecuteInEditMode]
public class SequenceSpawner : MonoBehaviour {

    public Item prefab;
    public float radius = 0.5f;
    public float offset = 0;
    
    Sequencer sequencer;
    Sequence sequence;
    public Vector3 sequencerPosition;
    Vector3 sequencerPositionOld;

    public bool showLabel = true;
    public bool shortLabel = false;

    public OptionColor customColor = new OptionColor();

    public bool IsValid() => sequence != null && sequencer != null;
    
    void Init() {

        sequencer = GetComponentInParent<Sequencer>();
        sequence = GetComponentInParent<Sequence>();

        if (IsValid() == false) {
            return;
        }

        sequencerPositionOld = transform.localPosition + sequence.transform.localPosition;

        while (transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    void Start() {
        Init();
    }

    void Update() {

#if UNITY_EDITOR
        if (Application.isPlaying == false) {
            Init();
        }
#endif
        
        gameObject.name = string.Format("Spawner:{0}", prefab?.gameObject.name ?? "None");
        // sequencerPosition = transform.localPosition + sequence.transform.localPosition;
        sequencerPosition = sequencer?.transform.InverseTransformPoint(transform.position + transform.right * offset) ?? Vector3.zero;

        float max = sequencer?.triggerSize / 2 ?? 0f;
        if (sequencerPosition.y >= -max && sequencerPosition.y <= max) {
            if (sequencerPosition.x <= radius && sequencerPositionOld.x > radius) {
                if (Application.isPlaying) {
                    Spawn();
                }
            }
        }
    }

    void Spawn() {
        if (prefab != null) {
            var spawned = Instantiate(prefab, transform.position, Quaternion.identity);
            var link = spawned.gameObject.AddComponent<SequenceSpawnerLink>();
            link.spawner = this;
        }
    }

    void LateUpdate() {
        sequencerPositionOld = sequencerPosition;
    }

    IEnumerable<(Vector3, Vector3)> ChordAround(Vector3 center, float radius, int step = 32) {
        Vector3 A = center + Vector3.right * radius;
        for (int index = 0; index < step; index++) {
            float angle = Mathf.PI * 2f * ((float)index + 1f) / (float)step;
            Vector3 B = new Vector3(center.x + radius * Mathf.Cos(angle), center.y + radius * Mathf.Sin(angle), center.z);
            yield return (A, B);
            A = B;
        }
    }

    static string ShortLabel(string label) {
        int index = 0;
        int max = label.Length;
        List<char> chars = new List<char>();
        bool nextCharMustBeAdd = true;
        while (index < max) {
            char c = label[index++];
            if (nextCharMustBeAdd) {
                chars.Add(c);
                nextCharMustBeAdd = false;
            }
            else if (char.IsUpper(c)) {
                chars.Add(c);
            }
            else if (char.IsWhiteSpace(c)) {
                nextCharMustBeAdd = true;
            }
        }
        return new string(chars.ToArray());
    }
    
#if UNITY_EDITOR

    bool GetSelected() {

        var scope = transform;
        while (scope != null) {
            if (Selection.gameObjects.Contains(scope.gameObject)) {
                return true;
            }
            scope = scope.parent;
        }
        return false;
    }

    void OnDrawGizmos() {

        bool hidePassedSpawner = sequencer?.hidePassedSpawner ?? false;
        var gizmosColor = sequencer?.gizmosColor ?? Color.red;

        if (!hidePassedSpawner || sequencerPosition.x >= 0) {

            Gizmos.color = customColor.active ? customColor.color : gizmosColor;
            
            bool selected = GetSelected();
            var discPosition = transform.position + transform.right * offset;

            Gizmos.DrawLine(transform.position, discPosition);
            Gizmos.DrawSphere(transform.position, selected ? 0.15f : 0.1f);
            foreach (var (A, B) in ChordAround(discPosition, radius)) {
                Gizmos.DrawLine(A, B);
            }
            if (showLabel) {
                Vector3 cameraPos = Camera.current.WorldToScreenPoint(transform.position);
                if (cameraPos.z < 10f) {
                    string label = (prefab != null) ? prefab.name : "None";
                    if (shortLabel) {
                        label = ShortLabel(label);
                    }
                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Gizmos.color;
                    Handles.Label(transform.position, label, style);
                }
            }

            Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, selected ? 0.1f : 0f);
            Gizmos.DrawMesh(SpawnerUtils.disc, discPosition, Quaternion.identity, Vector3.one * radius / 0.5f);
        }
    }
#endif

#if UNITY_EDITOR
    [CustomEditor(typeof(SequenceSpawner))]
    class MyEditor : Editor {
        SequenceSpawner Target => target as SequenceSpawner;
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            bool show = Target.transform.childCount == 0;
            string label = show ? "Show Prefab" : "Hide Prefab";
            if (show) {
                var enabled = GUI.enabled;
                GUI.enabled = Target.prefab != null;
                if (GUILayout.Button(label)) {
                    var preview = Instantiate(Target.prefab, Target.transform.position, Quaternion.identity, Target.transform);
                    preview.gameObject.hideFlags = HideFlags.HideInHierarchy;
                }
                GUI.enabled = enabled;
            } else {
                if (GUILayout.Button(label)) {
                    DestroyImmediate(Target.transform.GetChild(0).gameObject);
                }
            }

            EditorGUILayout.LabelField(string.Format("Sequencer found: {0}", Target.sequencer));
        }
    }
#endif
}
