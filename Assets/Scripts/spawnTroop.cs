using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTroop : MonoBehaviour
{
    Vector3 spawnLocation;
    public Transform location;
    public int spawnLimit;
    public GameObject troop;

    Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = location.transform.position;
        spawnLimit = 10;
        rotation = location.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnLimit > 0)
        {
            Instantiate(troop, spawnLocation, rotation);
            spawnLimit--;
        }
        else
        Destroy(gameObject);
    }
}
