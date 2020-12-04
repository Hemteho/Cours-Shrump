using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Sequencer : MonoBehaviour {

    public float triggerSize = 30f;
    public float velocity = 2f;
    public float timeScale = 1f;

    public Color gizmosColor = new Color(1f, 0.5f, 1f);
    public bool hidePassedSpawner = true;

    void DrawArrow(Vector3 position, float size) {
        Gizmos.DrawLine(position, position + new Vector3(size, size, 0f));
        Gizmos.DrawLine(position, position + new Vector3(size, -size, 0f));
    }

    void OnDrawGizmos() {
        Gizmos.color = gizmosColor;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(0f, triggerSize + 2f, 2f));
        
        int max = Mathf.FloorToInt(triggerSize / 2f / 2f);
        for (int i = -max; i <= max; i++) {
            DrawArrow(new Vector3(0f, i * 2f, 0f), 0.5f);
        }

        Gizmos.color = new Color(gizmosColor.r, gizmosColor.g, gizmosColor.b, 0.25f);
        Gizmos.DrawCube(Vector3.zero, new Vector3(0f, triggerSize + 2f, 2f));
    }
}
