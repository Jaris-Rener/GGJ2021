using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionDebugUI : MonoBehaviour {
    public Transform transitionPoint;

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 2);

        if (transitionPoint != null) {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transitionPoint.position);
        }
    }
}
