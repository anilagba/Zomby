using UnityEngine;

public abstract class Humanoids : MonoBehaviour
{
    protected int hp;
    protected float speed;
    protected int damage;
    protected WaitForSeconds attackDelay;

    public bool canAttack;
    public bool canMove;

    protected virtual void SetStats(int hp, int damage, float speed, WaitForSeconds attackDelay, bool canAttack, bool canMove)
    {
        this.hp = hp;
        this.speed = speed;
        this.damage = damage;
        this.attackDelay = attackDelay;
        this.canAttack = canAttack;
        this.canMove = canMove;
    }


    public virtual void ChangeAttackSpeed(float newSpeed)
    {
        attackDelay = new WaitForSeconds(newSpeed);
    }


    protected abstract void BehaveIfHPDropsDownZero();


    public virtual void ReceiveDamage(int damage)
    {
        hp -= damage;
        if (hp < 1)
        {
            BehaveIfHPDropsDownZero();
        }
    }
}