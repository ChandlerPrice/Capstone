using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<AICarController> aICarControllers;
    public SimpleCarController player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(AICarController ai in aICarControllers)
        {
            if(ai.wayInt < (player.wayInt-1))
            {
                ai.transform.position = new Vector3(player.waypoints[(player.wayInt - 2)].transform.position.x, (player.waypoints[(player.wayInt - 2)].transform.position.y + Random.Range(0, 100)), player.waypoints[(player.wayInt - 2)].transform.position.z);
                ai.transform.rotation = player.waypoints[(player.wayInt - 2)].transform.rotation;
                ai.wayInt = (player.wayInt - 1);
                ai.currentWaypoint = player.waypoints[ai.wayInt];
                ai.m_steeringAngle = 0;
                ai.brakeAccel = 1;
            }
        }
    }
}
