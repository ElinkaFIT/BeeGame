using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

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

    public Animator animator;
    public Animator borderAnimator;

    public GameObject graphic;
    public GameObject selectGraphic;

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

        InvokeRepeating(nameof(CheckForEnemies), 0.0f, checkRate);
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
        else if (curFeed < 20)
        {

            // jde se najisrt
            SetState(UnitState.MoveToFoodRoom);
           
        }

        if (curFeed < 5)
        {
            Log.instance.AddNewLogText(Time.time, "Bee is starving", Color.red);
        }
    }

    void RecalculateEnergy()
    {
        // tady odebere energii podle toho v jakem je unitstate
        switch (state)
        {
            // pokud se nekam pohybuje
            case (UnitState)1 or (UnitState)2 or (UnitState)4 or (UnitState)6 or (UnitState)8 or (UnitState)10 or (UnitState)12 or (UnitState)14:
                {
                    curEnergy -= 1;
                    break;
                }
            // pokud pracuje
            case (UnitState)3 or (UnitState)5 or (UnitState)7 or (UnitState)9 or (UnitState)15:
                {
                    curEnergy -= 3;
                    break;
                }
            // pokud je v IDLE nebo jine nenarocne cinnosti
            default: { break; }
        }

        // pokud dojde energie prestane pracovat a jde spat
        if (curEnergy <= 20)
        {
            SetState(UnitState.MoveToRestRoom);
        }
    }

    private void CheckForEnemies()
    {

        // pokud nepracuje zautoci
        if (state != (UnitState)3 || state != (UnitState)5 || state != (UnitState)7 || state != (UnitState)9)
        {
            List<UnitAI> targets = PlayerAI.enemy.units;
            GameObject newTarget = null;
            float closestDist = 0.0f;

            foreach (UnitAI target in targets)
            {
                if (!newTarget || Vector3.Distance(transform.position, target.transform.position) < closestDist)
                {
                    newTarget = target.gameObject;
                    closestDist = Vector3.Distance(transform.position, target.transform.position);
                }
            }

            // pokud je enemy jednotka dostatecne blizko
            if (newTarget != null && closestDist < 10)
            {
                AttackUnit(newTarget.GetComponent<UnitAI>());
            }

        }


        
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
                    MoveToFoodRoomUpdate();
                    break;
                }
            case UnitState.Eat:
                {
                    WorkUpdate();
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
            case UnitState.Idle:
                {
                    SetAnimation(false);
                    break;
                }

        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void SearchingUpdate()
    {
        SetAnimation(false);

        if (curResourceTile == null || curResourceTile.state == ResourceTileState.Exposed)
        {
            SetState(UnitState.Idle);
            return;
        }

        if (Time.time - lastSearchTime > searchRate)
        {
            lastSearchTime = Time.time;
            curResourceTile.SearchArea(searchAmount);

            // procentualni moznost ze jednotka neprezije
            if (Random.Range(0, 100) < curResourceTile.dangerValue / 20)
            { 
                Log.instance.AddNewLogText(Time.time, "Bee died while exploring the area.", Color.black);
                Die();
            }

        }
        
    }

    private void SearchMoveUpdate()
    {
        SetAnimation(true);

        if (curResourceTile == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);

        SetBeeDirection(pos.x, des.x);


        if (Vector2.Distance(pos, des) < 0.01f)
        {
            SetState(UnitState.Searching);
        }
    }

    void MoveUpdate()
    {
        SetAnimation(true);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);

        SetBeeDirection(pos.x, des.x);

        if (Vector2.Distance(pos, des) <= 0.01f)
            SetState(UnitState.Idle);
    }

    void MoveToResourceUpdate()
    {
        SetAnimation(true);

        if (curResourceSource == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);

        SetBeeDirection(pos.x, des.x);

        if (Vector2.Distance(pos, des) <= 0.01f)
            SetState(UnitState.Gather);
    }

    void GatherUpdate()
    {
        SetAnimation(false);

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
        SetAnimation(true);

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
            SetBeeDirection(transform.position.x, curEnemyTarget.transform.position.x);
        }

        if (Vector3.Distance(transform.position, curEnemyTarget.transform.position) <= 1.5)
            SetState(UnitState.Attack);
    }

    void AttackUpdate()
    {
        SetAnimation(false);

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
        SetAnimation(true);

        if (curBuildRoom == null)
        {
            SetState(UnitState.Idle);
            return;
        }

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);

        SetBeeDirection(pos.x, des.x);


        if (Vector2.Distance(pos, des) < 0.01f)
        {
            SetState(UnitState.Build);
        }
    }

    void BuildUpdate()
    {
        SetAnimation(false);

        if (curBuildRoom.concructionDone)
        {
            MoveToPosition(new Vector2(0,0));
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
        SetAnimation(true);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);

        SetBeeDirection(pos.x, des.x);

        if (Vector2.Distance(pos, des) < 0.01f)
        {
            SetState(UnitState.Work);
        }
    }

    void WorkUpdate()
    {
        SetAnimation(false);

            foreach(Room room in Hive.instance.rooms)
            {
                Vector2 unitPos = agent.transform.position;
                Vector2 roomPos = room.transform.position;
                if(Vector2.Distance(unitPos, roomPos) < 0.01f)
                {
                    curBuildRoom = room;
                }
            }
        
        // zrovna pracuje tu
        curBuildRoom.WorkInRoom(gameObject.GetComponent<Unit>());
    }

    void MoveToRestRoomUpdate()
    {
        SetAnimation(true);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2();

        // TODO najdi nejblizsi volny rest room
        GameObject[] restRooms = GameObject.FindGameObjectsWithTag("RestRoom");

        if (restRooms != null && restRooms.Length > 0)
        {
            des = new Vector2(restRooms[0].transform.position.x, restRooms[0].transform.position.y);
            agent.SetDestination(new Vector3(des.x, des.y, 0));

            SetBeeDirection(pos.x, des.x);

            if (Vector2.Distance(pos, des) < 0.01f)
            {
                SetState(UnitState.Sleep);
            }
        }

        
    }

    void MoveToFoodRoomUpdate()
    {
        SetAnimation(true);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2();

        // TODO najdi nejblizsi volny rest room
        GameObject[] foodRooms = GameObject.FindGameObjectsWithTag("FoodRoom");

        if (foodRooms != null && foodRooms.Length > 0)
        {
            des = new Vector2(foodRooms[0].transform.position.x, foodRooms[0].transform.position.y);
            agent.SetDestination(new Vector3(des.x, des.y, 0));

            SetBeeDirection(pos.x, des.x);

            if (Vector2.Distance(pos, des) < 0.01f)
            {
                SetState(UnitState.Eat);
            }
        }
    }

    //// zobrazit vyber jednotky
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
        //curBuildRoom = room;
        SetState(UnitState.MoveToWork);
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    void SetState(UnitState toState)
    {
        // pokud pracovala, prestane odchodem pracovat
        if (state == UnitState.Work || state == UnitState.Sleep || state == UnitState.Eat)
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

    public void SetAnimation(bool setFlyingOn)
    {
        if (setFlyingOn)
        {
            animator.SetFloat("Speed", 1);
            borderAnimator.SetBool("Flies", true);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            borderAnimator.SetBool("Flies", false);
        }
    }

    public void SetBeeDirection(float pos, float dest)
    {
        if (dest < pos)
        {
            // otoc smerem doleva
            graphic.GetComponent<SpriteRenderer>().flipX = true;
            selectGraphic.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (dest > pos)
        {
            // otoc doprava
            graphic.GetComponent<SpriteRenderer>().flipX = false;
            selectGraphic.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

}
