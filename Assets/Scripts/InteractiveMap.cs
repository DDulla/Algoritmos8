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
        Nodes[zoneId1].AddNeighbor(Nodes[zoneId2]);
        Nodes[zoneId2].AddNeighbor(Nodes[zoneId1]);
    }
}
