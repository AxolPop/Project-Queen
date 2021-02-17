using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyWander : MonoBehaviour
{
    NavMeshAgent ai;

    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    public float health = 100;
    float maxHealth;
    Image healthValue;
    // Start is called before the first frame update
    void Start()
    {
        ai = GetComponent<NavMeshAgent>();

        healthValue = gameObject.transform.Find("Health/Health Value").GetComponent<Image>();

        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        ai.speed = 3.8f;
        ai.acceleration = 8;
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            ai.SetDestination(newPos);
            timer = 0;
        }
        healthValue.fillAmount = health / maxHealth;

        if (health < maxHealth)
        {
            ai.speed = 0;
        }

        if (health < 1)
        {
            Destroy(gameObject, 0);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;

    }
}
