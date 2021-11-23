using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Idle = 0, Patrol, Wait, Chase, Attack, Death }

public class Enemy : MonoBehaviour, ILivingEntity
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private Status status;
    private new Rigidbody rigidbody;
    private EnemyState state;
    private Vector3 originPos;
    [SerializeField] private float moveRange = 5;
    [SerializeField] private float detectRange = 5;
    [SerializeField] private float chaseRange = 5;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private float patrolSpeed = 2;
    [SerializeField] private float chaseSpeed = 3;
    [SerializeField] private float rotateSpeed = 5;
    private Transform target;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        status = GetComponent<Status>();
        rigidbody = GetComponent<Rigidbody>();
        state = EnemyState.Idle;
        originPos = transform.position;
    }

    private void Update()
    {
        if (state == EnemyState.Death) return;

        DetectPlayer();
        FSM();
    }

    private void DetectPlayer()
    {
        if (state == EnemyState.Attack) return;

        float range = target == null ? detectRange : chaseRange;

        Collider[] colliders = Physics.OverlapSphere(transform.position, range, 1 << LayerMask.NameToLayer("Player"));
        if (colliders.Length > 0)
        {
            target = colliders[0].transform;
            state = Vector3.Distance(target.position, transform.position) <= attackRange ? EnemyState.Attack : EnemyState.Chase;
        }
        else
        {
            target = null;
            if (state == EnemyState.Chase)
            {
                state = EnemyState.Idle;
            }
        }
    }

    private void FindRandomPos()
    {
        Vector3 destination = originPos;
        destination.x += Random.Range(-1f, 1f) * moveRange;
        destination.z += Random.Range(-1f, 1f) * moveRange;
        navMeshAgent.SetDestination(destination);
    }

    private IEnumerator Wait(float t)
    {
        state = EnemyState.Wait;

        yield return new WaitForSeconds(t);

        if (state == EnemyState.Wait)
        {
            state = EnemyState.Idle;
        }
    }

    private void FSM()
    {
        switch (state)
        {
            case EnemyState.Idle:
                navMeshAgent.speed = patrolSpeed; 
                animator.SetFloat("movementSpeed", 1.0f);
                FindRandomPos();
                state = EnemyState.Patrol;
                break;
            case EnemyState.Patrol:
                Debug.DrawLine(transform.position, navMeshAgent.destination, Color.blue, 0.1f);
                if ((navMeshAgent.destination - transform.position).sqrMagnitude < 0.01f)
                {
                    transform.position = navMeshAgent.destination;
                    navMeshAgent.ResetPath();
                    animator.SetFloat("movementSpeed", 0.0f);
                    StartCoroutine("Wait", 3);
                }
                break;
            case EnemyState.Chase:
                navMeshAgent.speed = chaseSpeed;
                animator.SetFloat("movementSpeed", 1.0f);
                navMeshAgent.SetDestination(target.position);
                break;
            case EnemyState.Attack:
                if (isAttacking) break;
                isAttacking = true;
                navMeshAgent.ResetPath();
                StartCoroutine("LookAt", target.position - transform.position);
                animator.SetTrigger("attack");
                break;
        }
    }

    private IEnumerator LookAt(Vector3 position)
    {
        Quaternion originRotation = transform.rotation;
        Quaternion lookRotation = Quaternion.LookRotation(position);

        float percent = 0;
        float current = 0;
        while (percent < 1)
        {
            transform.rotation = Quaternion.Slerp(originRotation, lookRotation, percent);
            current += Time.deltaTime;
            percent = current * rotateSpeed;
            yield return null;
        }
    }

    public void TakeDamage(int damage, Transform damageCauser)
    {
        status.HP = Mathf.Max(0, status.HP - damage);
        rigidbody.AddForce((transform.position - damageCauser.position).normalized * 100, ForceMode.Impulse);
        Debug.Log($"{name} : {status.HP}");
        if (status.HP == 0)
        {
            state = EnemyState.Death;
            navMeshAgent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            animator.SetTrigger("death");
            Destroy(gameObject, 5);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }
    }

    private void AttackStart()
    {
        rigidbody.AddForce((target.position - transform.position).normalized * 100, ForceMode.Impulse);
        Instantiate(status.skills[0], transform.position + (target.position - transform.position).normalized * attackRange + Vector3.up, Quaternion.identity).Init(status.Damage, "Enemy");
    }

    private void ComboCheck()
    {
        
    }

    public void AttackEnd()
    {
        isAttacking = false;
        state = EnemyState.Idle;
    }
}
