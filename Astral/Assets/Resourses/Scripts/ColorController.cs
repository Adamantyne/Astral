using UnityEngine;
using UnityEngine.UI;

public class ColorController : MonoBehaviour
{
    public static ColorController ColorInstance;
    [SerializeField] private Button btn;
    [SerializeField] public Color standardColor;
    [SerializeField] public Color selectedColor;

    private void Awake(){
        ColorInstance = this;
        SetImageColor(standardColor);
    }

    public void SetImageColor(Color _color){
        btn.image.color = _color;
    }


}
