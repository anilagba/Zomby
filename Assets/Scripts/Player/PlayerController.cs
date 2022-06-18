using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(InputActionAsset))]
public class PlayerController : Humanoids
{
    [SerializeField] InputActionAsset inputActions;

    [Header("Health")]
    [SerializeField] TextMeshProUGUI healthText;
    const string healthTextString = " = ";

    [Header("Gun")]
    [SerializeField] Transform gunBarrel;
    [Tooltip("kurþunlarýn hareket hýzý")]
    [SerializeField] float fireSpeed;
    [Tooltip("kurþunlarýn geri teptirme oraný")]
    public float punchPower;
    public bool isMultiShootActive;

    [Header(" ")]
    [SerializeField] Animator animator;
    int moveAnimationBoolParameter = Animator.StringToHash("IsMoving");

    [SerializeField] Slider repairButtonSlider;
    [SerializeField] Button repairButton;

    Rigidbody rb;

    Vector3 moveDir = Vector3.zero;
    Quaternion lookRotation = Quaternion.identity;
    bool isRepairing;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        SetStats(30, 3, 0.15f, new WaitForSeconds(0.3f), false, true);

        healthText.text = healthTextString + hp.ToString();

        GameManager.Instance.AddNewPlayerToTheList(GetComponent<PlayerController>());

        isMultiShootActive = false;

        StartCoroutine(Fire());
    }


    private void FixedUpdate()
    {
        if (canMove) rb.MovePosition(transform.position + moveDir * speed);
        else rb.Sleep();  //player collider zomby collideri sýkýþtýrdýðýnda player colliderin geri doðru itilmesini engeller

        transform.rotation = lookRotation;
    }

    private void Update()
    {
        if (isRepairing) Repair();
        else repairButtonSlider.value = 0;
    }


    #region Health Update

    protected override void BehaveIfHPDropsDownZero()
    {
        hp = 0;
        TowerController.Instance.LoseHP(damage); //oyuncu ölmez, hasar kuleye yansýr
        return;
    }


    public override void ReceiveDamage(int damage)
    {
        base.ReceiveDamage(damage);
        healthText.text = healthTextString + hp.ToString();
        if (hp < 2) repairButton.gameObject.SetActive(false);
        else repairButton.gameObject.SetActive(true);
    }
    #endregion


    #region Attack Mehtods
    IEnumerator Fire()
    {
        if (canAttack)
        {
            BulletController bullet = BulletPooling.Instance.GetBulletFromPool();
            bullet.transform.SetPositionAndRotation(gunBarrel.position, gunBarrel.rotation);
            bullet.SetAttackPower(damage, punchPower);
            Rigidbody rb = bullet.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(bullet.transform.forward * fireSpeed, ForceMode.VelocityChange);

            if (isMultiShootActive)
            {
                BulletController bullet2 = BulletPooling.Instance.GetBulletFromPool();
                bullet2.transform.SetPositionAndRotation(gunBarrel.position, gunBarrel.rotation);
                bullet2.transform.Rotate(Vector3.up * 15);
                bullet2.SetAttackPower(damage, punchPower);
                Rigidbody rb2 = bullet2.gameObject.GetComponent<Rigidbody>();
                rb2.AddForce(bullet2.transform.forward * fireSpeed, ForceMode.VelocityChange);

                BulletController bullet3 = BulletPooling.Instance.GetBulletFromPool();
                bullet3.transform.SetPositionAndRotation(gunBarrel.position, gunBarrel.rotation);
                bullet3.transform.Rotate(Vector3.up * -15);
                bullet3.SetAttackPower(damage, punchPower);
                Rigidbody rb3 = bullet3.gameObject.GetComponent<Rigidbody>();
                rb3.AddForce(bullet3.transform.forward * fireSpeed, ForceMode.VelocityChange);
            }
        }

        yield return attackDelay;

        StartCoroutine(Fire());
    }

    public void AttackInputAction(InputAction.CallbackContext context)
    {
        Vector2 inputV2 = context.ReadValue<Vector2>();
        Vector3 rotateDir = new Vector3(inputV2.x, 0, inputV2.y);

        if (context.performed) lookRotation = Quaternion.LookRotation(rotateDir, Vector3.up);

        canAttack = context.performed;
    }

    #endregion


    #region Move Methods
    public void ChangeMoveSpeed(float speed)
    {
        this.speed = speed;
    }

    void Move(Vector3 pos)
    {
        if (!canMove) return;

        Vector2 inputV2 = pos;
        Vector3 moveDir = new Vector3(inputV2.x, 0, inputV2.y);

        this.moveDir = moveDir.normalized;
    }

    public void MoveInputAction(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>());

        animator.SetBool(moveAnimationBoolParameter, context.performed);

        canMove = context.performed;
    }

    //player objenin kulenin içinden geçmesini engeller
    //rigidbody collision detectioný continuous yapmak iþe yaramýyor
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.GetComponent<TowerController>()) return;
        rb.MovePosition(transform.position - moveDir * speed);
    }
    #endregion


    #region Repair
    public void StartRepairAction(InputAction.CallbackContext context)
    {
        isRepairing = context.performed;
    }

    //oyuncunun canýndan kuleninkine aktarýr
    void Repair()
    {
        repairButtonSlider.value += Time.fixedDeltaTime / 7;

        if (repairButtonSlider.value == 1)
        {
            ReceiveDamage(2);
            TowerController.Instance.LoseHP(-1);
            repairButtonSlider.value = 0;
        }
    }
    #endregion
}