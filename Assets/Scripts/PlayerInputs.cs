using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] private RectTransform SelectionBox;
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask layermaskUnit;

    private Vector2 mouseStartPos;
    private float dragDelay = 0.1f;
    private float mouseCooldown;

    private void Update()
    {
        HandleSelectorInputs();
    }
    private void HandleSelectorInputs()
    {
        if (Input.GetMouseButtonDown(0) && !MousePosition.mouseOutOfMap)
        {
            SelectionBox.sizeDelta = Vector2.zero;      // onceki box selectten kalanlari yok edip yenisini baslatiyoruz
            SelectionBox.gameObject.SetActive(true);
            mouseStartPos= Input.mousePosition;
            mouseCooldown = Time.time;
        }
        else if(Input.GetMouseButton(0) && mouseCooldown + dragDelay < Time.time)                // user mouse sol clicke basili tutuyor(GetMouseButton) // basmaya basladigi andaki time + drag ile sadece time karsilastirmasi sayesinde direct kisa sureli click'e gecebiliriz 
        {
            ResizeSelectionBox();
        }
        else if (Input.GetMouseButtonUp(0))                // bir onceki blogu atlayip buraya geldik cunku kisa sureli klik ve birak (GetMouseButtonUp)

        {
            SelectionBox.sizeDelta = Vector2.zero;          // user released mouse 0 button dolayisiyla selectionbox iptal
            SelectionBox.gameObject.SetActive(false);
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition),out RaycastHit rayHit, layermaskUnit) && rayHit.collider.TryGetComponent(out Unit unit))  //Kameradan cikan ray bir "layermaskUnite" vurdu mu & bu hedefin "unit" script componenti var mi 
            {
                SelectionManager.Instance.DeSelectAll();
                SelectionManager.Instance.Select(unit);         // 1 unite secmek icin 
            }
            else if (mouseCooldown + dragDelay > Time.time)         // unit harici bir yere tiklayinca DeSelectAll() yapmak icin
                SelectionManager.Instance.DeSelectAll();

            mouseCooldown = 0;
        }

    }
    private void ResizeSelectionBox()
    {
        float width = Input.mousePosition.x - mouseStartPos.x;
        float height = Input.mousePosition.y - mouseStartPos.y;

        SelectionBox.anchoredPosition = mouseStartPos + new Vector2(width / 2, height / 2);     // Rect'in anchoru ortalamak icin mouseStartPos + kutunun yarisini ekliyoruz (yani + width / 2, height / 2 )
        SelectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

        Bounds bounds = new Bounds(SelectionBox.anchoredPosition, SelectionBox.sizeDelta);      // Selection karesinin kenarlarini belirlemek icin boundsa karenin merkezini ve boyutunu aktariyoruz 

        for (int i = 0; i < SelectionManager.Instance.AvailableUnits.Count; i++)
        {
            if (UnitIsInSelectionArea(mainCam.WorldToScreenPoint(SelectionManager.Instance.AvailableUnits[i].transform.position), bounds))      
            {
                SelectionManager.Instance.Select(SelectionManager.Instance.AvailableUnits[i]);
            } else {
                SelectionManager.Instance.DeSelect(SelectionManager.Instance.AvailableUnits[i]);
            }
        }
    }

    private bool UnitIsInSelectionArea(Vector3 _position, Bounds _bounds)               //  AvailableUnits'in vector3 pozisyonu bounds'un icinde kaliyor mu hesaplamasi
    {
        return (_position.x > _bounds.min.x && _position.x < _bounds.max.x)         
         && (_position.y > _bounds.min.y && _position.y < _bounds.max.y);
    }
}
