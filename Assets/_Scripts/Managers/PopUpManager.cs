using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class PopUpManager : MonoBehaviour
{
  public List<PopUpModel>  PopUpModels = new List<PopUpModel>();


    public void ShowPopUp(int popUpId)//, float xPos,float yPos)
    {
        PopUpModel popUpModel =PopUpModels.FirstOrDefault(popUp=>popUp.popUpId == popUpId);
        if (popUpModel==null)
        {
            Debug.LogWarning("Tried using a popUpId without a respective popUp");
            return; 
        }
        else
        {
            Instantiate(popUpModel.popUpObject);
        }
    }
 }

public class PopUpModel
{
    public int popUpId;
    public GameObject popUpObject;

}