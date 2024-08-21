using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlantDetector : MonoBehaviour
{
    AudioSource m_AudioSource;
    Transform cemaraTransform;
    [SerializeField] ImageRendererController ImageController;
    [Header("Plants List")]
    [SerializeField] TextMeshProUGUI plantText;
    [SerializeField] PlantSO[] plantList;
    [SerializeField] PlantSO currentPlant;
    [SerializeField] int plantCount;
    [SerializeField] int plantIndex;

    [SerializeField] GameObject areaIndicatorDecal;
    [SerializeField] Transform selectedPlantArea;
    [SerializeField] Vector3 decalOffset = new Vector3(0f, 0.3f, 0f);

    [Header("Ray Casting")]
    [SerializeField] LayerMask plantAreaLayer;
    [SerializeField] float rayDistance = 4f;

    [Header("Plant")]
    [SerializeField] GameObject plantPrefab;
    [SerializeField] Vector3 plantOffset = new Vector3(0f, 0.12f, 0f);
    [SerializeField] AudioClip planingAudio;

    [Header("Smoothing")]
    [SerializeField] Vector3 targetPos;
    [SerializeField] float smoothingCooldown = 0.3f;
    [SerializeField] float smoothingTimer;
    [SerializeField] bool willSmooth;
    [SerializeField] float smoothSpeed;
    bool isExited;
    bool isSmoothing;
    private void Awake()
    {
        cemaraTransform = Camera.main.transform;
        m_AudioSource = GetComponent<AudioSource>();

    }

    private void Start()
    {
        plantCount = plantList.Length;
        plantIndex = 0;
        //ImageController.SetListObjects(plantList);
        SwitchPlant(0);
        
    }

    void SwitchPlant(int _delta)
    {
        plantIndex += _delta;
        if (plantIndex < 0)
        {
            plantIndex = plantCount - 1;
        }
        else if (plantIndex >= plantCount)
        {
            plantIndex = 0;
        }
        currentPlant = plantList[plantIndex];
        ImageController.ActivateImage(currentPlant);
        plantText.SetText(currentPlant.PlantName.ToString());
    }

    void CheckPlantArea()
    {
        Ray ray = new Ray(cemaraTransform.position, cemaraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit _hitInfo, rayDistance, plantAreaLayer))
        {

            if (!selectedPlantArea)
            {
                selectedPlantArea = _hitInfo.transform;

                areaIndicatorDecal.SetActive(true);
                if (!willSmooth)
                {
                    areaIndicatorDecal.transform.position = selectedPlantArea.position + decalOffset;
                }
                else
                {
                    targetPos = selectedPlantArea.position + decalOffset;
                    isSmoothing = true;
                }
                willSmooth = true;
                smoothingTimer = smoothingCooldown;
                isExited = false;
            }

            else if (selectedPlantArea != _hitInfo.transform)
            {
                selectedPlantArea = _hitInfo.transform;
                areaIndicatorDecal.SetActive(true);

                targetPos = selectedPlantArea.position + decalOffset;
                isSmoothing = true;

                willSmooth = true;
                smoothingTimer = smoothingCooldown;
                isExited = false;
            }
        }
        else
        {
            if (!isExited)
            {
                isExited = true;
                selectedPlantArea = null;
                areaIndicatorDecal.SetActive(false);
                willSmooth = true;
                smoothingTimer = smoothingCooldown;
            }
        }

    }

    void SmoothingTimer()
    {
        if (willSmooth)
        {
            smoothingTimer -= Time.deltaTime;
            if (smoothingTimer <= 0f)
            {
                willSmooth = false;
            }
        }
    }

    void SmoothTheIndicator()
    {
        if (isSmoothing && selectedPlantArea)
        {
            float speed = Mathf.Clamp(Vector3.Distance(areaIndicatorDecal.transform.position, targetPos), 1, 30f) * smoothSpeed;

            areaIndicatorDecal.transform.position = Vector3.MoveTowards(areaIndicatorDecal.transform.position, targetPos, speed * Time.deltaTime);
            if (areaIndicatorDecal.transform.position == targetPos)
            {
                isSmoothing = false;
            }
        }
    }

    void PlantSeed()
    {

        if (selectedPlantArea)
        {
            PlantController _thisController = selectedPlantArea.GetComponent<PlantController>();
            if (_thisController.PlantSeed(currentPlant))
            {
                m_AudioSource.PlayOneShot(planingAudio);
            }
            
            
        }
    }


    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlantSeed();
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            SwitchPlant((int)Input.mouseScrollDelta.y);
        }
    }

    private void Update()
    {
        CheckPlantArea();
        SmoothingTimer();
        SmoothTheIndicator();
        GetInput();
     

    }
}
