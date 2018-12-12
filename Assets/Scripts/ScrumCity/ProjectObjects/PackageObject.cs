using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScrumCity;

public class PackageObject : MonoBehaviour, CityObject
{
    // Static dictionary to store created materials 
    // (keeps us from creating a new material for every package)
    private static Dictionary<int, Material> materials = new Dictionary<int, Material>();

    public float maxGrayVal = .95f;

    private PackageNode node;
    public CityNode Node
    {
        get { return node; }
        set { node = (PackageNode)value; SetMaterial(); }
    }

    private Renderer rend;

    public int NestingLevel
    {
        get
        {
            return node.Name.Split('.').Length;
        }
    }

    private void SetMaterial()
    {
        //// Set Materials for each package
        //rend = GetComponent<Renderer>();
        //Material mat;
        //if (!materials.TryGetValue(NestingLevel, out mat))
        //{
        //    mat = new Material(rend.material);
        //    materials[NestingLevel] = mat;
        //    //Debug.Log(string.Format("New Material at level {0}", NestingLevel));
        //}
        //rend.material = mat;
    }

    private void Start()
    {
        // Update color or each material based on nesting level
        //int maxNest = materials.Keys.Max();
        //float grayVal = NestingLevel / (float)maxNest * maxGrayVal;
        //Color color = new Color(grayVal, grayVal, grayVal);
        //rend.sharedMaterial.color = color;
    }

}
