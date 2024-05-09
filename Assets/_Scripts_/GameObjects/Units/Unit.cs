//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

/// <summary>
/// Defines the possible states of a unit within the game.
/// </summary>
public enum UnitState
{
    Idle,                // The unit is idle.
    Move,                // The unit is moving to a target position.
    MoveToResource,      // The unit is moving to a resource.
    Gather,              // The unit is gathering resources.
    MoveToEnemy,         // The unit is moving towards an enemy.
    Attack,              // The unit is attacking an enemy.
    MoveToBuild,         // The unit is moving to a construction site.
    Build,               // The unit is building or repairing a structure.
    MoveToWork,          // The unit is moving to a workplace.
    Work,                // The unit is working in a room.
    MoveToFoodRoom,      // The unit is moving to the food room to eat.
    Eat,                 // The unit is eating in the food room.
    MoveToRestRoom,      // The unit is moving to the rest room to sleep.
    Sleep,               // The unit is sleeping in the rest room.
    SearchMove,          // The unit is moving to a search area.
    Searching            // The unit is searching in a specific area.
}

/// <summary>
/// Manages the attributes and behaviors of a unit, including movement, combat, resource gathering, and building.
/// </summary>
public class Unit : MonoBehaviour
{
    [Header("Components")]
    public GameObject selectionVisual;      // Visual indicator for when the unit is selected.
    public UnitHealth healthBar;            // Health bar UI component.
    private NavMeshAgent agent;             // The NavMesh agent for handling movement.
    public Player player;                   // Reference to the player who owns this unit.

    [Header("Stats")]
    public UnitState state;                 // Current state of the unit.
    public int curHp;                       // Current health of the unit.
    public int maxHp;                       // Maximum health of the unit.

    public int minAttackDamage;             // Minimum damage the unit can inflict.
    public int maxAttackDamage;             // Maximum damage the unit can inflict.

    public float attackRate;                // Rate at which the unit attacks.
    private float lastAttackTime;           // Time when the unit last attacked.

    public float pathUpdateRate = 1.0f;     // Rate at which the unit's pathfinding is updated.
    private float lastPathUpdateTime;       // Time when the unit's path was last updated.

    public int gatherAmount;                // Amount of resources the unit can gather at one time.
    public float gatherRate;                // Rate at which the unit gathers resources.
    private float lastGatherTime;           // Time when the unit last gathered resources.

    public int buildAmount;                 // Amount of building work the unit can do at one time.
    public float buildRate;                 // Rate at which the unit builds or repairs.
    private float lastBuildTime;            // Time when the unit last performed building work.

    public int searchAmount;                // Amount of searching work the unit can do at one time.
    public float searchRate;                // Rate at which the unit searches.
    private float lastSearchTime;           // Time when the unit last performed searching work.

    public int curFeed;                     // Current amount of food the unit has consumed.
    public int maxFeed;                     // Maximum amount of food the unit can consume before needing more.

    public int curEnergy;                   // Current energy level of the unit.
    public int maxEnergy;                   // Maximum energy level of the unit.

    public Animator animator;               // Animator for the unit's animations.
    public Animator borderAnimator;         // Animator for the unit's border animations.

    public GameObject graphic;              // Graphic representation of the unit.
    public GameObject selectGraphic;        // Graphic representation of the unit's selection state.

    private UnitAI curEnemyTarget;          // Current enemy target the unit is attacking.
    private ResourceSource curResourceSource; // Current resource source the unit is gathering from.
    private Room curBuildRoom;              // Current room the unit is building or working in.
    private ResourceTile curResourceTile;   // Current resource tile the unit is searching.

    public float checkRate;                 // Rate at which the unit checks its status.

    /// <summary>
    /// Event triggered when the state of the unit changes.
    /// </summary>
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<UnitState> { }
    public StateChangeEvent onStateChange;

    /// <summary>
    /// Initializes the unit, setting up its initial state and starting its behavior loops.
    /// </summary>
    private void Start()
    {
        SetState(UnitState.Idle);
        InvokeRepeating(nameof(RecalculateHunger), 0.0f, checkRate);
        InvokeRepeating(nameof(RecalculateEnergy), 0.0f, checkRate);
        InvokeRepeating(nameof(CheckForEnemies), 0.0f, checkRate);
    }

