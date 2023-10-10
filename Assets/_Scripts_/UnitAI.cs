using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public float checkRate = 1.0f;
    public float nearbyEnemyAttackRange;
    public LayerMask unitLayerMask;
    public PlayerAI playerAI;
    public Unit unit;


    void Start()
    {
        InvokeRepeating("Check", 0.0f, checkRate);
    }
    void Check()
    {
        // check if we have nearby enemies - if so, attack them
        if (unit.state != UnitState.Attack && unit.state != UnitState.MoveToEnemy)
        {
            Unit potentialEnemy = CheckForNearbyEnemies();
            if (potentialEnemy != null)
                unit.AttackUnit(potentialEnemy);
        }
        if (unit.state == UnitState.Idle)
            PursueEnemy();

    }

    public void InitializeAI(PlayerAI playerAI, Unit unit)
    {
        this.playerAI = playerAI;
        this.unit = unit;
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
            if (unit.player.IsMyUnit(hits[x].collider.GetComponent<Unit>()))
                continue;

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
        Player enemyPlayer = GameManager.instance.GetRandomEnemyPlayer(unit.player);
        if (enemyPlayer.units.Count > 0)
            unit.AttackUnit(enemyPlayer.units[Random.Range(0, enemyPlayer.units.Count)]);

    }

}
