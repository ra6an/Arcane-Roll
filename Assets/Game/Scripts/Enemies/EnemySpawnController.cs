using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [Header("Enemy Spawn Points")]
    [SerializeField] private Vector3[] spawnPosition = new Vector3[3];
    [SerializeField] private float gizmoRadius = 0.5f;
    [SerializeField] private Color gizmoColor = Color.red;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        foreach (Vector3 pos in spawnPosition)
        {
            Vector3 worldPosition = transform.position + pos;
            Gizmos.DrawSphere(worldPosition, gizmoRadius);
        }
    }
}
