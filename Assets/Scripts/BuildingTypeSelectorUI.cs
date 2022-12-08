using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingTypeSelectorUI : MonoBehaviour
{
    [SerializeField] private List<BuildingTypeScriptableObj> buildingTypeSOList;
    [SerializeField] private Transform[] buildingBtnTransforms;
    [SerializeField] private StructureBuilder structureBuilderScript;

    // UI button ve ScriptableObj'leri koordine etmek icin Dictionary 
    private Dictionary<Transform, BuildingTypeScriptableObj> buildingBtnDictionary;

    void Awake()
    {
        buildingBtnDictionary = new Dictionary<Transform, BuildingTypeScriptableObj>();

        int index = 0;
        foreach (Transform transform in buildingBtnTransforms)
        {         
            buildingBtnDictionary[transform] = buildingTypeSOList[index];       // her buttonin dictionary key ve degeri hazir, OnBuildingButtonPressed()'de key'lerde cycle yapilacak
            index++;
            }
    }
    public void OnBuildingButtonPressed()
    {
        foreach (Transform obj in buildingBtnTransforms)
        {
            while (EventSystem.current.currentSelectedGameObject.name == obj.name)              // bastigimiz UI button ismiyle buildingBtnTransforms array'deki birimin ismi eslesince..
            {                                                                                           // builder scripte bu scriptableObj bilgisini aktariyorum
                structureBuilderScript.SetActiveBuildingType(buildingBtnDictionary[obj]);        
                break;
            }
        }    
    }   
}
