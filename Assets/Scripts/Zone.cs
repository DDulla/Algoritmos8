using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : Node<int>
{
    [SerializeField] private string zoneName;
    [SerializeField] private string zoneDescription;

    public string ZoneName => zoneName;
    public string ZoneDescription => zoneDescription;

    public Zone(string zoneName, string zoneDescription, int key) : base(key)
    {
        this.zoneName = zoneName;
        this.zoneDescription = zoneDescription;
    }
}