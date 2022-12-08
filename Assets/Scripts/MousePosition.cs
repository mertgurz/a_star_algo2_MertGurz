using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private LayerMask layermaskGround, layermaskUnwalkable;
    
    public static Vector3 mouseRayHitPoint;
    public static Interactables mouseOverInteractablesObj;

    public static bool mouseOverInteractable;
    public static bool mouseOutOfMap;

    public void Update()
    {
        Ray _ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out RaycastHit rayHitGround, layermaskGround))
            mouseRayHitPoint = rayHitGround.point;

        if (Physics.Raycast(_ray, out RaycastHit rayHitUnwalkable, layermaskUnwalkable))   // Mouse game board'da unwalkable layerda mi geziyor? & Geziyorsa Ray'ler interactable objelere(orn: unit, building) mi carpti? 
        {
            if (rayHitUnwalkable.transform.TryGetComponent(out Interactables _interactables))
            {
                mouseOverInteractable = true;
                mouseOverInteractablesObj = _interactables;        // interactable obje; soldier(Unit script) gidip saldirirken kullaniliyor
            }
            else { mouseOverInteractable = false;
                mouseOverInteractablesObj = null;                   // bu line olmazsa grableyip eslestirdigi son interactable variable'da takili kaliyor
            }

            if (rayHitUnwalkable.transform.CompareTag("OutOfMap"))
                mouseOutOfMap = true;
            else
                mouseOutOfMap = false;
        
        }
    }
}
