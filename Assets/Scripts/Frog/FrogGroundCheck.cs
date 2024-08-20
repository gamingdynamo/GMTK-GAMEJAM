using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogGroundCheck : MonoBehaviour
{
    [SerializeField]
    private FrogBehaviour frogBehave;

    private int onGround = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("TonguableGround"))
        {
            onGround++;
            frogBehave.FrogOnGround(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground") || other.CompareTag("TonguableGround"))
        {
            onGround--;
            if (onGround <= 0)
            {
                frogBehave.FrogOnGround(false);
            }
        }
    }
}
