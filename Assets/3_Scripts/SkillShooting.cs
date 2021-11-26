using System.Collections.Generic;
using UnityEngine;

public class SkillShooting : MonoBehaviour
{
    public Dictionary<Skill, SkillTimer> skillTimers = new Dictionary<Skill, SkillTimer>();

    public bool SkillShot(Skill skill, ILivingEntity entity, int damage)
    {
        if (skill == null) return false;

        if (!CheckForSkillCooltime(skill)) return false;

        skillTimers[skill].SetLastAttackTime();

        Skill clone = null;

        if(skill.skillType == SkillType.Cursor)
        {
            clone = Instantiate(skill, entity.GetAttackPosition(), Quaternion.identity);
        }
        else if(skill.skillType == SkillType.Explode)
        {
            clone = Instantiate(skill, transform.position, Quaternion.identity);
        }
        else if(skill.skillType == SkillType.Projectile)
        {
            clone = Instantiate(skill, transform.position + entity.GetAttackDirection() + Vector3.up / 2, Quaternion.LookRotation(entity.GetAttackDirection()));
        }

        clone.Init(damage, gameObject);
        AutoDestroy(clone.gameObject);

        return true;
    }

    private void AutoDestroy(GameObject hitInstance)
    {
        var hitPs = hitInstance.GetComponent<ParticleSystem>();
        if (hitPs != null)
        {
            Destroy(hitInstance, hitPs.main.duration);
        }
    }

    public bool CheckForSkillCooltime(Skill skill)
    {
        if (skillTimers.ContainsKey(skill) == false)
        {
            skillTimers.Add(skill, new SkillTimer(skill, Time.time));
            return true;
        }
        else if (skillTimers[skill].CanAttack() == false)
        {
            return false;
        }
        return true;
    }
}
