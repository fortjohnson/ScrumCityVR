using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScrumCity
{
    public class CityBuilder
    {

        private string ProjectXMLFile { get; set; }
        private Dictionary<ObjectType, GameObject> Prefabs { get; set; }

        public CityBuilder(string projectXMlFile, Dictionary<ObjectType, GameObject> prefabs)
        {
            ProjectXMLFile = projectXMlFile;
            Prefabs = prefabs;
        }

        public GameObject Build()
        {
            ProjectXMLReader reader = new ProjectXMLReader();
            ProjectNode project = reader.LoadProject(ProjectXMLFile);
            PackageNode main = project.Packages[0];

            PackageLayout projectObject = new PackageLayout(main, Prefabs);

            GameObject city = projectObject.Build();

            city.transform.localScale = city.transform.localScale * .001f;
            var pos = city.transform.localPosition;
            pos.x -= .75f;
            pos.y += 1f;
            city.transform.localPosition = pos;

            return city;
        }

        private static void CombineMeshes(GameObject city)
        {
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
        }
    }
}