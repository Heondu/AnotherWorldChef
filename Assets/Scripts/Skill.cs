using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName = "Å«ÆòÅ¸";
    public string skillDesc = "";
    public float skillDamageMult = 1;
    public float coolTime = 0;
    public float maxRange = 1;
    private int damage;
    private string instigatorTag;

    public void Init(int damage, string instigatorTag)
    {
        this.damage = Mathf.RoundToInt(damage * skillDamageMult);
        this.instigatorTag = instigatorTag;
        GetComponent<SphereCollider>().radius = maxRange;
        GetComponent<SphereCollider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(instigatorTag))
        {
            ILivingEntity entity = other.GetComponent<ILivingEntity>();
            if (entity != null)
            {
                entity.TakeDamage(damage, transform);
            }    
        }

        Destroy(gameObject);
    }
}
