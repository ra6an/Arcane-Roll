using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPointerHit : MonoBehaviour
{
    [SerializeField] private int maxDistance = 3000;
    [SerializeField] private LayerMask targetLayerMask;

    private void Update()
    {
        Raycasting();
    }

    public void Raycasting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, maxDistance, targetLayerMask))
        //{
        //    // Pogodak - crvena linija
        //    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);

        //    // Informacija o meti
        //    Debug.Log($"Pogodak: {hit.collider.name}");
        //}
        //else
        //{
        //    // Nema pogodaka - zelena linija
        //    Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.green);
        //}

        if (Physics.Raycast(ray, out hit, maxDistance, targetLayerMask)) {
            Transform root = hit.collider.transform.root;
            Damageable target = root.GetComponent<Damageable>();
            if (target != null) {
                Debug.Log(target.name);
            }
        }
    }
}

