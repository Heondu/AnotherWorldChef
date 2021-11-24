using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum PlayerState { Idle = 0, Move, Attack, Death }

public class PlayerController : MonoBehaviour, ILivingEntity
{
    #region Variable & Property
    private Camera mainCamera;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float rotateSpeed = 5;
    [SerializeField] private int maxCombo = 3;
    private int currentCombo = 0;
    private bool isComboInputOn = false;
    private bool isAttacking = false;
    private Vector3 mouseDir;

    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private PlayerState state;
    public Status Status { get; private set; }
    private new Rigidbody rigidbody;

    [HideInInspector] public UnityEvent onHPValueChanged = new UnityEvent(); 
    #endregion

    public void Init(Camera cam)
    {
        mainCamera = cam;
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Status = GetComponent<Status>();
        state = PlayerState.Idle;
        rigidbody = GetComponent<Rigidbody>();
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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        return -Vector3.one;
    }

    public Vector3 GetMouseDir()
    {
        Vector3 mousePos = GetMousePos();
        Vector3 start = new Vector3(transform.position.x, 1, transform.position.z);
        Vector3 end = (new Vector3(mousePos.x, 1, mousePos.z) - start).normalized;
        mouseDir = end;
        return mouseDir;
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
        if (state == PlayerState.Attack)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isComboInputOn = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Vector3 start = new Vector3(transform.position.x, 1, transform.position.z);
                Vector3 end =  (new Vector3(hit.point.x, 1, hit.point.z) - start).normalized;
                mouseDir = end;
                Debug.DrawLine(start, start + end * attackRange, Color.red, 1);

                //if (Physics.Raycast(start, end, out hit, attackRange, 1 << LayerMask.NameToLayer("Enemy")))
                //{
                //    Enemy enemy = hit.collider.GetComponent<Enemy>();
                //    if (enemy != null && Vector3.Distance(enemy.transform.position, transform.position) <= attackRange)
                //    {
                //        enemy.TakeDamage(status.Damage);
                //    }
                //}
            }
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
                if (isAttacking) break;
                isAttacking = true;
                navMeshAgent.ResetPath();
                Attack();
                break;
        }
    }

    public void TakeDamage(int damage, GameObject eventInstigator, Transform damageCauser)
    {
        Status.HP = Mathf.Max(0, Status.HP - damage);
        onHPValueChanged.Invoke();
        rigidbody.AddForce((transform.position - damageCauser.position).normalized * 100, ForceMode.Impulse);
        Debug.Log($"{name} : {Status.HP}");
        if (Status.HP == 0)
        {
            state = PlayerState.Death;
            navMeshAgent.enabled = false;
            GetComponent<Collider>().enabled = false;
            animator.SetTrigger("death");
        }
    }

    public float GetHPRatio()
    {
        return (float)Status.HP / Status.MaxHP;
    }

    private void AttackStart()
    {
        rigidbody.AddForce(mouseDir * 100 * currentCombo, ForceMode.Impulse);
        //Instantiate(status.skills[0], transform.position + mouseDir * attackRange + Vector3.up, Quaternion.identity).Init(status.Damage, "Player");
    }

    private void ComboCheck()
    {
        if (isComboInputOn)
        {
            isComboInputOn = false;
            Attack();
        }
    }

    private void Attack()
    {
        currentCombo = Mathf.Min(maxCombo, currentCombo + 1);
        animator.Play("Attack" + currentCombo);
        StartCoroutine("LookAt", mouseDir);
    }

    private void AttackEnd()
    {
        isAttacking = false;
        isComboInputOn = false;
        currentCombo = 0;
        state = PlayerState.Idle;
    }
}
