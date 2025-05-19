using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    private InteractiveMap interactiveMap = new InteractiveMap(); 

    [SerializeField] private GameObject indicator; // Indicador visual
    [SerializeField] private GameObject LineRendererPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Cámara
    [SerializeField] private Transform NodeHolder;
    [SerializeField] private GameObject[] planets; // Niveles
    [SerializeField] private GameObject[] extraPlanets; // Niveles extra
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private int currentNodeIndex = 0; // Índice del nodo actual

    void Start()
    {
        virtualCamera.Follow = indicator.transform;
        virtualCamera.LookAt = indicator.transform;

        Zone zoneA = new Zone("Zona A", "Zona número 1", planets[0], 0);
        Zone zoneB = new Zone("Zona B", "Zona número 2", planets[1], 1);
        Zone zoneC = new Zone("Zona C", "Zona número 3", planets[2], 2);
        Zone zoneD = new Zone("Zona D", "Zona número 4", planets[3], 3);
        Zone zoneE = new Zone("Zona E", "Zona número 5", planets[4], 4);

        Zone extraZone1 = new Zone("Zona Extra 1", "Zona extra número 1", extraPlanets[0], 10);
        Zone extraZone2 = new Zone("Zona Extra 2", "Zona extra número 2", extraPlanets[1], 20);

        interactiveMap.AddZone(zoneA);
        interactiveMap.AddZone(zoneB);
        interactiveMap.AddZone(zoneC);
        interactiveMap.AddZone(zoneD);
        interactiveMap.AddZone(zoneE);

        interactiveMap.AddZone(extraZone1);
        interactiveMap.AddZone(extraZone2);

        for (int i = 0; i < planets.Length - 1; i++)
        {
            interactiveMap.ConnectZones(i, i + 1);
        }

        interactiveMap.ConnectZones(1, 10);
        interactiveMap.ConnectZones(3, 20);

        NodeConnection();

        ReadSelection(interactiveMap.Nodes[currentNodeIndex].Key); 
    }

    public void UpdateSelection()
    {
        Node<Zone> selectedNode = interactiveMap.Nodes[currentNodeIndex];

        // Movimiento suave del indicador
        StartCoroutine(MoveIndicatorSmoothly(selectedNode.Key.ZoneObject.transform.position));
        ReadSelection(selectedNode.Key);
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
                GameObject LineR = Instantiate(LineRendererPrefab, NodeHolder);

                Zone nodeObject = node.Value.Key;
                Zone neighborObject = nodeNeighbor.Key;

                LineR.GetComponent<LineRenderer>().SetPosition(0, nodeObject.ZoneObject.transform.position);
                LineR.GetComponent<LineRenderer>().SetPosition(1, neighborObject.ZoneObject.transform.position);
            }
        }
    }

    public void ReadSelection(Zone zone)
    {
        levelText.text = "Nivel seleccionado: " + zone.ZoneName;
        descriptionText.text = "Descripción: " + zone.ZoneDescription;
    }
    public void MoveSelection(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        float input = context.ReadValue<float>(); // Leer input de "A" o "D"

        if (input < 0) // "A" → Nodo anterior
        {
            if (currentNodeIndex > 0)
            {
                currentNodeIndex--;
                UpdateSelection();
            }
        }
        else if (input > 0) // "D" → Nodo siguiente
        {
            if (currentNodeIndex < planets.Length - 1)
            {
                currentNodeIndex++;
                UpdateSelection();
            }
        }
    }
    public void MoveExtraSelection(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        float input = context.ReadValue<float>();

        if (input > 0)
        {
            if (interactiveMap.Nodes[currentNodeIndex].neighbors.Count > 2)
            {
                Node<Zone> selectedNode = interactiveMap.Nodes[currentNodeIndex].neighbors[2];
                StartCoroutine(MoveIndicatorSmoothly(selectedNode.Key.ZoneObject.transform.position));
                ReadSelection(selectedNode.Key);
            }
            else
                return;

        }
        else if (input < 0)
        {
            Node<Zone> selectedNode = interactiveMap.Nodes[currentNodeIndex];

            StartCoroutine(MoveIndicatorSmoothly(selectedNode.Key.ZoneObject.transform.position));

            ReadSelection(selectedNode.Key);
        }       
    }
}