using UnityEngine;

[CreateAssetMenu(fileName = "PlantSO", menuName = "Scriptable Objects/PlantSO")]
public class PlantSO : ScriptableObject
{
    public GameObject[] PlantStages;
    public Plants PlantName;
    public float[] StageDurations;
    public int SellingPrice;
    public int BuyingPrice;

    public enum Plants
    {
        Beetroot,
        Broccoli,
        Carrot,
        Onion,
        Watermelon,
        Potato,
        Corn,
        Pumpkin
    }
}
