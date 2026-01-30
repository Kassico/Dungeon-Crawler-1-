using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/NormalEnemyStats")]
public class EnemyStats: ScriptableObject
{
    public string enemyType = "NormalEnemy";

    public float normalMaxHealth = 5;
    public float normalMoveSpeed = 2f;
    public float normalChaseRange = 6f;
    public float normalAttackDamage = 0.5f;
    public float normalAttackRange = 1.1f;
    public float normalAttackRate = 2f;
    public float normalKnockbackForceResistans = 0.5f;
    public float normalScoreValue = 1f;
    public float normalAttackDeley = 0.5f;
    public float normalAttackHitBox = 1.4f;
}
