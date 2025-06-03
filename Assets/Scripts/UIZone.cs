using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIZone : MonoBehaviour
{
    [SerializeField] private int zoneKey;
    [SerializeField] private string zoneDescription;
    [SerializeField] private Material unlockedMaterial;
    [SerializeField] private Material lockedMaterial;

    private MeshRenderer meshRenderer;
    private Zone linkedZone;
    public bool IsUnlocked;

    private UnityEvent onZoneSelected;
    public int ZoneKey => zoneKey;
    public string ZoneDescription => zoneDescription;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        SetDescription(zoneDescription);
    }

    public void Initialize(Zone zone)
    {
        linkedZone = zone;
    }

    public void SetUnlocked(bool unlocked)
    {
        IsUnlocked = unlocked;
        if (meshRenderer != null)
        {
            meshRenderer.material = IsUnlocked ? unlockedMaterial : lockedMaterial;
        }
    }

    public void SetDescription(string description)
    {
        zoneDescription = description;
    }
    public void OnSelected(InputAction.CallbackContext context)
    {
        if (IsUnlocked)
        {
            Debug.Log("Zona seleccionada: " + linkedZone.ZoneName);
            onZoneSelected?.Invoke();
        }
        else
        {
            Debug.Log("Zona " + linkedZone.ZoneName + " está bloqueada.");
        }
    }

    public Zone GetZone()
    {
        return linkedZone;
    }
}