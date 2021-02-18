using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    bool areWeBeingAttacked;

    Renderer m_renderer;

    public Texture m_MainTexture;
    public Texture attackTexture;

    // Start is called before the first frame update
    void Start()
    {
        areWeBeingAttacked = gameObject.GetComponentInParent<enemyWander>().beingAttacked;

        m_renderer = GetComponent<Renderer>();

        m_renderer.material.SetTexture("Bunbee Attacking", m_MainTexture);
    }

    // Update is called once per frame
    void Update()
    {
        areWeBeingAttacked = gameObject.GetComponentInParent<enemyWander>().beingAttacked;
        if (areWeBeingAttacked == true)
        {

        }
    }
}
