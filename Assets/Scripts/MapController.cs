using System.Collections;
using UnityEngine;
using Cinemachine;
using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class MapController : MonoBehaviour
{
    private InteractiveMap interactiveMap = new InteractiveMap(); 

    [SerializeField] private GameObject indicator; 
    [SerializeField] private GameObject LineRendererPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCamera; 
    [SerializeField] private Transform NodeHolder;
    [SerializeField] private UIZone[] levels;

    #region Text
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    #endregion

    #region Raycast
    [SerializeField] private Camera mainCamera;
    private UIZone currentZone;
    public LayerMask clickableLayers;
    #endregion

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
        interactiveMap.AddEdge(5, 4);
        interactiveMap.AddEdge(2, 10);
        interactiveMap.AddEdge(10, 2);
        interactiveMap.AddEdge(4, 20);
        interactiveMap.AddEdge(20, 4);


        NodeConnection();

        UpdateUnlockedZones();

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
        UpdateUnlockedZones();
        ReadSelection(currentZone.GetZone());
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

    public void SelectNewZone(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayers))
        {
            UIZone selectedZone = hit.collider.GetComponent<UIZone>();

            if (selectedZone != null && selectedZone.IsUnlocked)
            {
                StartCoroutine(MoveIndicatorSmoothly(hit.point));
                currentZone = selectedZone;
            }
            else
            {
                Debug.Log("Zona no desbloqueada o no válida.");
            }
        }
    }

    public void ReadSelection(Zone zone)
    {
        levelText.text = "Nivel seleccionado: " + zone.ZoneName;
        descriptionText.text = "Descripción: " + zone.ZoneDescription;
    }

    public void UpdateUnlockedZones()
    {
        Zone currentZoneData = currentZone.GetZone();

        if (interactiveMap.Nodes.TryGetValue(currentZoneData.ZoneKey, out Node<Zone> currentNode))
        {
            foreach (var neighbor in currentNode.neighbors)
            {
                Zone neighborZone = neighbor.Key;
                UIZone neighborUIZone = neighborZone.GetUIZone();

                if (neighborUIZone != null)
                {
                    neighborUIZone.SetUnlocked(true);
                }
            }

            currentZone.SetUnlocked(true);

            foreach (var node in interactiveMap.Nodes)
            {
                Zone zone = node.Value.Key;

                bool isNeighbor = false;
                foreach (var neighbor in currentNode.neighbors)
                {
                    if (neighbor.Key.ZoneKey == zone.ZoneKey)
                    {
                        isNeighbor = true;
                        break;
                    }
                }

                if (zone.ZoneKey != currentZoneData.ZoneKey && !isNeighbor)
                {
                    UIZone uiZone = zone.GetUIZone();
                    if (uiZone != null)
                    {
                        uiZone.SetUnlocked(false);
                    }
                }
            }
        }
    }


}