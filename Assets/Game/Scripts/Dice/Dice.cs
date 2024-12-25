using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private bool isRolling = false;
    private DiceSideDetector[] sides;

    [Header("Threshold")]
    [SerializeField] private int stableFrameCount = 0;
    [SerializeField] private int requiredStableFrames = 10;
    [SerializeField] private float velocityThreshold = 0.1f;
    [SerializeField] private float angularVelocityThreshold = 0.1f;

    private bool isDiceStopped = false;

    [Header("Force Values")]
    [SerializeField] private int forceOne = 10;
    [SerializeField] private int forceTwo = 15;
    [SerializeField] private int forceThree = 20;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sides = GetComponentsInChildren<DiceSideDetector>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !isRolling)
        {
            RollDice();
        }

        if(isRolling && !isDiceStopped)
        {
            if(rb.velocity.magnitude < velocityThreshold && rb.angularVelocity.magnitude < angularVelocityThreshold)
            {
                stableFrameCount++;
                if(stableFrameCount >= requiredStableFrames)
                {
                    isDiceStopped=true;
                    OnDiceStopped();
                }
            }
        } else
        {
            stableFrameCount = 0;
        }
    }

    private void RollDice()
    {
        isRolling = true;
        isDiceStopped = false;
        stableFrameCount = 0;

        Vector3 diceForce = new Vector3(Random.Range(-forceOne, forceOne), Random.Range(forceOne, forceThree), Random.Range(-forceOne, forceOne));
        Vector3 torqueForce = new Vector3(Random.Range(-forceTwo, forceTwo), Random.Range(-forceTwo, forceTwo), Random.Range(-forceTwo, forceTwo));

        rb.AddForce(diceForce, ForceMode.Impulse);
        rb.AddTorque(torqueForce, ForceMode.Impulse);
    }

    private void OnDiceStopped()
    {
        isRolling = false;
        SetSideTouchingGround();
    }

    public void SetSideTouchingGround()
    {
        foreach (var side in sides)
        {
            if(side.isTouchingTable)
            {
                DiceManager.Instance.SetRolledNumber(side.GetNumber());
            }
        }
    }
}
