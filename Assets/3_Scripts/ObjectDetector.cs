using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public enum ExplodeAttackType
{
    repeat,
    enter,
    once
}

public class ObjectDetector : MonoBehaviour
{
    private Skill skill;
    private SphereCollider sphereCollider;
    [SerializeField] private ExplodeAttackType attackType;
    [SerializeField] private float repeatTime = 1;
    [SerializeField] private float waitTime = 0;

    private List<GameObject> enterObjects = new List<GameObject>();

    private void Start()
    {
        skill = GetComponent<Skill>();
        sphereCollider = GetComponent<SphereCollider>();
        StartCoroutine("Attack");
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(waitTime);

        do
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, sphereCollider.radius);
            foreach (Collider collider in colliders)
            {
                if (attackType == ExplodeAttackType.enter)
                {
                    if (!enterObjects.Contains(collider.gameObject))
                    {
                        enterObjects.Add(collider.gameObject);
                        skill.Attack(collider.gameObject);
                    }
                }
                else
                {
                    skill.Attack(collider.gameObject);
                }
            }

            if (attackType == ExplodeAttackType.repeat)
            {
                yield return new WaitForSeconds(repeatTime);
            }
            else if (attackType == ExplodeAttackType.enter)
            {
                yield return null;
            }
            else
            {
                yield break;
            }

        } while (attackType != ExplodeAttackType.once);
    }
}
