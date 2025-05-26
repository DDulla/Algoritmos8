using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveMap : Graph<int, Zone>
{
    public void AddZone(Zone zone)
    {
        AddNode(zone.ZoneKey, zone);
    }
}
