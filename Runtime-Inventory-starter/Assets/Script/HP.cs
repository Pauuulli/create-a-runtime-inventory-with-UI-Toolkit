using UnityEngine.UIElements;

public class HP : VisualElement
{
    public HP()
    {
        this.name = "HPValue";
        AddToClassList("HP-value");
    }

    public void setHP(int percent)
    {
       this.style.width = Length.Percent(percent);
    }
}
