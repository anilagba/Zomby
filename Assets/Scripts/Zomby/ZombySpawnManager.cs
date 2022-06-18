using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class ZombySpawnManager : Simpleton<ZombySpawnManager>
{
    [Min(3)][SerializeField] float maxSpawnDelay = 3f;
    [Min(1)][SerializeField] float minSpawnDelay = 1f;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] ZombyController[] zombyPrefabs;
    [SerializeField] IndicatorBehavior indicatorPrefab;
    Canvas indicatorParentCanvas;

    WaitForSeconds spawnTimer;



    private void Start()
    {
        indicatorParentCanvas = GameObject.Find("IndicatorParentCanvas").GetComponent<Canvas>();
        StartCoroutine(CreateRandomZomby());
    }




    IEnumerator CreateRandomZomby()
    {
        spawnTimer = new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

        float random = Random.Range(0, 100);

        if (random > 20)
        {
            CreateNewZomby(zombyPrefabs[0], GameManager.Instance.zombyTarget, IndicatorColorEnum.zomby);
        }
        else
        {
            CreateNewZomby(zombyPrefabs[1], GameManager.Instance.zombyDogTarget, IndicatorColorEnum.zombyDog);
        }

        yield return spawnTimer;

        StartCoroutine(CreateRandomZomby());
    }


    //TODO zomby pooling
    ZombyController CreateNewZomby(ZombyController zombyPrefab, Transform target, IndicatorColorEnum colorEnum)
    {
        ZombyController newZomby;

        newZomby = Instantiate(zombyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, transform);
        newZomby.target = target;
        newZomby.indicator = CreateNewIndicator(indicatorPrefab, newZomby, target, colorEnum);

        if (!newZomby.GetComponent<ZombyDogController>()) GameManager.Instance.AddNewZombyToList(newZomby);

        return newZomby;
    }


    IndicatorBehavior CreateNewIndicator(IndicatorBehavior indicatorPrefab, ZombyController zombyToIndicate, Transform target, IndicatorColorEnum colorEnum)
    {
        IndicatorBehavior indicator = Instantiate(indicatorPrefab);
        indicator.transform.parent = indicatorParentCanvas.transform;
        indicator.objToIndicate = zombyToIndicate.transform;
        indicator.target = target;
        indicator.indColor = colorEnum;

        return indicator;
    }
}