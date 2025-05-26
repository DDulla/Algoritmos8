using System.Collections.Generic;
using System;
using UnityEngine;
//using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Graph<TKey, TNodeValue>
{
    public Dictionary<TKey, Node<TNodeValue>> Nodes { get; set; }

    public Graph()
    {
        Nodes = new Dictionary<TKey, Node<TNodeValue>>();
    }

    public void AddNode(TKey key, TNodeValue value)
    {
        if (!Nodes.ContainsKey(key))
        {
            Node<TNodeValue> newNode = new Node<TNodeValue>(value);
            //Nodes[key] = newNode;
            Nodes.Add(key, newNode);
        }
    }

    public void AddEdge(TKey key1, TKey key2)
    {
        if (Nodes.ContainsKey(key1) && Nodes.ContainsKey(key2))
        {
            Node<TNodeValue> n1 = Nodes[key1];
            Node<TNodeValue> n2 = Nodes[key2];

            n1.AddNeighbor(n2);
        }
        else
        {
            Debug.Log("Uno o ambos nodos no existen en el grafo.");
        }
    }
    public void DisplayGraphAsMatrix()
    {
        int size = Nodes.Count;
        if (size == 0)
        {
            Debug.Log("El grafo está vacío.");
            return;
        }
        int[,] adjacencyMatrix = new int[size, size];
        Dictionary<TKey, int> nodeIndexMap = new Dictionary<TKey, int>();
        TKey[] keys = new TKey[size];
        // Mapear claves a índices
        int index = 0;
        foreach (var key in Nodes.Keys)
        {
            nodeIndexMap[key] = index;
            keys[index] = key;
            index++;
        }
        // Construir matriz de adyacencia
        foreach (var kvp in Nodes)
        {
            int nodeIndex = nodeIndexMap[kvp.Key];
            foreach (var neighbor in kvp.Value.Neighbors)
            {
                // Necesitamos encontrar la clave correspondiente al nodo vecino
                foreach (var entry in Nodes)
                {
                    if (EqualityComparer<Node<TNodeValue>>.Default.Equals(entry.Value, neighbor))
                    {
                        int neighborIndex = nodeIndexMap[entry.Key];
                        adjacencyMatrix[nodeIndex, neighborIndex] = 1;
                        adjacencyMatrix[neighborIndex, nodeIndex] = 1; // simétrico
                        break;
                    }
                }
            }
        }
        #region Console Draw
        Debug.Log("Matriz de adyacencia:");

        // Encabezado
        string header = " ";
        for (int i = 0; i < size; i++)
        {
            header += $"{keys[i].ToString().PadRight(10)}";
        }
        Debug.Log(header);

        // Filas
        for (int i = 0; i < size; i++)
        {
            string row = $"{keys[i].ToString().PadRight(5)}";
            for (int j = 0; j < size; j++)
            {
                row += $"{(adjacencyMatrix[i, j] == 1 ? "Si" : "No").PadRight(10)}";
            }
            Debug.Log(row);
        }
        #endregion
    }
    public void DisplayGraphAsList()
    {
        Debug.Log("Grafo:");
        if (Nodes.Count == 0)
        {
            Debug.Log("El grafo está vacío.");
            return;
        }
        Debug.Log("Lista de adyacencia: \n");
        foreach (var node in Nodes)
        {
            string NodeValues = "";
            NodeValues += "Nodo" + node.Key + ":";
            // Necesitamos encontrar las claves correspondientes a los nodos vecinos foreach (var neighbor in node.Value.Neighbors)
            foreach (var neighbor in node.Value.Neighbors)
            {
                foreach (var entry in Nodes)
                {
                    if (EqualityComparer<Node<TNodeValue>>.Default.Equals(entry.Value, neighbor))
                    {
                        NodeValues += "" + entry.Key;
                        break;
                    }
                }
            }
            Debug.Log(NodeValues);
        }
    }
}