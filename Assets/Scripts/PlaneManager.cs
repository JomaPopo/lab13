using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class PlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private GameObject model3DPrefab;

    private GameObject model3DPlaced;

    private void Awake()
    {
        if (arPlaneManager == null)
            arPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        if (model3DPlaced != null)
            return;

        foreach (var plane in arPlaneManager.trackables)
        {
            Vector2 size = plane.size;
            if (size.x >= 0.4f && size.y >= 0.4f)
            {
                PlaceModelOnPlane(plane);
                StopPlaneDetection();
                break;
            }
        }
    }

    private void PlaceModelOnPlane(ARPlane plane)
    {
        model3DPlaced = Instantiate(model3DPrefab);
        float yOffset = model3DPlaced.transform.localScale.y / 2f;
        Vector3 placementPosition = plane.transform.position + new Vector3(0, yOffset, 0);

        model3DPlaced.transform.position = placementPosition;
        model3DPlaced.transform.up = plane.transform.up;
    }

    private void StopPlaneDetection()
    {
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }
}
