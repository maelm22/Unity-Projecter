using UnityEngine;
using Random = UnityEngine.Random;

public class RepairPack : MonoBehaviour
{
    private void Update()
    {
        Rotation();
    }
    private void Rotation()
    {
        float movement = Random.Range(50, 100);
        transform.Rotate(new Vector3(movement, movement, movement) * Time.deltaTime, Space.Self);
    }

    

    
}