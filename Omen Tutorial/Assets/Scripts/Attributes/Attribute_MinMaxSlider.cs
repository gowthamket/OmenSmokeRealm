using UnityEngine;

public class Attribute_MinMaxSlider : PropertyAttribute
{
    public float min;
    public float max;

    public Attribute_MinMaxSlider(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}  
