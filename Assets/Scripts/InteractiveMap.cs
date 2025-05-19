using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveMap : Graph<int, Zone>
{
    public void AddZone(Zone zone)
    {
        AddNode(zone.ZoneKey, zone);
    }
    public void ConnectZones(int zoneId1, int zoneId2)
    {
        AddEdge(zoneId1, zoneId2);
        AddEdge(zoneId2, zoneId1);
    }
}
