using UnityEngine;
using UnityEngine.UI;


public enum IndicatorColorEnum { zomby, zombyDog };


public class IndicatorBehavior : MonoBehaviour
{
    [HideInInspector] public Transform target;
    [Tooltip("indicatorun görünür olmaya baþlayacaðý mesafe")]
    [SerializeField] float distanceToTarget;
    /*[SerializeField]*/
    RectTransform rectTransform;
    [SerializeField] Image image;
    [SerializeField] Color indicatorColorForZomby;
    [SerializeField] Color indicatorColorForZombyDog;


    [HideInInspector] public Transform objToIndicate;

    Color indicatorColor;
    Canvas canvas;
    Camera mainCamera;



    [HideInInspector] public IndicatorColorEnum indColor;



    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        canvas = transform.parent.GetComponent<Canvas>();

        mainCamera = Camera.main;

        switch (indColor)
        {
            case IndicatorColorEnum.zomby:
                indicatorColor = indicatorColorForZomby;
                break;
            case IndicatorColorEnum.zombyDog:
                indicatorColor = indicatorColorForZombyDog;
                break;
            default:
                indicatorColor = Color.white;
                break;
        }
    }

    private void Update()
    {
        UpdateIndicator();
    }


    void UpdateIndicator()
    {
        float y;
        float x;
        float height = canvas.pixelRect.height;
        float width = canvas.pixelRect.width;

        y = mainCamera.WorldToScreenPoint(objToIndicate.transform.position).y - height / 2;
        x = mainCamera.WorldToScreenPoint(objToIndicate.transform.position).x - width / 2;

        if (y + height / 2 > height || x + width / 2 > width || y + height / 2 < 0 || x + width / 2 < 0)
        {
            y = Mathf.Clamp(y, -(height / 2 - rectTransform.rect.height / 2), height / 2 - rectTransform.rect.height / 2);
            x = Mathf.Clamp(x, -(width / 2 - rectTransform.rect.width / 2), width / 2 - rectTransform.rect.width / 2);

            Vector3 objPos = new Vector3(x, y, 0);

            transform.localPosition = objPos;

            float distance = Vector2.Distance(new Vector2(target.position.x, target.position.z), new Vector2(objToIndicate.position.x, objToIndicate.position.z));

            indicatorColor.a = 1 - (distance + 1) / distanceToTarget;
            image.color = indicatorColor;
        }
        else
        {
            indicatorColor.a = 0;
            image.color = indicatorColor;
        }
    }
}