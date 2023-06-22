using UnityEngine;

public class ItensController : MonoBehaviour
{
    public GameObject itenBorder;
    public string itenName;
    public static ItensController itenInstance;
    public void Awake(){
        itenInstance=this;
    }

    public void SetItemStatus(string _itemName, bool _status){
        if(_itemName == itenName){
            itenBorder.SetActive(_status);
        }
    }
}
