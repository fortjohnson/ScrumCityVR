using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScrumCity;
using HoloToolkit.Unity.InputModule.Utilities.Interactions;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CityBuilder : MonoBehaviour {

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
        PackageLayout projectObject = new PackageLayout(main, prefabs);

        GameObject city = projectObject.Build();

        city.transform.localScale = city.transform.localScale * .001f;
        var pos = city.transform.localPosition;
        pos.x -= .75f;
        pos.y += 1f;
        city.transform.localPosition = pos;

        MeshFilter[] meshFilters = city.transform.GetComponentsInChildren<MeshFilter>();
        Debug.Log("Meshes:" + meshFilters.Length);
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        foreach (var mesh in meshFilters)
        {
            combine[i].mesh = mesh.mesh;
            combine[i].transform = mesh.transform.localToWorldMatrix;
            mesh.gameObject.SetActive(false);
            i++;
        }

        Mesh combineMesh = new Mesh();
        combineMesh.CombineMeshes(combine);
        city.GetComponent<MeshFilter>().mesh = combineMesh;
        city.SetActive(true);

        TwoHandManipulatable twm = city.gameObject.AddComponent<TwoHandManipulatable>() as TwoHandManipulatable;
        twm.EnableEnableOneHandedMovement = false;
        twm.ManipulationMode = ManipulationMode.MoveScaleAndRotate;
    }
}