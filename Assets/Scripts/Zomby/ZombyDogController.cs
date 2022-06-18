using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ZombyDogController : ZombyController
{
    [SerializeField] SkinnedMeshRenderer skinnedMesh;
    Material dissolveMaterial;
    float dissolve = 0;

    private void Start()
    {
        SetStats(7, 2, 16, new WaitForSeconds(attackSpeed), true, true);

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        animator = GetComponent<Animator>();

        dissolveMaterial = skinnedMesh.material;
    }

    protected override void DeathAnimEvent()
    {
        StartCoroutine(DissolveObj());
    }

    IEnumerator DissolveObj()
    {
        dissolve += Time.deltaTime / 2;
        dissolveMaterial.SetFloat("_Dissolve", dissolve);
        yield return new WaitForEndOfFrame();
        if (dissolve > 1)
        {
            base.DeathAnimEvent();
        }
        StartCoroutine(DissolveObj());
    }
}