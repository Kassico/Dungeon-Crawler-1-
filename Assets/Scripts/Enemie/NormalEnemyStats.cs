using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/NormalEnemyStats")]
public class EnemyStats: ScriptableObject // bara ett scriptable object som håller alla stats för en normal enemy, används för att lätt kunna ändra stats i inspector utan att behöva ändra i koden, och för att kunna ha olika stats för olika typer
{
    public string enemyType = "NormalEnemy";

    public float maxHealth = 5;
    public float moveSpeed = 2f;
    public float chaseRange = 6f;
    public float attackDamage = 0.5f;
    public float attackRange = 1.1f;
    public float attackRate = 2f;
    public float knockbackForceResistans = 0.5f;
    public float scoreValue = 1f;
    public float attackDeley = 0.5f;
    public float attackHitBox = 1.4f;
    public float rotateSpeed = 0.8f;        //används ändast för ranged enemies, hur snabbt de kan snurra runt för att sikta mor player, kanske ska användas.
    public float projectileSpeed = 5f;     //används ändast för ranged enemies, hastigheten på deras projektiler.
}
