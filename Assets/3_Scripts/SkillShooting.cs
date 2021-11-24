using System.Collections.Generic;
using UnityEngine;

public class SkillShooting : MonoBehaviour
{
    public Skill[] Skills;

    private PlayerController playerController;

    public Dictionary<Skill, SkillTimer> skillTimers = new Dictionary<Skill, SkillTimer>();

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        Inventory.Instance.onItemChanged += UpdateShortcut;
        for (int i = 0; i < 4; i++)
        {
            Inventory.Instance.shortcuts[i] = Skills[i + 1];
        }
        Inventory.Instance.onItemChanged.Invoke();
    }

    private void Update()
    {
        if (GameManager.Instance.IsStop) return;

        ObserveSkillShot();
    }

    private void UpdateShortcut()
    {
        for (int i = 0; i < Inventory.Instance.shortcuts.Length; i++)
        {
            Skills[i + 1] = Inventory.Instance.shortcuts[i];
        }
    }

    private void ObserveSkillShot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SkillShot(Skills[0]);
        }
        if (Input.GetKeyDown("q"))
        {
            SkillShot(Skills[1]);
        }
        if (Input.GetKeyDown("w"))
        {
            SkillShot(Skills[2]);
        }
        if (Input.GetKeyDown("e"))
        {
            SkillShot(Skills[3]);
        }
        if (Input.GetKeyDown("r"))
        {
            SkillShot(Skills[4]);
        }
    }

    private void SkillShot(Skill skill)
    {
        if (skillTimers.ContainsKey(skill) == false)
        {
            skillTimers.Add(skill, new SkillTimer(skill, Time.time));
        }
        else if (skillTimers[skill].CanAttack() == false)
        {
            return;
        }

        skillTimers[skill].SetLastAttackTime();

        Skill clone = null;

        if(skill.skillType == SkillType.Cursor)
        {
            clone = Instantiate(skill, playerController.GetMousePos(), Quaternion.identity);
        }
        else if(skill.skillType == SkillType.Explode)
        {
            clone = Instantiate(skill, transform.position, Quaternion.identity);
        }
        else if(skill.skillType == SkillType.Projectile)
        {
            clone = Instantiate(skill, transform.position + playerController.GetMouseDir() * 2 + Vector3.up, Quaternion.LookRotation(playerController.GetMouseDir()));
        }

        clone.Init(playerController.Status.Damage, gameObject);
        AutoDestroy(clone.gameObject);
    }

    private void AutoDestroy(GameObject hitInstance)
    {
        var hitPs = hitInstance.GetComponent<ParticleSystem>();
        if (hitPs != null)
        {
            Destroy(hitInstance, hitPs.main.duration);
        }
    }
}
