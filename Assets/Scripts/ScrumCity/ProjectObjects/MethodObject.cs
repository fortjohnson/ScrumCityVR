using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScrumCity;

public class MethodObject : MonoBehaviour, CityObject
{
    private MethodNode node;
    public CityNode Node
    {
        get { return node; }
        set { node = (MethodNode)value; }
    }
}
