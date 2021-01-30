using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionDebugUI : MonoBehaviour {
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 2);
    }
}
