using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Building : Interactables
{
    [SerializeField] private Image healthBar;
    CanvasGroup infotainmentCanvas;
    [HideInInspector] public int maxHealth;
    public virtual void Awake()
    {
        maxHealth = healthPoints;                       // awakede max health'i storelayinca 2 degiskene gerek kalmadi (max health & currenthealth)
        infotainmentCanvas = GetComponentInChildren<CanvasGroup>();
    }

    void Update()
    {  
        if (InteractableIsCloseToMouse(0.5f))           //mouse ve parent (Interactable) scriptteki static degiskenler ile mouse bu binanin uzerinde mi & ustune tikladi mi kontrolleri
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectionManager.Instance.selectedBuildingOnMap = this.transform;
                StartCoroutine(infotainmentFade());
            }
        }
        if (SelectionManager.Instance.selectedBuildingOnMap != this.transform)
        {
            infotainmentCanvas.gameObject.SetActive(false);
            infotainmentCanvas.alpha = 0;
        }

        healthBar.fillAmount = GetHealthNormalized();       // healthbar UI icin guncel health / max health orani hesaplama
        if (healthPoints <= 0)
            gameObject.SetActive(false);
    }
    private float GetHealthNormalized()
    {
        return (float) healthPoints / maxHealth;
    }

    IEnumerator infotainmentFade()                          // Production menu fade fx
    {
        infotainmentCanvas.gameObject.SetActive(true);
        while (infotainmentCanvas.alpha <= 1)
        {
            infotainmentCanvas.alpha = Mathf.Lerp(infotainmentCanvas.alpha, 1, Time.deltaTime);
            yield return null;
        }
    }
}



