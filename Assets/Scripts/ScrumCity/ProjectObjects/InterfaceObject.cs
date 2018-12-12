using System.Collections;
using System.Collections.Generic;
using ScrumCity;
using UnityEngine;

public class InterfaceObject : MonoBehaviour, CityObject
{
    private ClassNode node;
    public CityNode Node
    {
        get { return node; }
        set { node = (ClassNode)value; }
    }
}
