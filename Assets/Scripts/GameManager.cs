using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    private Graph<int, GameObject> graph = new Graph<int, GameObject>();

    [SerializeField] private GameObject[] gameObjects; // Planetas
    [SerializeField] private GameObject indicator; // Indicador visual
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Cámara

    private int currentNodeIndex = 0; // Índice del nodo actual

    void Start()
    {
        // Crear nodos (planetas)
        for (int i = 0; i < gameObjects.Length; i++)
        {
            graph.AddNode(i, gameObjects[i]);
        }

        // Conectar nodos en orden
        for (int i = 0; i < gameObjects.Length - 1; i++)
        {
            graph.AddEdge(i, i + 1);
        }

        // Inicializar selección
        UpdateSelection();
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
            if (currentNodeIndex < gameObjects.Length - 1)
            {
                currentNodeIndex++;
                UpdateSelection();
            }
        }
    }

    public void UpdateSelection()
    {
        Node<GameObject> selectedNode = graph.Nodes[currentNodeIndex];

        // Mover la cámara al nuevo nodo
        virtualCamera.Follow = indicator.transform;
        virtualCamera.LookAt = indicator.transform;

        // Movimiento suave del indicador
        StartCoroutine(MoveIndicatorSmoothly(selectedNode.Key.transform.position));
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
        // Aquí puedes cargar la escena del nivel correspondiente
    }
}