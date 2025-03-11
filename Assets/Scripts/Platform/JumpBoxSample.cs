using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoxSample : MonoBehaviour
{
    [SerializeField] private float jumpPower;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("!23");
        if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            player.Jump(jumpPower);
        }
    }
}
