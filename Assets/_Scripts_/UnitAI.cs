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


    private void Start()
    {
        InvokeRepeating("Check", 0.0f, checkRate);
    }
    private void Awake()
    {
        playerAI = GetComponent<PlayerAI>();

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
                    
                    break;
                }
            case AIUnitState.Attack:
                {
                    
                    break;
                }


        }
    }
    void Check()
    {
        // check if we have nearby enemies - if so, attack them
        if (unitState != AIUnitState.Attack && unitState != AIUnitState.MoveToEnemy)
        {
            Unit potentialEnemy = CheckForNearbyEnemies();
            AttackUnit(potentialEnemy);
        }
        if (unitState == AIUnitState.Idle)
            PursueEnemy();

    }

    Unit CheckForNearbyEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, nearbyEnemyAttackRange, Vector3.up, unitLayerMask);
        GameObject closest = null;

        float closestDist = 0.0f;
        for (int x = 0; x < hits.Length; x++)
        {
            // skip if this is us
            if (hits[x].collider.gameObject == gameObject)
                continue;
            // is this a teammate?
            //if (unit.player.IsMyUnit(hits[x].collider.GetComponent<Unit>()))
            //    continue;

            if (!closest || Vector3.Distance(transform.position, hits[x].transform.position) < closestDist)
            {
            closest = hits[x].collider.gameObject;
            closestDist = Vector3.Distance(transform.position, hits[x].transform.position);
            }
        }

        if (closest != null)
            return closest.GetComponent<Unit>();
        else
            return null;
    }
    void PursueEnemy()
    {
        Player enemyPlayer = Player.me;
        if (enemyPlayer.units.Count > 0)
        {
            AttackUnit(enemyPlayer.units[Random.Range(0, enemyPlayer.units.Count)]);
        }

    }

    public void AttackUnit(Unit target)
    {
        curEnemyTarget = target;
        SetState(AIUnitState.MoveToEnemy);
    }

    void Die()
    {

        playerAI.units.Remove(this);
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
