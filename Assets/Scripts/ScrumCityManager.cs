using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ScrumCity;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;

[Serializable]
public struct CityPrefabs
{
    public ObjectType Type;
    public GameObject Prefab;
}

public class ScrumCityManager : MonoBehaviour {

    public string projectXmlFile;

    public CityPrefabs[] prefabs = new CityPrefabs[4];

    // Use this for initialization
    private void Start ()
    {
        Dictionary<ObjectType, GameObject> prefabDict = prefabs.ToDictionary(p => p.Type, p => p.Prefab);

        CityBuilder cityBuilder = new CityBuilder(projectXmlFile, prefabDict);
        GameObject city = cityBuilder.Build();

        TwoHandManipulatable twm = city.gameObject.AddComponent<TwoHandManipulatable>() as TwoHandManipulatable;
        twm.EnableEnableOneHandedMovement = false;
        twm.ManipulationMode = ManipulationMode.MoveScaleAndRotate;
    }
}
