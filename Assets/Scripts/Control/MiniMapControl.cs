using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapControl : SingleTon<MiniMapControl>
{
    public GameObject Player;
    public float MapHeight;

    private void Start()
    {
        FollowPlayerPosition();
        FollowPlayerRotation();
    }

    public void FollowPlayerRotation()
    {
        
        transform.rotation = Quaternion.Euler(90, Player.transform.rotation.y*360, 0);
    }

    public void FollowPlayerPosition()
    {
        transform.position = new Vector3(Player.transform.position.x, MapHeight,Player.transform.position.z);
    }
}