    /// <summary>
    /// Recalculates the unit's hunger and updates its state if necessary.
    /// </summary>
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
            // The unit goes to eat if it's hungry
            SetState(UnitState.MoveToFoodRoom);
        }

        if (curFeed < 5)
        {
            Log.instance.AddNewLogText(Time.time, "Bee is starving", Color.red);
        }
    }

    /// <summary>
    /// Recalculates the unit's energy based on its current activity and updates its state if necessary.
    /// </summary>
    void RecalculateEnergy()
    {
        switch (state)
        {
            case (UnitState)1 or (UnitState)2 or (UnitState)4 or (UnitState)6 or (UnitState)8 or (UnitState)10 or (UnitState)12 or (UnitState)14:
                {
                    curEnergy -= 1;
                    break;
                }
            case (UnitState)3 or (UnitState)5 or (UnitState)7 or (UnitState)9 or (UnitState)15:
                {
                    curEnergy -= 3;
                    break;
                }
            // If the unit is in IDLE or other low energy states
            default: { break; }
        }

        // If energy is low, the unit stops working and goes to sleep
        if (curEnergy <= 20)
        {
            SetState(UnitState.MoveToRestRoom);
        }
    }

    /// <summary>
    /// Checks for nearby enemies and initiates an attack if one is found.
    /// </summary>
    private void CheckForEnemies()
    {
        // If the unit is not working, it will attack enemies
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

            // If an enemy unit is close enough
            if (newTarget != null && closestDist < 10)
            {
                AttackUnit(newTarget.GetComponent<UnitAI>());
            }
        }
    }

    /// <summary>
    /// Initializes the unit's necessary components and settings.
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        ToggleSelectionVisual(false);
    }

    /// <summary>
    /// Updates the unit's based on its current state.
    /// </summary>
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
    /// Handles the searching of the unit.
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

            // There's a chance the unit will die based on the tile's danger value
            if (Random.Range(0, 100) < curResourceTile.dangerValue / 20)
            {
                Log.instance.AddNewLogText(Time.time, "Bee died while exploring the area.", Color.black);
                Die();
            }
        }
    }

    /// <summary>
    /// Updates the unit's when moving to search.
    /// </summary>
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

    /// <summary>
    /// Updates the unit's movement.
    /// </summary>
    void MoveUpdate()
    {
        SetAnimation(true);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2(agent.destination.x, agent.destination.y);

        SetBeeDirection(pos.x, des.x);

        if (Vector2.Distance(pos, des) <= 0.01f)
            SetState(UnitState.Idle);
    }

    /// <summary>
    /// Updates the unit's moving to a resource.
    /// </summary>
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

    /// <summary>
    /// Handles the gathering of the unit.
    /// </summary>
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

    /// <summary>
    /// Updates the unit's behavior when moving to an enemy.
    /// </summary>
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

    /// <summary>
    /// Handles the attacking of the unit.
    /// </summary>
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

        // If the target is too far, move closer
        if (Vector3.Distance(transform.position, curEnemyTarget.transform.position) > 1.5)
            SetState(UnitState.MoveToEnemy);
    }

    /// <summary>
    /// Updates the unit's moving to build.
    /// </summary>
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

    /// <summary>
    /// Handles the building by the unit.
    /// </summary>
    void BuildUpdate()
    {
        SetAnimation(false);

        if (curBuildRoom.concructionDone)
        {
            MoveToPosition(new Vector2(0, 0));
            return;
        }

        if (Time.time - lastBuildTime > buildRate)
        {
            lastBuildTime = Time.time;
            curBuildRoom.BuildRoom(buildAmount);
        }
    }

    /// <summary>
    /// Updates the unit's moving to work.
    /// </summary>
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

    /// <summary>
    /// Handles the work of the unit.
    /// </summary>
    void WorkUpdate()
    {
        SetAnimation(false);

        foreach (Room room in Hive.instance.rooms)
        {
            Vector2 unitPos = agent.transform.position;
            Vector2 roomPos = room.transform.position;
            if (Vector2.Distance(unitPos, roomPos) < 0.01f)
            {
                curBuildRoom = room;
            }
        }

        curBuildRoom.WorkInRoom(gameObject.GetComponent<Unit>());
    }

    void MoveToRestRoomUpdate()
    {
        SetAnimation(true);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2();

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

    /// <summary>
    /// Updates the unit's moving to the food room.
    /// </summary>
    void MoveToFoodRoomUpdate()
    {
        SetAnimation(true);

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 des = new Vector2();

        // Find the nearest available food room
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

    /// <summary>
    /// Shows the selection visual for the unit.
    /// </summary>
    /// <param name="selected">Whether the unit is selected.</param>
    public void ToggleSelectionVisual(bool selected)
    {
        if (selectionVisual != null)
            selectionVisual.SetActive(selected);
    }

    /// <summary>
    /// Moves the unit to a specified position.
    /// </summary>
    /// <param name="target">The target position to move to.</param>
    public void MoveToPosition(Vector2 target)
    {
        SetState(UnitState.Move);
        agent.SetDestination(new Vector3(target.x, target.y, 0));
    }

    /// <summary>
    /// Initiates resource gathering by the unit.
    /// </summary>
    /// <param name="resource">The resource source to gather from.</param>
    /// <param name="pos">The position of the resource.</param>
    public void GatherResource(ResourceSource resource, Vector3 pos)
    {
        curResourceSource = resource;
        SetState(UnitState.MoveToResource);
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    /// <summary>
    /// Initiates building or construction by the unit.
    /// </summary>
    /// <param name="room">The room to be built.</param>
    /// <param name="pos">The position of the construction site.</param>
    public void BuildRoom(Room room, Vector3 pos)
    {
        curBuildRoom = room;
        SetState(UnitState.MoveToBuild);
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    /// <summary>
    /// Initiates searching by the unit.
    /// </summary>
    /// <param name="tile">The resource tile to search.</param>
    /// <param name="pos">The position to move to for searching.</param>
    public void Searching(ResourceTile tile, Vector3 pos)
    {
        curResourceTile = tile;
        SetState(UnitState.SearchMove);
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    /// <summary>
    /// Moves the unit to a room for work.
    /// </summary>
    /// <param name="room">The room to work in.</param>
    /// <param name="pos">The position of the room.</param>
    public void WorkInRoom(Room room, Vector3 pos)
    {
        //curBuildRoom = room;
        SetState(UnitState.MoveToWork);
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    /// <summary>
    /// Changes the current state of the unit.
    /// </summary>
    /// <param name="toState">The new state to transition to.</param>
    void SetState(UnitState toState)
    {
        // If the unit was working, it stops working due to leaving.
        if (state == UnitState.Work || state == UnitState.Sleep || state == UnitState.Eat)
        {
            curBuildRoom.StopWorkInRoom(gameObject.GetComponent<Unit>());
        }

        state = toState;

        if (onStateChange != null)
        {
            onStateChange.Invoke(state);
        }
        if (toState == UnitState.Idle)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    /// <summary>
    /// Initiates an attack on a target enemy unit.
    /// </summary>
    /// <param name="target">The enemy unit to attack.</param>
    public void AttackUnit(UnitAI target)
    {
        curEnemyTarget = target;
        SetState(UnitState.MoveToEnemy);
    }

    /// <summary>
    /// Applies damage to the unit.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        curHp -= damage;
        if (curHp <= 0)
        {
            Die();
        }

        healthBar.UpdateHealthBar(curHp, maxHp);
    }

    /// <summary>
    /// Handles the death of the unit.
    /// </summary>
    void Die()
    {
        if (player != null)
        {
            player.units.Remove(this);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the animation state of the unit.
    /// </summary>
    /// <param name="setFlyingOn">Whether the flying animation should be on.</param>
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

    /// <summary>
    /// Sets the visual direction of the unit based on movement.
    /// </summary>
    /// <param name="pos">The current position of the unit.</param>
    /// <param name="dest">The destination position of the unit.</param>
    public void SetBeeDirection(float pos, float dest)
    {
        if (dest < pos)
        {
            // Turn to the left
            graphic.GetComponent<SpriteRenderer>().flipX = true;
            selectGraphic.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (dest > pos)
        {
            // Turn to the right
            graphic.GetComponent<SpriteRenderer>().flipX = false;
            selectGraphic.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
