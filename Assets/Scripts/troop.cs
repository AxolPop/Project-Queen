using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
public class troop : MonoBehaviour
{
    //Wander Variables
    public float wanderRadius;
    public float wanderTimer;
    public GameObject player;

    Vector3 moveVector;

    //Coroutines

    Coroutine startChargingDude = null;
    Coroutine waitABit = null;
    Coroutine broStartAttacking = null;

    bool allowAttack = true;

    float closestDistance;

    private NavMeshAgent ai;
    private float timer;

    enemyWander getEnemyScript;

    public static float troopMaxTotal;

    public Vector3 hitPoint;
    public float distance = 10;
    public float distanceFromSlot = 10;
    public bool canCharge = true;

    public GameObject thisTroop;
    private float distanceToEnemy;
    private float minimumDistance = 2f;

    Camera cam;

    public static float troopTotal = 0;
    public float twoopTwotalUwU;
    public float troopID;

    GameObject troopNumber;

    float moveSpeed = 25;
    CharacterController cc;

    public bool canGoToKing = true;

    public enum State
    {
        wandering,
        atKing,
        attacking,
        charging
    }

    bool charging = false;

    public GameObject nearestEnemy;

    State state;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "cursor" && state == State.wandering && Input.GetMouseButtonDown(1))
        {
            setNumber();
            state = State.atKing;
        }

        if (other.tag == "attackarea" && state == State.atKing && charging)
        {
            state = State.attacking;
        }

        if (other.tag == "cursor" && state == State.attacking && Input.GetMouseButtonDown(1))
        {
            StopCoroutine(broStartAttacking);
            StartCoroutine(returnToPlayer());
        }
    }

    void setNumber()
    {
        troopTotal++;
        troopID = troopTotal;
    }

    // Start is called before the first frame update
    void Start()
    {
        troopMaxTotal = 12;
        distance = 10;
        cc = GetComponent<CharacterController>();
        cam = Camera.main;
        state = State.wandering;
        ai = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        allowAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        twoopTwotalUwU = troopTotal;
        switch (state)
        {
            case State.wandering:
                ai.speed = 3.8f;
                ai.acceleration = 8;
                timer += Time.deltaTime;

                if (timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    ai.SetDestination(newPos);
                    timer = 0;
                }
                break;

            case State.atKing:
                ai.speed = 25;
                ai.acceleration = 9999;
                troopNumber = GameObject.Find(troopID.ToString());
                if (troopNumber == null) { state = State.wandering; }

                if (canGoToKing)
                {
                    ai.SetDestination(new Vector3(troopNumber.transform.position.x, troopNumber.transform.position.y, troopNumber.transform.position.z));
                }

                if (Input.GetMouseButtonDown(0) && canGoToKing && canCharge == true)
                {
                    if (troopID == 1)
                    {
                        canGoToKing = false;
                        troopTotal--;

                        charging = true;

                        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                        LayerMask mask = 1 << 8;

                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, 999, mask))
                        {

                            hitPoint = hit.point;
                        }

                        disableAgent();
                        canCharge = false;
                        startChargingDude = StartCoroutine(startCharging());

                    }
                    else { troopID--; }
                }
                break;

            case (State.attacking):
                if (allowAttack)
                    broStartAttacking = StartCoroutine(attackingEnemy());
                break;
        }
    }

    IEnumerator returnToPlayer()
    {
        ai.radius = 0.1f;
        enableAgent();
        canCharge = false;
        state = State.atKing;
        charging = false;
        distance = 10;
        distanceFromSlot = 10;
        while (!canCharge && distanceFromSlot > 1)
        {
            ai.SetDestination(player.transform.position);
            distanceFromSlot = Vector3.Distance(player.transform.position, transform.position);
            yield return null;
        }
        allowAttack = true;
        canCharge = true;
        canGoToKing = true;
        troopTotal++;
        troopID = troopTotal;
    }

    IEnumerator attackingEnemy()
    {
        allowAttack = false;
        findClosestEnemy();

        enableAgent();

        StopCoroutine(waitABit);
        StopCoroutine(startChargingDude);

        getEnemyScript = nearestEnemy.GetComponent<enemyWander>();

        while (getEnemyScript.health > 0 && nearestEnemy != null)
        {
            ai.radius = 0.3f;
            ai.SetDestination(nearestEnemy.transform.position);
            yield return new WaitForSeconds(1.5f);
            DOVirtual.Float(getEnemyScript.health, getEnemyScript.health - 10, 0.15f, Bro).SetEase(Ease.OutSine);

        }

        StartCoroutine(returnToPlayer());
    }

    IEnumerator startCharging()
    {
        canCharge = false;
        waitABit = StartCoroutine(ifWaitingTooLong());
        while (distance > 1.2)
        {
            var offset = hitPoint - transform.position;
            distance = Vector3.Distance(hitPoint, transform.position);
            //Get the difference.
            if (offset.magnitude > .1f)
            {
                //If we're further away than .1 unit, move towards the target.
                //The minimum allowable tolerance varies with the speed of the object and the framerate. 
                // 2 * tolerance must be >= moveSpeed / framerate or the object will jump right over the stop.
                offset = offset.normalized * moveSpeed;
                //normalize it and account for movement speed.
                cc.Move(offset * Time.deltaTime);
                useGravity();
                //actually move the character.
            }
            yield return null;
        }

        StopCoroutine(waitABit);

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(returnToPlayer());
    }

    IEnumerator ifWaitingTooLong()
    {
        yield return new WaitForSeconds(1);
        StopCoroutine(startChargingDude);
        yield return new WaitForSeconds(1.5f);

        StartCoroutine(returnToPlayer());
    }

    void Bro(float x)
    {
        getEnemyScript.health = x;
    }

    private GameObject findClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("enemy");
        GameObject closestEnemy = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closestEnemy = go;
                distance = curDistance;
            }
        }
        nearestEnemy = closestEnemy;
        Debug.Log(closestEnemy);
        return closestEnemy;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;

    }

    void disableAgent()
    {
        ai.enabled = false;
        ai.updatePosition = false;
        ai.updateRotation = false;
    }

    void enableAgent()
    {
        ai.enabled = true;
        ai.updatePosition = true;
        ai.updateRotation = true;
    }

    void useGravity()
    {
        //REeset the MoveVector
        moveVector = Vector3.zero;

        //Check if cjharacter is grounded
        if (cc.isGrounded == false)
        {
            //Add our gravity Vecotr
            moveVector += Physics.gravity;
        }

        //Apply our move Vector , remeber to multiply by Time.delta
        cc.Move(moveVector * Time.deltaTime);

    }
}
