using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    bool CanSeePlayer()
    {
                var direction = player.position - npc.transform.position;
                var angle = Vector3.Angle(direction, npc.transform.position);
                return direction.magnitude < visDist && angle < visAngle;
    }
}
