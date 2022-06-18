using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class TowerController : Simpleton<TowerController>
{
    [SerializeField] int hp;
    [SerializeField] TextMeshProUGUI towerHealthText;
    [SerializeField] Camera birdsEyeViewCam;
    Camera mainCamera;
    Vector3 screenPosition;

    public bool canLoseHp = true;
    const string healthTextString = " = ";


    private void Start()
    {
        mainCamera = Camera.main;
        towerHealthText.text = healthTextString + hp.ToString();
    }

    private void Update()
    {
        ActivateSecondCamera();
    }

    //kule main kameranýn görüþ açýsý dýþýna çýktýðýnda
    //kuþ bakýþý kamera aktif olur
    void ActivateSecondCamera()
    {
        screenPosition = mainCamera.WorldToViewportPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.y < 0 || screenPosition.x > 1 || screenPosition.y > 1)
        {
            birdsEyeViewCam.enabled = true;
        }
        else birdsEyeViewCam.enabled = false;
    }


    public void LoseHP(int damage)
    {
        hp -= damage;

        if (hp < 1)
        {
            hp = 0;
            canLoseHp = false;
        }
        towerHealthText.text = healthTextString + hp.ToString();
    }
}