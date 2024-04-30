using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public enum AIUnitState
{
    Idle,
    MoveToEnemy,
    Attack,
    MoveToHive,
    DestroyRoom
}

public enum AIUnitType
{
    BeeAttacker,
    HiveDestroyer

}

public class UnitAI : MonoBehaviour
{
    public AIUnitType unitType;
    public float checkRate;
    public float nearbyEnemyAttackRange;
    public LayerMask unitLayerMask;

    private PlayerAI playerAI;
    private Unit curEnemyTarget;
    private Room curHiveTarget;

    public AIUnitState unitState;
    NavMeshAgent agent;
    public UnitHealth healthBar;

    public int curHp;
    public int maxHp;

    public int minAttackDamage;
    public int maxAttackDamage;

    public float pathUpdateRate;
    private float lastPathUpdateTime;

    public float attackRate;
    private float lastAttackTime;

    public Animator animator;
    public GameObject graphic;

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
    private void Awake()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }
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

    void CheckEnemy()
    {
        // check if we have nearby enemies - if so, attack them
        if (unitState == AIUnitState.Idle || unitState == AIUnitState.MoveToEnemy)
        {
            Unit potentialEnemy = CheckForNearbyEnemies();
            if (potentialEnemy != null)
            {
                AttackUnit(potentialEnemy);
            }
            
        }

    }

    void CheckHive()
    {
        // check if we have built hive rooms nearby - attack them
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


    public void AttackUnit(Unit target)
    {
        curEnemyTarget = target;
        SetState(AIUnitState.MoveToEnemy);
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        if (curHp <= 0)
            Die();

        healthBar.UpdateHealthBar(curHp, maxHp);
    }

    void Die()
    {

        PlayerAI.enemy.units.Remove(this);
        Destroy(gameObject);
        
    }

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
