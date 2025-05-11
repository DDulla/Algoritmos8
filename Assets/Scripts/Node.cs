using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    #region Properties
    private T key;
    public List<Node<T>> neighbors;
    #endregion
    #region Getters
    public T Key => key;
    public List<Node<T>> Neighbors => neighbors;
    #endregion

    public Node(T _key)
    {
        key = _key;
        neighbors = new List<Node<T>>();
    }
    #region Methods
    public void AddNeighbor(Node<T> neighbor)
    {
        if (!Neighbors.Contains(neighbor))
        {
            Neighbors.Add(neighbor);
        }
    }
    #endregion
}