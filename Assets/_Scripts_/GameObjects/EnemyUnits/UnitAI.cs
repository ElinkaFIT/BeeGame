//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// States of enemy unit
/// </summary>
public enum AIUnitState
{
    Idle,
    MoveToEnemy,
    Attack,
    MoveToHive,
    DestroyRoom
}

/// <summary>
/// Types of enemy unit
/// </summary>
public enum AIUnitType
{
    BeeAttacker,
    HiveDestroyer

}

/// <summary>
/// Class object pro konkretni objekt enemy
/// </summary>
public class UnitAI : MonoBehaviour
{
    public AIUnitType unitType;             // type of enemy unit
    public AIUnitState unitState;           // current enemy state
    NavMeshAgent agent;                     // navigation agent of enemy
    private PlayerAI playerAI;              // player object of enemy

    // Health
    public UnitHealth healthBar;            // enemy health bar object
    public int curHp;                       // current enemy health
    public int maxHp;                       // maximum enemy health

    // Attacking
    private Unit curEnemyTarget;            // current bee unit target
    private Room curHiveTarget;             // current room in hive target

    public float checkRate;                 // 
    public float nearbyEnemyAttackRange;    // 
    public LayerMask unitLayerMask;         // 

    public int minAttackDamage;             // minimal damage of enemy
    public int maxAttackDamage;             // maximum demage of enemy

    public float attackRate;                // attack rate in time
    private float lastAttackTime;           // last time unit attacked

    // Moving
    public float pathUpdateRate;            // 
    private float lastPathUpdateTime;       // 

    // Graphic
    public Animator animator;               // animation component 
    public GameObject graphic;              // graphic of enemy unit

    /// <summary>
    /// Podle typu enemy jednotky v intervalech hleda cil utoku
    /// </summary>
    private void Start()
    {
        if(unitType == AIUnitType.BeeAttacker)
        {
            InvokeRepeating(nameof(CheckEnemy), 0.0f, checkRate);
        }
        else if(unitType == AIUnitType.HiveDestroyer)
        {
            InvokeRepeating(nameof(CheckHive), 0.0f, checkRate);
        }
    }

    /// <summary>
    /// inicializace of navigation agent
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    /// <summary>
    /// Provede odlidne update funkce dle stavu enemy jednotky
    /// </summary>
    void Update()
    {
        switch (unitState)
        {
            case AIUnitState.Idle:
                {
                    break;
                }
            case AIUnitState.MoveToEnemy:
                {
                    MoveToEnemyUpdate();
                    break;
                }
            case AIUnitState.Attack:
                {
                    AttackUpdate();
                    break;
                }
            case AIUnitState.MoveToHive:
                {
                    MoveToHiveUpdate();
                    break;
                }
            case AIUnitState.DestroyRoom:
                {
                    DestroyRoomUpdate();
                    break;
                }


        }
    }

    /// <summary>
    /// Move enemy unit to current target room in hive
    /// </summary>
    private void MoveToHiveUpdate()
    {
        SetAnimation(true);
        if (curHiveTarget == null)
        {
            SetState(AIUnitState.Idle);
            return;
        }

        if (Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            lastPathUpdateTime = Time.time;
            agent.isStopped = false;
            agent.SetDestination(curHiveTarget.transform.position);
            SetEnemyDirection(agent.transform.position.x, curHiveTarget.transform.position.x);
            
        }

        if (Vector3.Distance(transform.position, curHiveTarget.transform.position) <= 1.5)
            SetState(AIUnitState.DestroyRoom);
    }

