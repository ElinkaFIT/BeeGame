using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

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
    Work,
    MoveToFoodRoom,
    Eat,
    MoveToRestRoom,
    Sleep,
    SearchMove,
    Searching
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

    public int searchAmount;
    public float searchRate;
    private float lastSearchTime;

    public int curFeed;
    public int maxFeed;

    public int curEnergy;
    public int maxEnergy;

    private UnitAI curEnemyTarget;
    private ResourceSource curResourceSource;
    private Room curBuildRoom;

    private ResourceTile curResourceTile;

    public float checkRate;

    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<UnitState> { }
    public StateChangeEvent onStateChange;

    private void Start()
    {
        SetState(UnitState.Idle);
        InvokeRepeating(nameof(RecalculateHunger), 0.0f, checkRate);
        InvokeRepeating(nameof(RecalculateEnergy), 0.0f, checkRate);
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
            case UnitState.MoveToFoodRoom:
                {
                    break;
                }
            case UnitState.Eat:
                {
                   
                    break;
                }
            case UnitState.MoveToRestRoom:
                {
                    MoveToRestRoomUpdate();
                    break;
                }
            case UnitState.SearchMove:
                {
                    SearchMoveUpdate();
                    break;
                }
            case UnitState.Searching:
                {
                    SearchingUpdate();
                    break;
                }
            case UnitState.Sleep:
                {
                    WorkUpdate();
                    break;
                }

        }
    }

    private void SearchingUpdate()
    {
        if (curResourceTile == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Time.time - lastSearchTime > searchRate)
        {
            lastSearchTime = Time.time;
            curResourceTile.SearchArea(searchAmount);
        }

    }

    private void SearchMoveUpdate()
    {
        if (curResourceTile == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);


        if (Vector2.Distance(pos, des) < 0.01f)
        {
            SetState(UnitState.Searching);
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

    void MoveToRestRoomUpdate()
    {

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2();

        // TODO najdi nejblizsi volny rest room
        GameObject[] restRooms = GameObject.FindGameObjectsWithTag("RestRoom");

        if (restRooms.Length > 0)
        {
            des = new Vector2(restRooms[0].transform.position.x, restRooms[0].transform.position.y);
            agent.SetDestination(new Vector3(des.x, des.y, 0));
        }

        if (Vector2.Distance(pos, des) < 0.01f)
        {
            SetState(UnitState.Sleep);
        }
    }

    // repeating funkce
    void RecalculateHunger()
    {
        curFeed--;
        if (curFeed <= 0)
        {
            Log.instance.AddNewLogText(Time.time, "Bee died of hunger", Color.red);
            Die();
        }
        else if (curFeed < 15)
        {
            // prestane pracovat a jde se najist

        }

        if (curFeed < 5)
        {
            // Log.instance.AddNewLogText(Time.time, "Bee is starving", Color.red);
        }
    }

    void RecalculateEnergy()
    {
        // tady odebere energii podle toho v jakem je unitstate
        switch (state)
        {
            // pokud se nekam pohybuje
            case (UnitState)1 or (UnitState)2 or (UnitState)4 or (UnitState)6 or (UnitState)8 or (UnitState)10 or (UnitState)12:
                {
                    curEnergy -= 1;
                    break;
                }
            // pokud pracuje
            case (UnitState)3 or (UnitState)5 or (UnitState)7 or (UnitState)9:
                {
                    curEnergy -= 3;
                    break;
                }
            // pokud je v IDLE nebo jine nenarocne cinnosti
            default: { break; }
        }

        // pokud dojde energie prestane pracovat a jde spat
        if(curEnergy <= 0)
        {
            SetState(UnitState.MoveToRestRoom);
        }
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

    public void Searching(ResourceTile tile, Vector3 pos)
    {
        curResourceTile = tile;
        SetState(UnitState.SearchMove);
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
        if (state == UnitState.Work || state == UnitState.Sleep)
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
