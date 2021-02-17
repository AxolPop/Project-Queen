using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class troopSize : MonoBehaviour
{
    public Text troopTwotal;
    // Start is called before the first frame update
    void Start()
    {
        troopTwotal.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        troopTwotal.text = troop.troopTotal.ToString() + "/" + troop.troopMaxTotal.ToString();
        troopTwotal.SetAllDirty();
    }
}
