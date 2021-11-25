using UnityEngine;

public enum SkillType { Projectile, Explode, Cursor }
public class Skill : MonoBehaviour
{
    public string skillName = "Å«ÆòÅ¸";
    public Sprite skillIcon;
    public SkillType skillType = SkillType.Projectile;
    public string skillDesc = "";
    public float skillDamageMult = 1;
    public float coolTime = 0;
    [HideInInspector] public int damage;
    [HideInInspector] public GameObject eventInstigator;

    public void Init(int damage, GameObject eventInstigator)
    {
        this.damage = Mathf.RoundToInt(damage * skillDamageMult);
        this.eventInstigator = eventInstigator;
    }

    public void Attack(GameObject other)
    {
        if (!other.CompareTag(eventInstigator.tag))
        {
            ILivingEntity entity = other.gameObject.GetComponent<ILivingEntity>();
            if (entity != null)
            {
                entity.TakeDamage(damage, eventInstigator, transform);
            }
        }
    }
}

public class SkillTimer
{
    private Skill skill;
    private float lastAttackTime;

    public SkillTimer(Skill skill, float lastAttackTime)
    {
        this.skill = skill;
        this.lastAttackTime = lastAttackTime;
    }

    public bool CanAttack()
    {
        if (Time.time - lastAttackTime >= skill.coolTime)
        {
            return true;
        }
        return false;
    }

    public void SetLastAttackTime()
    {
        lastAttackTime = Time.time;
    }

    public float GetCoolTimeRatio()
    {
        return 1 - ((Time.time - lastAttackTime) / skill.coolTime);
    }
}