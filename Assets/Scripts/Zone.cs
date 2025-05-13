using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : Node<Zone>
{
    #region Properties
    [SerializeField] private string zoneName;
    [SerializeField] private string zoneDescription;
    [SerializeField] private int zoneKey;
    [SerializeField] private GameObject zoneObject;
    #endregion

    #region Getters
    public string ZoneName => zoneName;
    public string ZoneDescription => zoneDescription;
    public int ZoneKey => zoneKey;
    public GameObject ZoneObject => zoneObject;
    #endregion

    #region Constructor
    public Zone(string name, string description, GameObject zoneObject, int key) : base(null)
    {
        this.zoneName = name;
        this.zoneDescription = description;
        this.zoneKey = key;
        this.zoneObject = zoneObject;
    }
    #endregion
}