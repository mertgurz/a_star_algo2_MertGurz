using UnityEngine;

public class Interactables : MonoBehaviour
{
    public int healthPoints;
    [HideInInspector] public int damagePerSecond;
    [HideInInspector] public float damageCooldownTimer;

    public bool InteractableIsCloseToMouse(float distanceToCheck)
    {
        if (MousePosition.mouseOverInteractablesObj != null && Vector3.Distance(transform.position, MousePosition.mouseOverInteractablesObj.transform.position) < distanceToCheck)
        {
            return true;
        }
        return false;
    }
    public void DealtDamage()        
    {
        foreach (Unit unit in SelectionManager.Instance.SelectedUnits)
        {
            if (unit.isAttacking && MousePosition.mouseOverInteractable  && Input.GetMouseButtonDown(1) && unit.damageCooldownTimer <= 0.1f)
            {
                MousePosition.mouseOverInteractablesObj.healthPoints -= damagePerSecond;
                unit.damageCooldownTimer = 0.5f;   
            }

            if (Input.GetMouseButtonUp(1))
                unit.damageCooldownTimer = 0.5f;            //  alinan damage'i pes pese clicklerle bozmamak icin cooldown timer

            if (unit.damageCooldownTimer >= 0)
                unit.damageCooldownTimer -= Time.deltaTime;

        }
        if (healthPoints <= 0)
            gameObject.SetActive(false);
    }
}


