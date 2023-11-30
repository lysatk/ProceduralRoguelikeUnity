using UnityEngine;

public class ManekinLogic : MonoBehaviour
{
    public string description;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HeroUnitBase unit))
        {
            unit.SetNameToChangeMage(name);
            TogglePopUp(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HeroUnitBase unit))
        {
            unit.SetNameToChangeMage(null);
            TogglePopUp(false);
        }
    }
    private void TogglePopUp(bool show)
    {
        
        if (show)
        {
            
            Debug.Log("Description: " + description);
        }
        else
        {
            
        }
    }
}