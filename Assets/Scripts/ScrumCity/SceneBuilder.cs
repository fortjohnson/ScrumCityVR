using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScrumCity;

public class SceneBuilder : MonoBehaviour {

    public string projectXmlFile;

    public GameObject packagePrefab;
    public GameObject classPrefab;
    public GameObject interfacePrefab;
    public GameObject methodPrefab;


    // Use this for initialization
    void Start() {
        ProjectXMLReader reader = new ProjectXMLReader();
        ProjectNode project = reader.LoadProject(projectXmlFile);
        PackageNode main = project.Packages[0];

        Dictionary<ObjectType, GameObject> prefabs = new Dictionary<ObjectType, GameObject>()
        {
            { ObjectType.Package, packagePrefab },
            { ObjectType.Class, classPrefab },
            { ObjectType.Interface, interfacePrefab },
            { ObjectType.Method, methodPrefab },
        };
        PackageLayout authObject = new PackageLayout(main, prefabs);

        GameObject city = authObject.Build();

        city.transform.localScale = city.transform.localScale * .5f;
        //var pos = city.transform.localPosition;
        //pos.x -= 1f;
        //pos.y += .5f;
        //city.transform.localPosition = pos;
    }

}