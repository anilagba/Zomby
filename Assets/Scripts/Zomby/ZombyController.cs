using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombyController : Humanoids
{
    [SerializeField] protected float attackSpeed;

    protected NavMeshAgent agent;

    public Transform target;
    public IndicatorBehavior indicator;

    Transform collsionAttackTarget;
    protected Animator animator;
    int attackAnimTrigger = Animator.StringToHash("attack");
    int takeDamageAnimTrigger = Animator.StringToHash("damage");
    int deathAnimTrigger = Animator.StringToHash("death");

    private void Start()
    {
        SetStats(10, 1, 10, new WaitForSeconds(attackSpeed), true, true);

        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    private void LateUpdate()
    {
        Chase(target.position, agent);
    }



    protected void Chase(Vector3 targetPos, NavMeshAgent agent)
    {
        if (canMove) agent.SetDestination(targetPos);
        else agent.isStopped = true;
    }



    #region HP Methods

    protected override void BehaveIfHPDropsDownZero()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        canAttack = false;
        canMove = false;
        animator.SetTrigger(deathAnimTrigger);
        Destroy(indicator.gameObject);
        return;
    }

    public override void ReceiveDamage(int damage)
    {
        animator.SetTrigger(takeDamageAnimTrigger);
        base.ReceiveDamage(damage);
    }

    // ölüm animasyonu esnasýnda aktif olur
    protected virtual void DeathAnimEvent()
    {
        Destroy(gameObject);
        PowerUpManager.Instance.PopUpPowerUp(transform.position);
        GameManager.Instance.RemoveZombyFromList(gameObject.GetComponent<ZombyController>());
    }
    #endregion




    //receive damage animation event
    //zomby hasar aldýðýnda hareketsiz kalýr
    void StopMovingIfStumbled()
    {
        agent.isStopped = !agent.isStopped;
    }



    void AttackAnimEvent()
    {
        if (collsionAttackTarget.CompareTag("Tower") && TowerController.Instance.canLoseHp)
        {
            if (TowerController.Instance.canLoseHp) TowerController.Instance.LoseHP(damage);
        }
        else if (collsionAttackTarget.CompareTag("Player"))
        {
            PlayerController playerController = collsionAttackTarget.GetComponent<PlayerController>();
            playerController.ReceiveDamage(damage);
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        collsionAttackTarget = collision.transform;
        if (canAttack) animator.SetTrigger(attackAnimTrigger);
    }
}