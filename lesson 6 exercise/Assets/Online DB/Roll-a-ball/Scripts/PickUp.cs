using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    BoxCollider bc;
    MeshRenderer mr;

    private void Start()
    {
        bc = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
    }

    public IEnumerator ReSpawn()
    {
        bc.enabled = false;
        mr.enabled = false;
        yield return new WaitForSeconds(8);
        bc.enabled = true;
        mr.enabled = true;
    }

}
