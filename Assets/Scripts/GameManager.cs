using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    private Graph<int, Zone> graph = new Graph<int, Zone>();

    [SerializeField] private Zone[] levels; // Niveles
    [SerializeField] private GameObject indicator; // Indicador visual
    [SerializeField] private GameObject LineRendererPrefab;
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Cámara
    [SerializeField] private Transform NodeHolder;
    [SerializeField] private GameObject[] planets; // Niveles

    private float direction;

    private int currentNodeIndex = 0; // Índice del nodo actual

    void Start()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            graph.AddNode(i, levels[i]);
        }
        // Conectar nodos en orden
        for (int i = 0; i < levels.Length - 1; i++)
        {
            graph.AddEdge(i, i + 1);
        }
        NodeConection();
        // Inicializar selección
        UpdateSelection();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        direction = context.ReadValue<float>(); // Leer input de "A" o "D"
        MoveSelection();
    }
    public void MoveSelection()
    {
        if (direction < 0) // "A" → Nodo anterior
        {
            if (currentNodeIndex > 0)
            {
                currentNodeIndex--;
                UpdateSelection();
            }
        }
        else if (direction > 0) // "D" → Nodo siguiente
        {
            if (currentNodeIndex < levels.Length - 1)
            {
                currentNodeIndex++;
                UpdateSelection();
            }
        }
    }

    public void UpdateSelection()
    {
        Node<Zone> selectedNode = graph.Nodes[currentNodeIndex];

        // Mover la cámara al nuevo nodo
        virtualCamera.Follow = indicator.transform;
        virtualCamera.LookAt = indicator.transform;

        // Movimiento suave del indicador
        StartCoroutine(MoveIndicatorSmoothly(selectedNode.Key.ZoneObject.transform.position));
    }

    private IEnumerator MoveIndicatorSmoothly(Vector3 targetPosition)
    {
        Vector3 startPosition = indicator.transform.position;
        targetPosition += new Vector3(0, 1, 0); // Ajuste de altura

        float duration = 0.3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            indicator.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            indicator.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, elapsedTime / duration); // Efecto de escala
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        indicator.transform.position = targetPosition;
        indicator.transform.localScale = Vector3.one;
    }

    public void ConfirmSelection(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Debug.Log($"Nivel seleccionado: {currentNodeIndex}");
        // Aquí se carga la escena del nivel correspondiente
    }
    public void NodeConection()
    {
        foreach (var node in graph.Nodes)
        {
            foreach(var nodeNeighbor in node.Value.neighbors)
            {
                GameObject LineR = Instantiate(LineRendererPrefab, NodeHolder);

                Zone nodeObject = node.Value.Key; 
                Zone neighborObject = nodeNeighbor.Key;

                LineR.GetComponent<LineRenderer>().SetPosition(0, nodeObject.ZoneObject.transform.position);
                LineR.GetComponent<LineRenderer>().SetPosition(1, neighborObject.ZoneObject.transform.position);
            }
        }
    }
}