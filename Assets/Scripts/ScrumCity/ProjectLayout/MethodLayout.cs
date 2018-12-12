using System;
using System.Collections.Generic;
using UnityEngine;
using ScrumCity;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MethodLayout : CityLayout {

    public override Vector3 Margin { get { return new Vector3(2f, 0.5f, 2f); } }

    public MethodLayout(MethodNode node, Dictionary<ObjectType, GameObject> prefabs) : base(node, prefabs)
    { }

    public override GameObject Build()
    {
        GameObj = MonoBehaviour.Instantiate(prefabs[ObjectType.Method], Vector3.zero, Quaternion.identity);
        GameObj.GetComponent<CityObject>().Node = Node;
        GameObj.transform.localScale = LayoutSize - Margin;
        return GameObj;
    }
}
