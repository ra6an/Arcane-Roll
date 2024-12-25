using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Animation Parameters Setup")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float transformDuration = 1f;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private float rotationDuration = 1.5f;
    public void MoveCameraToTarget(System.Action onComplete = null)
    {
        this.transform.DOMove(targetPosition, transformDuration)
           .SetEase(Ease.InOutQuad)
           .OnComplete(() =>
           {
               // Izvrši dodatni kod nakon završetka animacije
               onComplete?.Invoke();
           });

        // Rotiraj kameru prema ciljnoj rotaciji
        this.transform.DORotate(targetRotation.eulerAngles, rotationDuration)
            .SetEase(Ease.InOutQuad);
    }
}
