using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling _instance { get; private set; }
    
    public Unit[] unitTypes;
    public int pooledAmount;
    public List<Unit> pooledSoldierLevel_0, pooledSoldierLevel_1, pooledSoldierLevel_2; 

    void Awake()
    {
        #region Singleton
        if (_instance != null && _instance != this)
            Destroy(this);
        else
            _instance = this;
        #endregion

        for (int i = 0; i < pooledAmount;  i++)
        {
            Unit _obj0 = Instantiate(unitTypes[0], transform);           // Awake'de objeleri instantiate & add 
            _obj0.gameObject.SetActive(false);
            pooledSoldierLevel_0.Add(_obj0);

            Unit _obj1 = Instantiate(unitTypes[1], transform);          
            _obj1.gameObject.SetActive(false);
            pooledSoldierLevel_1.Add(_obj1);

            Unit _obj2 = Instantiate(unitTypes[2], transform);         
            _obj2.gameObject.SetActive(false);
            pooledSoldierLevel_2.Add(_obj2);

        }
    }
    public Unit GetObjectInPool(List<Unit> list)                // liste parametresini girip o listeden obje cekme methodu
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].gameObject.activeInHierarchy)
            {
                return list[i];
            }
        }
        return null;
    }
}