    /// <summary>
    /// Unit make random demage to current attacking room in the hive
    /// </summary>
    private void DestroyRoomUpdate()
    {
        SetAnimation(false);

        if (curHiveTarget == null)
        {
            SetState(AIUnitState.Idle);
            return;
        }

        if (!agent.isStopped)
            agent.isStopped = true;

        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            curHiveTarget.TakeRoomDmg(Random.Range(minAttackDamage, maxAttackDamage + 1));
            if(curHiveTarget.curRoomHealth < 0)
            {
                SetState(AIUnitState.Idle);
            }
        }
    }

    /// <summary>
    /// Check for nearby bee unit and attack them
    /// </summary>
    void CheckEnemy()
    {
        if (unitState == AIUnitState.Idle || unitState == AIUnitState.MoveToEnemy)
        {
            Unit potentialEnemy = CheckForNearbyEnemies();
            if (potentialEnemy != null)
            {
                AttackUnit(potentialEnemy);
            }
            
        }

    }

    /// <summary>
    /// Check for nearby built room in hive and make damage
    /// </summary>
    void CheckHive()
    {
        if (unitState == AIUnitState.Idle)
        {
            Room potentialRoom = CheckForNearbyRooms();
            if (potentialRoom != null)
            {
                curHiveTarget = potentialRoom;
                SetState(AIUnitState.MoveToHive);
            }
        }
    }

    /// <summary>
    /// Move enemy unit to bee unit and if nearby start attack
    /// </summary>
    void MoveToEnemyUpdate()
    {
        SetAnimation(true);
        if (curEnemyTarget == null)
        {
            SetState(AIUnitState.Idle);
            return;
        }

        if (Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            lastPathUpdateTime = Time.time;
            agent.isStopped = false;
            agent.SetDestination(curEnemyTarget.transform.position);
            SetEnemyDirection(agent.transform.position.x, curEnemyTarget.transform.position.x);

        }

        if (Vector3.Distance(transform.position, curEnemyTarget.transform.position) <= 1.5)
            SetState(AIUnitState.Attack);
    }

    /// <summary>
    /// Attack bee unit
    /// </summary>
    void AttackUpdate()
    {
        if (curEnemyTarget == null)
        {
            SetState(AIUnitState.Idle);
            return;
        }

        if (!agent.isStopped)
            agent.isStopped = true;

        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            curEnemyTarget.TakeDamage(Random.Range(minAttackDamage, maxAttackDamage + 1));
        }

        // pokud je daleko pohnu se za nim
        if (Vector3.Distance(transform.position, curEnemyTarget.transform.position) > 1.5)
            SetState(AIUnitState.MoveToEnemy);
    }

    /// <summary>
    /// Find closest bee enemy and return it
    /// </summary>
    /// <returns></returns>
    Unit CheckForNearbyEnemies()
    {
        List<Unit> targets = Player.me.units;
        GameObject closest = null;

        float closestDist = 0.0f;
        foreach (Unit target in targets)
        {

            if (!closest || Vector3.Distance(transform.position, target.transform.position) < closestDist)
            {
                closest = target.gameObject;
                closestDist = Vector3.Distance(transform.position, target.transform.position);
            }
        }

        if (closest != null)
            return closest.GetComponent<Unit>();
        else
            return null;
    }

    /// <summary>
    /// Find closest built room in hive and return it
    /// </summary>
    /// <returns></returns>
    Room CheckForNearbyRooms()
    {
        List<Room> targets = Hive.instance.rooms;
        GameObject closest = null;

        float closestDist = 0.0f;
        foreach (Room target in targets)
        {

            if (!closest || Vector3.Distance(transform.position, target.transform.position) < closestDist)
            {
                closest = target.gameObject;
                closestDist = Vector3.Distance(transform.position, target.transform.position);
            }
        }

        if (closest != null)
            return closest.GetComponent<Room>();
        else
            return null;
    }

    /// <summary>
    /// Start attack unit
    /// </summary>
    /// <param name="target"></param>
    public void AttackUnit(Unit target)
    {
        curEnemyTarget = target;
        SetState(AIUnitState.MoveToEnemy);
    }

    /// <summary>
    /// Enemy unit take demage
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        curHp -= damage;
        if (curHp <= 0)
            Die();

        healthBar.UpdateHealthBar(curHp, maxHp);
    }

    /// <summary>
    /// Remove enemy unit from game
    /// </summary>
    void Die()
    {

        PlayerAI.enemy.units.Remove(this);
        Destroy(gameObject);
        
    }

    /// <summary>
    /// Set enemy anmimation active
    /// </summary>
    /// <param name="setFlyingOn"></param>
    public void SetAnimation(bool setFlyingOn)
    {
        if (setFlyingOn)
        {
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    /// <summary>
    /// Set enemy graphic direction 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dest"></param>
    public void SetEnemyDirection(float pos, float dest)
    {
        if (dest < pos)
        {
            // otoc smerem doleva
            graphic.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (dest > pos)
        {
            // otoc doprava
            graphic.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    /// <summary>
    /// Change current enemy state
    /// </summary>
    /// <param name="toState"></param>
    void SetState(AIUnitState toState)
    {

        unitState = toState;

        if (toState == AIUnitState.Idle)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

}
