using UnityEngine;

public interface ILivingEntity
{
    public void TakeDamage(int damage, GameObject eventInstigator, Transform damageCauser);
}