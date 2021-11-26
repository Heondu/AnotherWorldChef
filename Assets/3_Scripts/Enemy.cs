using System.Collections.Generic;
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
    [SerializeField] private float chaseRange = 10;
    [SerializeField] private float hitNotifyRange = 5;
    [SerializeField] private float patrolSpeed = 2;
    [SerializeField] private float chaseSpeed = 3;
    [SerializeField] private float rotateSpeed = 5;
    [SerializeField] private float chaseTimeAtHit = 5;
    [SerializeField] private GameObject pickUpObject;
    [Range(0, 100)][SerializeField] private int skillSpawnProb = 10;
    private Transform target;
    private bool isAttacking = false;
    private bool isHit = false;

    private Skill nextSkill = null;
    private SkillShooting skillShooting;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        status = GetComponent<Status>();
        rigidbody = GetComponent<Rigidbody>();
        skillShooting = GetComponent<SkillShooting>();
        state = EnemyState.Idle;
        originPos = transform.position;
    }

    private void Update()
    {
        if (state == EnemyState.Death) return;

        CheckForNextSkill();
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
            if (nextSkill == null)
            {
                state = EnemyState.Chase;
            }
            else if (Vector3.Distance(target.position, transform.position) <= nextSkill.attackRange)
            {
                state = EnemyState.Attack;
            }
            else
            {
                state = EnemyState.Chase;
            }
        }
        else if (!isHit)
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
        Vector3 randomDirection = Random.insideUnitSphere * moveRange;
        randomDirection += originPos;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, moveRange, -1);
        navMeshAgent.SetDestination(navHit.position);
        StartCoroutine("ResetDestination", 3f);
    }

    private IEnumerator ResetDestination(float t)
    {
        yield return new WaitForSeconds(t);

        if (state == EnemyState.Patrol)
            navMeshAgent.SetDestination(transform.position);
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
                navMeshAgent.ResetPath();
                StartCoroutine("LookAt", GetTargetDirection(true));
                if (nextSkill != null)
                {
                    isAttacking = true;
                    animator.SetTrigger("attack");
                }
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

    public void TakeDamage(int damage, GameObject eventInstigator, Transform damageCauser)
    {
        status.HP = Mathf.Max(0, status.HP - damage);
        rigidbody.AddForce((transform.position - damageCauser.position).normalized * 100, ForceMode.Impulse);
        //if (state != EnemyState.Patrol)
        //{
        //    AttackEnd();
        //    animator.Play("Hit");
        //}
        Collider[] colliders = Physics.OverlapSphere(transform.position, hitNotifyRange, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.target = eventInstigator.transform;
            enemy.StopCoroutine("HitChase");
            enemy.StartCoroutine("HitChase", chaseTimeAtHit);
        }

        if (status.HP == 0)
        {
            state = EnemyState.Death;
            navMeshAgent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            animator.Play("Death");
            if (Random.Range(0, 100) < skillSpawnProb)
                Instantiate(pickUpObject, transform.position, Quaternion.identity);
            Destroy(gameObject, 5);
        }
    }

    public IEnumerator HitChase(float t)
    {
        isHit = true;
        state = EnemyState.Chase;

        yield return new WaitForSeconds(t);

        isHit = false;
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

    private bool CheckForNextSkill()
    {
        if (nextSkill != null) return true;

        Skill skill = status.skills[Random.Range(0, status.skills.Count)];

        if (skillShooting.skillTimers.ContainsKey(skill) == false)
        {
            nextSkill = skill;
            return true;
        }
        else if (skillShooting.skillTimers[skill].CanAttack() == false)
        {
            nextSkill = null;
            return false;
        }
        else
        {
            nextSkill = skill;
            return true;
        }
    }

    private void AttackStart()
    {
        rigidbody.AddForce((target.position - transform.position).normalized * 100, ForceMode.Impulse);

        skillShooting.SkillShot(nextSkill, GetComponent<ILivingEntity>(), status.Damage);

        nextSkill = null;
    }

    private void ComboCheck()
    {
        
    }

    public void AttackEnd()
    {
        isAttacking = false;
        state = EnemyState.Idle;
    }

    public Vector3 GetAttackPosition()
    {
        return target.position;
    }

    public Vector3 GetAttackDirection()
    {
        return GetTargetDirection(false);
    }

    private Vector3 GetTargetDirection(bool freezeY)
    {
        if (freezeY)
        {
            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 to = new Vector3(target.position.x, 0, target.position.z);
            return (to - from).normalized;
        }
        else
        {
            return (target.position - transform.position).normalized;
        }
    }
}
