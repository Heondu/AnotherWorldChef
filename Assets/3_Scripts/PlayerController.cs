using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum PlayerState { Idle = 0, Move, Attack, Death }

public class PlayerController : MonoBehaviour, ILivingEntity
{
    #region Variable & Property
    private Camera mainCamera;
    [SerializeField] private float rotateSpeed = 5;
    [SerializeField] private int maxCombo = 3;
    [SerializeField] private bool canBeDamaged = true;
    private int currentCombo = 0;
    private bool isAttacking = false;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private PlayerState state;
    public Status Status { get; private set; }
    private new Rigidbody rigidbody;
    private SkillShooting skillShooting;

    [HideInInspector] public UnityEvent onHPValueChanged = new UnityEvent(); 
    #endregion

    public void Init(Camera cam, int hp)
    {
        mainCamera = cam;
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Status = GetComponent<Status>();
        state = PlayerState.Idle;
        rigidbody = GetComponent<Rigidbody>();
        skillShooting = GetComponent<SkillShooting>();
        Status.HP = hp;
    }

    private void Update()
    {
        if (GameManager.Instance.IsStop) return;
        if (state == PlayerState.Death) return;

        AttackCheck();
        Move();
        FSM();
    }

    public Vector3 GetMousePos()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Enemy")))
        {
            return hit.point;
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 position = hit.point;
            position.y = transform.position.y;
            return position;
        }
        return -Vector3.one;
    }

    public Vector3 GetMouseDir(bool freezeY)
    {
        Vector3 mousePos = GetMousePos();

        if (freezeY)
        {
            Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 to = new Vector3(mousePos.x, 0, mousePos.z);
            return (to - from).normalized;

        }
        else
        {
            return (mousePos - transform.position).normalized;
        }
    }

    private void Move()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, -1))
            {
                if (Vector3.Distance(transform.position, hit.point) >= 0.5f)
                {
                    state = PlayerState.Move;
                    animator.SetFloat("movementSpeed", 1.0f);
                    navMeshAgent.SetDestination(hit.point);
                }
            }
        }
    }

    private void AttackCheck()
    {
        if (GameManager.Instance.IsStop) return;

        if (Input.GetMouseButton(0))
        {
            if (skillShooting.SkillShot(Status.skills[0], GetComponent<ILivingEntity>(), Status.Damage))
                state = PlayerState.Attack;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Vector3 start = new Vector3(transform.position.x, 1, transform.position.z);
                Vector3 end = (new Vector3(hit.point.x, 1, hit.point.z) - start).normalized;
                Debug.DrawLine(start, start + end * 2, Color.red, 1);
            }
        }
        if (Input.GetKeyDown("q"))
        {
            if (skillShooting.SkillShot(Status.skills[1], GetComponent<ILivingEntity>(), Status.Damage))
                state = PlayerState.Attack;
        }
        if (Input.GetKeyDown("w"))
        {
            if (skillShooting.SkillShot(Status.skills[2], GetComponent<ILivingEntity>(), Status.Damage))
                state = PlayerState.Attack;
        }
        if (Input.GetKeyDown("e"))
        {
            if (skillShooting.SkillShot(Status.skills[3], GetComponent<ILivingEntity>(), Status.Damage))
                state = PlayerState.Attack;
        }
        if (Input.GetKeyDown("r"))
        {
            if (skillShooting.SkillShot(Status.skills[4], GetComponent<ILivingEntity>(), Status.Damage))
                state = PlayerState.Attack;
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

    private void FSM()
    {
        switch (state)
        {
            case PlayerState.Idle:
                animator.SetFloat("movementSpeed", 0.0f);
                break;
            case PlayerState.Move:
                if ((navMeshAgent.destination - transform.position).sqrMagnitude < 0.01f)
                {
                    state = PlayerState.Idle;
                    animator.SetFloat("movementSpeed", 0.0f);
                    transform.position = navMeshAgent.destination;
                    navMeshAgent.ResetPath();
                }
                break;
            case PlayerState.Attack:
                if (isAttacking) return;
                isAttacking = true;
                navMeshAgent.ResetPath();
                Attack();
                state = PlayerState.Idle;
                break;
        }
    }

    public void TakeDamage(int damage, GameObject eventInstigator, Transform damageCauser)
    {
        if (!canBeDamaged) return;

        Status.HP = Mathf.Max(0, Status.HP - damage);
        onHPValueChanged.Invoke();
        rigidbody.AddForce((transform.position - damageCauser.position).normalized * 100, ForceMode.Impulse);
        if (state == PlayerState.Idle)
        {
            AttackEnd();
            animator.Play("Hit");
        }
        navMeshAgent.ResetPath();
        state = PlayerState.Idle;
        if (Status.HP == 0)
        {
            state = PlayerState.Death;
            navMeshAgent.enabled = false;
            GetComponent<Collider>().enabled = false;
            animator.Play("Death");
            GameManager.Instance.IsDead = true;
            GameManager.Instance.IsStop = true;
        }
    }

    public float GetHPRatio()
    {
        return (float)Status.HP / Status.MaxHP;
    }

    private void AttackStart()
    {
        rigidbody.AddForce(GetMouseDir(true) * 100 * currentCombo, ForceMode.Impulse);
    }

    private void ComboCheck()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        currentCombo = Mathf.Min(maxCombo, currentCombo + 1);
        animator.Play("Attack" + currentCombo);
        StartCoroutine("LookAt", GetMouseDir(true));
    }

    private void AttackEnd()
    {
        isAttacking = false;
        currentCombo = 0;
        state = PlayerState.Idle;
    }

    public Vector3 GetAttackPosition()
    {
        return GetMousePos();
    }

    public Vector3 GetAttackDirection()
    {
        return GetMouseDir(false);
    }
}
