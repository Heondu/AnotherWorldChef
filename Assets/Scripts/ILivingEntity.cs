using UnityEngine;

public interface ILivingEntity
{
    public void TakeDamage(int damage, Transform damageCauser);
}