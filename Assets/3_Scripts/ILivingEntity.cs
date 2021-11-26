using UnityEngine;

public interface ILivingEntity
{
    public void TakeDamage(int damage, GameObject eventInstigator, Transform damageCauser);
    public Vector3 GetAttackPosition();
    public Vector3 GetAttackDirection();
}