using System.Collections;
using UnityEngine;

public enum PowerUpTypeEnum { ChaseMe, StayAway, Heal, LestFly, TripleShooter }


public class PowerUpManager : Simpleton<PowerUpManager>
{
    public PowerUp[] powerUps;

    [SerializeField] PowerUpPanel[] powerUpPanels;


    Coroutine changeZombyTargetCoRoutine;
    Coroutine increasePunchPower;
    Coroutine increaseMoveSpeed;
    Coroutine activateMultiShoot;


    public void PopUpPowerUp(Vector3 position)
    {
        if (Random.Range(0, 100) > 70)
        {
            Instantiate(powerUps[Random.Range(0, powerUps.Length)], position, Quaternion.identity, transform);
        }
    }


    public void ActivatePowerUP(PowerUpTypeEnum type, PlayerController player)
    {
        switch (type)
        {
            case PowerUpTypeEnum.ChaseMe:
                powerUpPanels[1].gameObject.SetActive(true);
                powerUpPanels[1].SetTimer(17);
                if (changeZombyTargetCoRoutine != null) StopCoroutine(changeZombyTargetCoRoutine);
                changeZombyTargetCoRoutine = StartCoroutine(ChangeZombyTargetCoRoutine(player.transform, 17));
                break;
            case PowerUpTypeEnum.StayAway:
                if (increasePunchPower != null) StopCoroutine(increasePunchPower);
                increasePunchPower = StartCoroutine(IncreasePunchPower(player, 30, 25));
                break;
            case PowerUpTypeEnum.Heal:
                IncreaseHealth(player, 1);
                break;
            case PowerUpTypeEnum.LestFly:
                powerUpPanels[2].gameObject.SetActive(true);
                powerUpPanels[2].SetTimer(30);
                if (increaseMoveSpeed != null) StopCoroutine(increaseMoveSpeed);
                increaseMoveSpeed = StartCoroutine(IncreaseMoveSpeed(player, 30));
                break;
            case PowerUpTypeEnum.TripleShooter:
                powerUpPanels[0].gameObject.SetActive(true);
                powerUpPanels[0].SetTimer(20);
                if (activateMultiShoot != null) StopCoroutine(activateMultiShoot);
                activateMultiShoot = StartCoroutine(ActivateMultiShoot(player, 20));
                break;
            default:
                break;
        }
    }


    IEnumerator ActivateMultiShoot(PlayerController player, float duration)
    {
        player.isMultiShootActive = true;
        yield return new WaitForSeconds(duration);
        player.isMultiShootActive = false;
    }


    IEnumerator IncreaseMoveSpeed(PlayerController player, float duration)
    {
        player.ChangeMoveSpeed(0.3f);
        yield return new WaitForSeconds(duration);
        player.ChangeMoveSpeed(0.15f);
    }



    void IncreaseHealth(PlayerController player, int healAmounth)
    {
        player.ReceiveDamage(-healAmounth);
    }



    IEnumerator IncreasePunchPower(PlayerController player, float duration, float punchPower)
    {
        player.punchPower = punchPower;
        yield return new WaitForSeconds(duration);
        player.punchPower = 2;
    }



    IEnumerator ChangeZombyTargetCoRoutine(Transform newTarget, float duration)
    {
        Transform target = GameManager.Instance.tower.transform;

        TowerController.Instance.canLoseHp = false;
        GameManager.Instance.zombyTarget = newTarget;
        foreach (ZombyController zomby in GameManager.Instance.zombies)
        {
            zomby.target = newTarget;
        }

        yield return new WaitForSeconds(duration);

        TowerController.Instance.canLoseHp = true;
        GameManager.Instance.zombyTarget = target;
        foreach (ZombyController zomby in GameManager.Instance.zombies)
        {
            zomby.target = target;
        }
    }
}