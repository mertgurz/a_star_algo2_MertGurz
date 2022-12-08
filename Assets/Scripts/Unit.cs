using System.Collections;
using UnityEngine;

public class Unit : Interactables
{
    float moveSpeed;
    Vector3[] unitsPath;
    Vector3 randomPointNearTarget;
    int targetIndex;
    [HideInInspector] public bool isAttacking;

    private GameObject attackSignAndSound;
    [SerializeField] private GameObject attackSignAndSoundPrefab;
    [SerializeField] private SpriteRenderer selectedSign;
    public enum SoldierType { SoldierScout, Soldier, SoldierHeavy };       // Soldier varyasyonlarini Awake()'de (ve editorde) baslatmak icin enum
    public SoldierType soldierType;

    private void Awake()
    {
        switch (soldierType)
        {
            case SoldierType.SoldierScout:
                damagePerSecond = 2;
                healthPoints = 10;
                moveSpeed = 23;
                break;
            case SoldierType.Soldier:
                damagePerSecond = 5;
                healthPoints = 10;
                moveSpeed = 20;

                break;
            case SoldierType.SoldierHeavy:
                damagePerSecond = 10;
                healthPoints = 10;
                moveSpeed = 16;

                break;
            default:
                damagePerSecond = 2;
                healthPoints = 10;
                moveSpeed = 25;
                break;
        }
        SelectionManager.Instance.AvailableUnits.Add(this);             // Unit'i awakede SelectionManager.AvailableUnits listine ekliyoruz

        attackSignAndSound = Instantiate(attackSignAndSoundPrefab, transform.position, Quaternion.Euler(90, 0, 0), transform);      // ses + flash efekti prefabini burada instantiate edip, gerekince Attack() methodunda setActive false & true
    }
    private void Update()
    {
        DealtDamage();

        if (SelectionManager.Instance.SelectedUnits.Contains(this))
        {
            if (MousePosition.mouseOverInteractable && Input.GetMouseButtonDown(1))
            {
                StartCoroutine(Attack());
                randomPointNearTarget = MousePosition.mouseOverInteractablesObj.transform.position + Random.onUnitSphere * 18;      // mouse scriptinin kaydettigi pathfinding hedef noktalarina + Random.onUnitSphere ekleyerek varis noktasina randomness ekliyoruz (+uniteler ust uste binmiyor)
                PathRequestManager.RequestPath(transform.position, randomPointNearTarget, OnPathFound);
            }
            else if (!MousePosition.mouseOverInteractable && Input.GetMouseButtonDown(1))
            {
                randomPointNearTarget = MousePosition.mouseRayHitPoint + Random.onUnitSphere * 4;
                PathRequestManager.RequestPath(transform.position, randomPointNearTarget, OnPathFound);
            }
        }
    }
    public void OnSelected()
    {
        selectedSign.gameObject.SetActive(true);
    }
    public void OnDeSelected()
    {
        selectedSign.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        SelectionManager.Instance.SelectedUnits.Remove(this);                           // unit olunce(setactive(false) bulundugu listelerden cikmazsa barrack respawnlari bozuluyor
        ObjectPooling._instance.pooledSoldierLevel_0.Remove(this);
        ObjectPooling._instance.pooledSoldierLevel_1.Remove(this);
        ObjectPooling._instance.pooledSoldierLevel_2.Remove(this);

    }
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful && gameObject.activeInHierarchy)
        {
            targetIndex = 0;
            unitsPath = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }
    IEnumerator Attack()
    {
        Vector3 currentDir = (MousePosition.mouseOverInteractablesObj.transform.position - attackSignAndSound.transform.position).normalized;
        attackSignAndSound.transform.position += currentDir;           // efektin SetActive olacagi yer icin hedef direction hesapliyor (ama buggy hizlica donemiyor - duzelt)

        if (InteractableIsCloseToMouse(25f) && !isAttacking)
        {
            isAttacking = true;
            attackSignAndSound.SetActive(true);
            yield return new WaitForSeconds(1.4f);          // SFX ile de uyumlu bir delay suresi
            attackSignAndSound.SetActive(false);
            isAttacking = false;
        }
    }
    IEnumerator FollowPath()
    {
        Vector3 currentWayPoint = unitsPath[0];             // bazen pes pese mouse(1) clicklerde "index outside of array bounds" hatasi var ama game breaking degil 

        while (currentWayPoint != null)
        {
            if (transform.position == currentWayPoint) {
                targetIndex++;
                if (targetIndex >= unitsPath.Length) {
                    targetIndex = 0;
                    unitsPath = new Vector3[0];
                    yield break;                            // unit hedefe ulasti cunku targetindex >= path length
                }
                currentWayPoint = unitsPath[targetIndex];   // eger hala hedefe ulasamadiysak arrayda siradaki Vector3u aliyoruz
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
