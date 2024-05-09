//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enum defining possible states of an enemy unit.
/// </summary>
public enum AIUnitState
{
    Idle,           // Unit is idle
    MoveToEnemy,    // Unit is moving towards an enemy
    Attack,         // Unit is attacking
    MoveToHive,     // Unit is moving towards the hive
    DestroyRoom     // Unit is destroying a room
}

/// <summary>
/// Enum defining types of enemy units.
/// </summary>
public enum AIUnitType
{
    BeeAttacker,    // Unit attacks bees
    HiveDestroyer   // Unit attacks the hive
}

/// <summary>
/// Represents a specific enemy unit and controls its behavior.
/// </summary>
public class UnitAI : MonoBehaviour
{
    public AIUnitType unitType;             // Type of enemy unit
    public AIUnitState unitState;           // Current state of the enemy
    NavMeshAgent agent;                     // Navigation agent used for movement
    private PlayerAI playerAI;              // Reference to the enemy's player AI

    // Health
    public UnitHealth healthBar;            // Health bar UI for the enemy
    public int curHp;                       // Current health of the enemy
    public int maxHp;                       // Maximum health of the enemy

    // Attacking
    private Unit curEnemyTarget;            // Current target enemy unit
    private Room curHiveTarget;             // Current target room in the hive

    public float checkRate;                 // Rate to check for nearby enemies
    public float nearbyEnemyAttackRange;    // Attack range for nearby enemies
    public LayerMask unitLayerMask;         // Layer mask to filter units during checks

    public int minAttackDamage;             // Minimum damage the enemy can deal
    public int maxAttackDamage;             // Maximum damage the enemy can deal

    public float attackRate;                // Rate at which the enemy attacks
    private float lastAttackTime;           // Time since the enemy last attacked

    // Movement
    public float pathUpdateRate;            // Rate at which the enemy updates its path
    private float lastPathUpdateTime;       // Time since the enemy's path was last updated

    // Graphics
    public Animator animator;               // Animation component 
    public GameObject graphic;              // Graphic representation of the enemy

    /// <summary>
    /// Initializes the enemy unit based on its type.
    /// </summary>
    private void Start()
    {
        if (unitType == AIUnitType.BeeAttacker)
        {
            InvokeRepeating(nameof(CheckEnemy), 0.0f, checkRate);
        }
        else if (unitType == AIUnitType.HiveDestroyer)
        {
            InvokeRepeating(nameof(CheckHive), 0.0f, checkRate);
        }
    }

    /// <summary>
    /// Initializes the navigation agent.
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    /// <summary>
    /// Updates the unit based on its current state.
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
    /// Handles movement towards the current hive target.
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
    /// Performs actions to damage a room.
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
            if (curHiveTarget.curRoomHealth < 0)
            {
                SetState(AIUnitState.Idle);
            }
        }
    }

    /// <summary>
    /// Checks for nearby enemiy bees to attack.
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
    /// Checks for nearby hive rooms to attack.
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
    /// Handles movement towards the current enemy target.
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
    /// Handles attacking the current target.
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

        if (Vector3.Distance(transform.position, curEnemyTarget.transform.position) > 1.5)
            SetState(AIUnitState.MoveToEnemy);
    }

    /// <summary>
    /// Finds the closest enemy bee unit.
    /// </summary>
    /// <returns>Closest enemy unit.</returns>
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
        return null;
    }

    /// <summary>
    /// Finds the closest room in the hive.
    /// </summary>
    /// <returns>Closest hive room.</returns>
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
        return null;
    }

    /// <summary>
    /// Commands the unit to start attacking a target unit.
    /// </summary>
    /// <param name="target">The unit to attack.</param>
    public void AttackUnit(Unit target)
    {
        curEnemyTarget = target;
        SetState(AIUnitState.MoveToEnemy);
    }

    /// <summary>
    /// Handles the enemy taking damage.
    /// </summary>
    /// <param name="damage">The amount of damage taken.</param>
    public void TakeDamage(int damage)
    {
        curHp -= damage;
        if (curHp <= 0)
            Die();
        healthBar.UpdateHealthBar(curHp, maxHp);
    }

    /// <summary>
    /// Removes the enemy unit from the game.
    /// </summary>
    void Die()
    {
        PlayerAI.enemy.units.Remove(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Sets the enemy unit's animation state.
    /// </summary>
    /// <param name="setFlyingOn">Whether to enable flying animation.</param>
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
    /// Sets the direction the enemy unit is facing based on movement.
    /// </summary>
    /// <param name="pos">Current position.</param>
    /// <param name="dest">Destination position.</param>
    public void SetEnemyDirection(float pos, float dest)
    {
        if (dest < pos)
        {
            // turn left
            graphic.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (dest > pos)
        {
            // turn right
            graphic.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    /// <summary>
    /// Changes the current state of the unit.
    /// </summary>
    /// <param name="toState">The new state to transition to.</param>
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
