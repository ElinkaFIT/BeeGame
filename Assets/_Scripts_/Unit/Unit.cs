using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum UnitState
{
    Idle,
    Move,
    MoveToResource,
    Gather,
    MoveToEnemy,
    Attack,
    MoveToBuild,
    Build,
    MoveToWork,
    Work
}

public class Unit : MonoBehaviour
{
    [Header("Components")]
    public GameObject selectionVisual;
    public UnitHealth healthBar;
    NavMeshAgent agent;

    public Player player;

    [Header("Stats")]
    public UnitState state;

    public int curHp;
    public int maxHp;

    public int minAttackDamage;
    public int maxAttackDamage;

    public float attackRate;
    private float lastAttackTime;

    public float pathUpdateRate = 1.0f;
    private float lastPathUpdateTime;

    public int gatherAmount;
    public float gatherRate;
    private float lastGatherTime;

    public int buildAmount;
    public float buildRate;
    private float lastBuildTime;

    private UnitAI curEnemyTarget;
    private ResourceSource curResourceSource;
    private Room curBuildRoom;

    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<UnitState> { }
    public StateChangeEvent onStateChange;

    private void Start()
    {
        SetState(UnitState.Idle);
    }
    private void Awake()
    {
        // potrebne nastaveni jednotky kvuli NavMeshPlus
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        ToggleSelectionVisual(false);
    }

    void Update()
    {
        switch (state)
        {
            case UnitState.Move:
            {
                MoveUpdate();
                break;
            }
            case UnitState.MoveToResource:
            {
                MoveToResourceUpdate();
                break;
            }
            case UnitState.Gather:
            {
                GatherUpdate();
                break;
            }
            case UnitState.MoveToEnemy:
            {
                MoveToEnemyUpdate();
                break;
            }
            case UnitState.Attack:
            {
                AttackUpdate();
                break;
            }
            case UnitState.MoveToBuild:
            {
                MoveToBuildUpdate();
                break;
            }
            case UnitState.Build:
            {
                BuildUpdate();
                break;
            }
            case UnitState.MoveToWork:
            {
                MoveToWorkUpdate();
                break;
            }
            case UnitState.Work:
            {
                WorkUpdate();
                break;
            }

        }
    }

    void MoveUpdate()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);

        if (Vector2.Distance(pos, des) == 0.0f)
            SetState(UnitState.Idle);
    }

    void MoveToResourceUpdate()
    {
        if (curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);

        if (Vector2.Distance(pos, des) == 0.0f)
            SetState(UnitState.Gather);
    }

    void GatherUpdate()
    {
        if (curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Time.time - lastGatherTime > gatherRate)
        {
            lastGatherTime = Time.time;
            curResourceSource.GatherResource(gatherAmount);
        }
    }

    void MoveToEnemyUpdate()
    {
        if (curEnemyTarget == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Time.time - lastPathUpdateTime > pathUpdateRate)
        {
            lastPathUpdateTime = Time.time;
            agent.isStopped = false;
            agent.SetDestination(curEnemyTarget.transform.position);
        }

        if (Vector3.Distance(transform.position, curEnemyTarget.transform.position) <= 1.5)
            SetState(UnitState.Attack);
    }

    void AttackUpdate()
    {
        if (curEnemyTarget == null)
        {
            SetState(UnitState.Idle);
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
            SetState(UnitState.MoveToEnemy);
    }

    void MoveToBuildUpdate()
    {
        if (curBuildRoom == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);


        if (Vector2.Distance(pos, des) < 0.01f)
        {
            SetState(UnitState.Build);
        }
    }

    void BuildUpdate()
    {
        if (curBuildRoom == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Time.time - lastBuildTime > buildRate)
        {
            lastBuildTime = Time.time;
            curBuildRoom.BuildRoom(buildAmount);
        }

        // zacne se stavet
    }

    void MoveToWorkUpdate()
    {

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);


        if (Vector2.Distance(pos, des) < 0.01f)
        {
            SetState(UnitState.Work);
        }
    }

    void WorkUpdate()
    {
        curBuildRoom.WorkInRoom(gameObject.GetComponent<Unit>());
    }


    // zobrazit vyber jednotky
    public void ToggleSelectionVisual(bool selected)
    {
        if (selectionVisual != null)
            selectionVisual.SetActive(selected);

    }

    // pohyb jednotky
    public void MoveToPosition(Vector2 target)
    {
        SetState(UnitState.Move);
        agent.SetDestination(new Vector3(target.x, target.y, 0));
    }

    public void GatherResource(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;
        SetState(UnitState.MoveToResource);
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    public void BuildRoom(Room room, Vector3 pos)
    {
        curBuildRoom = room;
        SetState(UnitState.MoveToBuild);
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    public void WorkInRoom(Room room, Vector3 pos)
    {
        curBuildRoom = room;
        SetState(UnitState.MoveToWork);
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    void SetState(UnitState toState)
    {
        // pokud pracovala, prestane odchodem pracovat
        if (state == UnitState.Work)
        {
            curBuildRoom.StopWorkInRoom(gameObject.GetComponent<Unit>());
        }

        state = toState;
        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
        if (toState == UnitState.Idle)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    public void AttackUnit(UnitAI target)
    {
        curEnemyTarget = target;
        SetState(UnitState.MoveToEnemy);
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
        // me
        if (player != null)
        {
            player.units.Remove(this);
            Destroy(gameObject);
        }
    }

}
