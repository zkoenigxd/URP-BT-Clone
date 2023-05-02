using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject _player;

    private void Update()
    {
        if (_player != null)
        {
            transform.position = new(_player.transform.position.x, _player.transform.position.y, -150);
        }
    }
}
