using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Update()
    {
        if (player != null)
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
