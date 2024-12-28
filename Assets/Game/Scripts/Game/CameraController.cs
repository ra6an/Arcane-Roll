using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Animation Parameters Setup")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float transformDuration = 1f;
    [SerializeField] private Quaternion targetRotation;
    [SerializeField] private float rotationDuration = 1.5f;

    private void Awake()
    {
        playerController = PlayerController.Instance;
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        if(playerController == null) return;

        transform.SetLocalPositionAndRotation(playerController.transform.localPosition, playerController.transform.localRotation);
    }

    public void MoveCameraToTarget(System.Action onComplete = null)
    {
        this.transform.DOLocalMove(targetPosition, transformDuration)
           .SetEase(Ease.InOutQuad)
           .OnComplete(() =>
           {
               // Izvrši dodatni kod nakon završetka animacije
               onComplete?.Invoke();
           });

        // Rotiraj kameru prema ciljnoj rotaciji
        this.transform.DOLocalRotate(targetRotation.eulerAngles, rotationDuration)
            .SetEase(Ease.InOutQuad);
    }
}
