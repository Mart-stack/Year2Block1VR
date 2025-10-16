using UnityEngine;
using UnityEngine.AI;

public class NPCPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform patrolPathRoot;
    public float waitAtPoint = 1f;

    [Header("Detection Settings")]
    public float detectRadius = 8f;
    [Range(0, 180)] public float viewAngle = 60f;
    public LayerMask obstructionMask; 
    public LayerMask playerMask;      
    public string playerTag = "Player";
    public float spotCooldown = 0.2f;
    public float lostSightTime = 2f;

    [Header("References")]
    public NavMeshAgent agent;
    public Transform eyesTransform;
    public CubeResponder responderToActivate;

    private Transform[] points;
    private int currentIndex = 0;
    private float waitTimer = 0f;
    private float checkTimer = 0f;

    private Transform targetPlayer;
    private float lastSeenTimer = 0f;

    private enum State { Patrol, Chase, Search }
    private State state = State.Patrol;

    void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!eyesTransform) eyesTransform = transform;

        
        if (patrolPathRoot == null || patrolPathRoot.childCount == 0)
        {
            Debug.LogError("PatrolPathRoot пуст — NPC не знает куда идти");
            enabled = false;
            return;
        }

        points = new Transform[patrolPathRoot.childCount];
        for (int i = 0; i < points.Length; i++)
            points[i] = patrolPathRoot.GetChild(i);

        GoTo(points[0].position);
    }

    void Update()
    {
        checkTimer += Time.deltaTime;

        switch (state)
        {
            case State.Patrol:
                PatrolTick();

                if (checkTimer >= spotCooldown && CheckSeePlayer(out Transform player))
                {
                    checkTimer = 0f;
                    StartChase(player);
                }
                break;

            case State.Chase:
                ChaseTick();
                break;

            case State.Search:
                SearchTick();
                break;
        }
    }

    
    void PatrolTick()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtPoint)
            {
                waitTimer = 0f;
                currentIndex = (currentIndex + 1) % points.Length;
                GoTo(points[currentIndex].position);
            }
        }
    }

    
    void ChaseTick()
    {
        if (targetPlayer == null)
        {
            StartSearch();
            return;
        }

        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPlayer.position, out hit, 2f, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.SetDestination(hit.position);
        }

      
        checkTimer += Time.deltaTime;
        if (checkTimer >= spotCooldown)
        {
            checkTimer = 0f;

            if (CheckSeePlayer(out Transform again))
            {
                targetPlayer = again;
                lastSeenTimer = 0f;
            }
            else
            {
                lastSeenTimer += spotCooldown;
                if (lastSeenTimer >= lostSightTime)
                    StartSearch();
            }
        }
    }

    
    void SearchTick()
    {
        agent.isStopped = true;
        waitTimer += Time.deltaTime;

        if (waitTimer >= 1.5f)
        {
            waitTimer = 0f;
            state = State.Patrol;
            GoTo(points[currentIndex].position);
        }
    }

    void GoTo(Vector3 pos)
    {
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    void StartChase(Transform player)
    {
        state = State.Chase;
        targetPlayer = player;
        lastSeenTimer = 0f;

        if (responderToActivate)
            responderToActivate.OnAlert();
    }

    void StartSearch()
    {
        state = State.Search;
        targetPlayer = null;
        agent.isStopped = true;
    }

    bool CheckSeePlayer(out Transform playerTransform)
    {
        playerTransform = null;

        Collider[] hits = Physics.OverlapSphere(eyesTransform.position, detectRadius, playerMask);
        foreach (var col in hits)
        {
            if (!col.CompareTag(playerTag)) continue;

            Transform p = col.transform;
            Vector3 dir = (p.position - eyesTransform.position).normalized;
            float angle = Vector3.Angle(eyesTransform.forward, dir);
            if (angle > viewAngle * 0.5f) continue;

            float dist = Vector3.Distance(eyesTransform.position, p.position);
            if (!Physics.Raycast(eyesTransform.position, dir, dist, obstructionMask))
            {
                playerTransform = p;
                return true;
            }
        }
        return false;
    }

  
    void OnDrawGizmosSelected()
    {
        Transform eyes = eyesTransform ? eyesTransform : transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(eyes.position, detectRadius);

        Gizmos.color = Color.cyan;
        Vector3 left = Quaternion.Euler(0, -viewAngle / 2f, 0) * eyes.forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2f, 0) * eyes.forward;
        Gizmos.DrawRay(eyes.position, left * detectRadius);
        Gizmos.DrawRay(eyes.position, right * detectRadius);
    }
}