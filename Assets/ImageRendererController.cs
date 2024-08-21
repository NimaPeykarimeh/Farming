using System;
using System.Collections.Generic;
using UnityEngine;

public class ImageRendererController : MonoBehaviour
{
    public List<PlantObject> plantObjectList;
    [SerializeField] Transform objectParent;
    [SerializeField] float rotationSpeed = 10f;
    [Serializable]
    public class PlantObject
    {
        public GameObject _object;
        public PlantSO plantInfo;
    }

    public void SetListObjects(PlantSO[] plantList)
    {
        foreach (PlantSO _plant in plantList)
        {
            GameObject _obj = Instantiate(_plant.PlantStages[_plant.PlantStages.Length - 1].gameObject,objectParent.position,objectParent.rotation);
            _obj.transform.SetParent(objectParent);

            PlantObject newPlant = new PlantObject();
            newPlant.plantInfo = _plant;
            newPlant._object = _obj;
            plantObjectList.Add(newPlant);
            _obj.layer = 8;
            _obj.SetActive(false);
        }
    }
    private void Update()
    {
        objectParent.transform.Rotate(transform.up,rotationSpeed * Time.deltaTime);
    }
    public void ActivateImage(PlantSO _plant)
    {
        foreach (PlantObject _plantObject in plantObjectList)
        {
            _plantObject._object.SetActive(false);
        }

        foreach (PlantObject _plantObject in plantObjectList) 
        {
            if (_plantObject.plantInfo == _plant)
            {
                _plantObject._object.SetActive(true);
                return;
            }
        }
    }
}
