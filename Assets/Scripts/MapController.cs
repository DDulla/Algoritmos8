using System.Collections;
using UnityEngine;
using Cinemachine;
using System;
using TMPro;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    private InteractiveMap interactiveMap = new InteractiveMap(); 

    [SerializeField] private GameObject indicator; // Indicador visual
    [SerializeField] private GameObject LineRendererPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Cámara
    [SerializeField] private Transform NodeHolder;
    [SerializeField] private UIZone[] levels; // Niveles
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private UIZone currentZone;

    void Start()
    {
        virtualCamera.Follow = indicator.transform;
        virtualCamera.LookAt = indicator.transform;

        currentZone = levels[0];

        foreach (UIZone uiZone in levels)
        {
            if (uiZone == null) continue;

            Zone zone = new Zone(uiZone.name, uiZone.ZoneDescription, uiZone.ZoneKey);

            uiZone.Initialize(zone);
            zone.Initialize(uiZone);

            interactiveMap.AddZone(zone);
        }

        interactiveMap.AddEdge(1, 2);
        interactiveMap.AddEdge(2, 3);
        interactiveMap.AddEdge(3, 4);
        interactiveMap.AddEdge(4, 5);
        interactiveMap.AddEdge(2, 10);
        interactiveMap.AddEdge(10, 2);
        interactiveMap.AddEdge(4, 20);
        interactiveMap.AddEdge(20, 4);


        NodeConnection();


        ReadSelection(currentZone.GetZone()); 
    }

    public void UpdateSelection()
    {
        StartCoroutine(MoveIndicatorSmoothly(currentZone.transform.position));
        ReadSelection(currentZone.GetZone());
    }

    private IEnumerator MoveIndicatorSmoothly(Vector3 targetPosition)
    {
        Vector3 startPosition = indicator.transform.position;
        targetPosition += new Vector3(0, 1, 0); // Ajuste de altura

        Vector3 direction = (targetPosition - startPosition).normalized;

        float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        indicator.transform.rotation = Quaternion.Euler(180, 0, -targetAngle);

        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            indicator.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        indicator.transform.position = targetPosition;
    }
    public void NodeConnection()
    {
        foreach (var node in interactiveMap.Nodes)
        {
            foreach (var nodeNeighbor in node.Value.neighbors)
            {
                UIZone nodeObject = node.Value.Key.GetUIZone();
                UIZone neighborObject = nodeNeighbor.Key.GetUIZone();

                GameObject LineR = Instantiate(LineRendererPrefab, NodeHolder);
                LineRenderer lr = LineR.GetComponent<LineRenderer>();

                lr.positionCount = 2;
                lr.SetPosition(0, nodeObject.transform.position);
                lr.SetPosition(1, neighborObject.transform.position);
            }
        }
    }

    public void ReadSelection(Zone zone)
    {
        levelText.text = "Nivel seleccionado: " + zone.ZoneName;
        descriptionText.text = "Descripción: " + zone.ZoneDescription;
    }
    /*
    public void UpdateUnlockedZones()
    {
        if (interactiveMap.Nodes.TryGetValue(currentZone.ZoneKey, out var currentNode))
        {
            foreach (var neighbor in currentNode.neighbors)
            {
                int neighborKey = neighbor.Key.ZoneKey;
                if (zoneLookup.TryGetValue(neighborKey, out UIZone neighborUIZone))
                {
                    neighborUIZone.SetUnlocked(true);
                }
            }
        }

        if (zoneLookup.TryGetValue(currentZone.ZoneKey, out UIZone currentUIZone))
        {
            currentUIZone.SetUnlocked(true);
        }
    }
    */
}