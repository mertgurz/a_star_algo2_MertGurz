using UnityEngine;
using UnityEngine.EventSystems;

public class StructureBuilder : MonoBehaviour
{
    public Grid gridScript;
    public LayerMask layerMask;
    public GameObject buildingDeniedPrefab;

    public static bool mapHasChanged;
    [SerializeField] private BuildingTypeScriptableObj activeBuildingType;          // insa edilecek bina prefabini SO'lar icinden secmek icin variable

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())      // (IsPointerOverGameObject) UI objelerine clickleyince bina insa olmasini engelliyor
        {
            if (CanConstructBuilding(activeBuildingType, MousePosition.mouseRayHitPoint) && activeBuildingType!=null)
            {
                Instantiate(activeBuildingType._prefab, MousePosition.mouseRayHitPoint, Quaternion.identity);
                mapHasChanged = true;
                activeBuildingType = null;                                  //  bir kez binayi secince hic araliksiz insa etmek istiyorsak bu set null kaldirilabilir
            }
        }
    }
    public void SetActiveBuildingType(BuildingTypeScriptableObj buildingTypeSO)         //  activebuildingtype degiskenini UI managerdaki Scriptable Obj tercihine gore degistiriyoruz
    {
        activeBuildingType = buildingTypeSO;
    }
    private bool CanConstructBuilding(BuildingTypeScriptableObj buildingTypeSO, Vector3 _position)
    {
        if (Physics.CheckSphere(_position, 2, layerMask) && activeBuildingType != null) {
            GameObject deniedFXobj = Instantiate(buildingDeniedPrefab, MousePosition.mouseRayHitPoint, Quaternion.Euler(270,0,0));      // sfx ve vfx olan prefabi aktive et, grab & destroy
            Destroy(deniedFXobj, 1f);
            return false;
            } else { 
                return true;
        }
    } 
}
