using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScrumCity;

public class Details : MonoBehaviour {

    public string name;
    private CityNode node;
    public CityNode Node {
        get { return node; }
        set {
            node = value;
            name = node.Name;
        }
    }
}
