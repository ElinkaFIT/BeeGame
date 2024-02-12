using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIUnitState
{
    Idle,
    MoveToEnemy,
    Attack
}

public class UnitAI : MonoBehaviour
{
    public float checkRate;
    public float nearbyEnemyAttackRange;
    public LayerMask unitLayerMask;

    private PlayerAI playerAI;
    private Unit curEnemyTarget;

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


    private void Start()
    {
        SetState(AIUnitState.Idle);
        InvokeRepeating(nameof(Check), 0.0f, checkRate);
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


        }
    }
    void Check()
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

    void MoveToEnemyUpdate()
    {
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
