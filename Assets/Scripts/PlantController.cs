using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    PlantSO currentPlant;

    [SerializeField] GameObject[] plantStages;
    [SerializeField] List<GameObject> plantObjects;
    [SerializeField] float[] stageDurationList;
    public bool isPlanted;

    [SerializeField] float currentStageDuration;
    [SerializeField] float currentStageTimer;
    [SerializeField] int stageIndex;
    [SerializeField] int stageCount;
    [SerializeField] bool isReadyToHarvest;

    void SetPlantInfo()
    {
        plantStages = currentPlant.PlantStages;
        stageCount = plantStages.Length;
        stageDurationList = currentPlant.StageDurations;

        stageIndex = 0;
        currentStageTimer = stageDurationList[stageIndex];

    }

    public bool PlantSeed(PlantSO _plant)
    {
        if (isPlanted)
        {
            return false;
        }
        currentPlant = _plant;
        isPlanted = true;

        SetPlantInfo();
        SetPlantObjects();
        ActivatePlant();

        return true;
    }

    void SetPlantObjects()
    {
        foreach (GameObject plant in currentPlant.PlantStages) 
        {
            GameObject _thisPlant = Instantiate(plant, transform.position, transform.rotation);
            _thisPlant.transform.SetParent(transform);
            plantObjects.Add(_thisPlant);
            _thisPlant.SetActive(false);
        }
    }


    void ActivatePlant() 
    {
        foreach (GameObject plant in plantObjects)
        {
            plant.SetActive(false);
        }
        plantObjects[stageIndex].SetActive(true);
    }

    void StageCountDown()
    {
        currentStageTimer -= Time.deltaTime;
        if (currentStageTimer <= 0)
        {
            stageIndex++;
            if (stageIndex >= stageCount - 1)
            {
                isReadyToHarvest = true;
            }
            currentStageTimer = stageDurationList[stageIndex];
            ActivatePlant();
        }
    }

    private void Update()
    {
        if (isPlanted && !isReadyToHarvest) 
        {
            StageCountDown();
        }
    }

}
