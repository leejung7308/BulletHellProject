using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player.transform.position.x > 0 && player.transform.position.x < 100)
        {
            gameObject.transform.position= new Vector3(player.transform.position.x,0,-10);
        }
        else
        {
            if(player.transform.position.x <= 0)
            {
                gameObject.transform.position = new Vector3(0, 0, -10);
            }
            else if (player.transform.position.x >= 100)
            {
                gameObject.transform.position = new Vector3(100, 0, -10);
            }
        }
    }
}
