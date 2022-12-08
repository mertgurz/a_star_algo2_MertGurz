using System.Collections.Generic;
using UnityEngine;


public class SelectionManager   
{
    private static SelectionManager _instance;
    public static SelectionManager Instance         //  Singleton pattern
    {
        get {
            if (_instance ==null) 
                _instance = new SelectionManager();

            return _instance;
        }
        private set {
            _instance = value;
        }
    }
    public HashSet<Unit> SelectedUnits = new HashSet<Unit>();
    public List<Unit> AvailableUnits = new List<Unit>();
    public Transform selectedBuildingOnMap;
    
    // Utility amacli yardimci methodlar
    public void Select(Unit _unit)
    {
        SelectedUnits.Add(_unit);
        _unit.OnSelected();             // list'e ekle + unit birimindeki onSelected'i baslat
    }
    public void DeSelect(Unit _unit)
    {
        _unit.OnDeSelected();
        SelectedUnits.Remove(_unit);
    }
    public void DeSelectAll()
    {
        foreach (Unit _unit in SelectedUnits)
        {
            _unit.OnDeSelected();
        }
        SelectedUnits.Clear();
    }
    public bool IsSelected (Unit _unit)
    {
        return SelectedUnits.Contains(_unit);
    }
}
