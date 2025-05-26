using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : Node<Zone>
{
    #region Properties
    [SerializeField] private string zoneName;
    [SerializeField] private string zoneDescription;
    [SerializeField] private int zoneKey;
    #endregion

    #region Getters
    public string ZoneName => zoneName;
    public string ZoneDescription => zoneDescription;
    public int ZoneKey => zoneKey;
    #endregion

    #region Constructor
    public Zone(string name, string description, int key) : base(null)
    {
        this.zoneName = name;
        this.zoneDescription = description;
        this.zoneKey = key;
    }
    #endregion
}