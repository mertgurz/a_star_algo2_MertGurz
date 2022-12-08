using UnityEngine;
using System.Collections.Generic;
public class Barracks : Building
{
    public AudioClip audioClip;
    public override void Awake()            // override cunku inheritledigimiz parent classlarda ses efekti yok
    {
        base.Awake();
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
    }
    public void ProduceUnits(Unit soldier)                                // UI buttonlari ile uretilecek soldier tiplerini secebildigimiz method
    {
        switch (soldier.soldierType)
        {
            case Unit.SoldierType.SoldierScout:
                
                Unit instantiatedObj_0 = ObjectPooling._instance.GetObjectInPool(ObjectPooling._instance.pooledSoldierLevel_0);
                if (instantiatedObj_0 == null)
                    return;

                    PickUnits(ObjectPooling._instance.pooledSoldierLevel_0, instantiatedObj_0);
                break;

            case Unit.SoldierType.Soldier:
                Unit instantiatedObj_1 = ObjectPooling._instance.GetObjectInPool(ObjectPooling._instance.pooledSoldierLevel_1);
                if (instantiatedObj_1 == null)
                    return;

                PickUnits(ObjectPooling._instance.pooledSoldierLevel_1, instantiatedObj_1);
                break;

            case Unit.SoldierType.SoldierHeavy:
                Unit instantiatedObj_2 = ObjectPooling._instance.GetObjectInPool(ObjectPooling._instance.pooledSoldierLevel_2);
                if (instantiatedObj_2 == null)
                    return;

                PickUnits(ObjectPooling._instance.pooledSoldierLevel_2, instantiatedObj_2);
                break;
        }
    }
    private void PickUnits(List<Unit> list, Unit unit) 
    {
        for (int i = 0; i < list.Count; i++)
        {
            unit.transform.position = new Vector3(transform.position.x + Random.insideUnitSphere.x * 20, 0, transform.position.z + Random.insideUnitSphere.z * 20);
            unit.gameObject.SetActive(true);
        }
    }
}
