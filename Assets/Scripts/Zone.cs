using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : Node<Zone>
{
    #region Properties
    private string zoneName;
    private string zoneDescription;
    private int zoneKey;
    private UIZone linkedUIZone;
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

    #region Methods
    public void Initialize(UIZone uizone)
    {
        linkedUIZone = uizone;
    }
    public UIZone GetUIZone()
    {
        return linkedUIZone;
    }
    #endregion
}