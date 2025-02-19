using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Stage : MonoBehaviour {

    public static Stage instance;
    void OnEnable() {
        instance = this;
    }

    public Vector2 size = new Vector2(16f, 9f);
    public float margin = 10f;

    public float BottomY => -size.y / 2f;
    public Vector3 Bottom => new Vector3(0f, BottomY, 0f);
    
    public Vector3 Clamp(Vector3 position) {
        float x = Mathf.Clamp(position.x, -size.x / 2f, size.x / 2f);
        float y = Mathf.Clamp(position.y, -size.y / 2f, size.y / 2f);
        return new Vector3(x, y, position.z);
    }
    public Vector3 Clamp(Transform transform) => transform.position = Clamp(transform.position);

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,  new Vector3(size.x + margin * 2f, size.y + margin * 2f, 0f));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, size.y, 0f));
    }

    public bool IsInside(Vector3 position) {
        float maxX = size.x / 2f;
        float maxY = size.y / 2f;
        return
            position.x > -maxX &&
            position.x < maxX &&
            position.y > -maxY &&
            position.y < maxY;
    }

    public bool IsInsideMargin(Vector3 position) {
        float maxX = size.x / 2f + margin;
        float maxY = size.y / 2f + margin;
        return
            position.x > -maxX &&
            position.x < maxX &&
            position.y > -maxY &&
            position.y < maxY;
    }
}
