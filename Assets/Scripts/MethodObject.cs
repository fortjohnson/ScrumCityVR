using System;
using System.Collections.Generic;
using UnityEngine;
using ScrumCity;

public class MethodObject : CityLayout {

    public override Vector3 Margin { get { return new Vector3(2f, 0.5f, 2f); } }

    public MethodObject(MethodNode node, Dictionary<ObjectType, GameObject> prefabs) : base(node, prefabs)
    { }

    public override GameObject Build()
    {
        GameObj = MonoBehaviour.Instantiate(prefabs[ObjectType.Method], Vector3.zero, Quaternion.identity);
        GameObj.GetComponent<Details>().Node = Node;
        GameObj.transform.localScale = LayoutSize - Margin;
        return GameObj;
    }
}
