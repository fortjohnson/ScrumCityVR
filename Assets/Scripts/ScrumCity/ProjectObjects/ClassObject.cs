using System.Collections;
using System.Collections.Generic;
using ScrumCity;
using UnityEngine;

public class ClassObject : MonoBehaviour, CityObject
{
    private static Dictionary<Color, Material> materials = new Dictionary<Color, Material>();
    private static Dictionary<int, Color> colors = new Dictionary<int, Color>()
    {
        {  200, new Color(0f, .75f, 1f) },
        {  500, new Color(.25f, .41f, 1f) },
        { 1000, new Color(.42f, .35f, .8f) },
        { 1500, new Color(.58f, .44f, .86f) },
        { 2000, new Color(.78f, .08f, .52f) },
        { 2001, new Color(1f, .08f, .58f) }
    };

    private ClassNode node;
    public CityNode Node
    {
        get { return node; }
        set { node = (ClassNode)value; SetMaterial(); }
    }

    private Renderer rend;
    private MaterialPropertyBlock block;
    private int colorID;
    Color color;

    private void SetMaterial()
    {
        if (node.LOC < 200) color = colors[200];
        else if (node.LOC < 500) color = colors[500];
        else if (node.LOC < 1000) color = colors[1000];
        else if (node.LOC < 1500) color = colors[1500];
        else if (node.LOC < 2000) color = colors[2000];
        else color = colors[2001];

        rend = GetComponent<Renderer>();
        Material mat;
        if (!materials.TryGetValue(color, out mat))
        {
            mat = new Material(rend.sharedMaterial);
            mat.color = color;
            materials[color] = mat;
            //Debug.Log(string.Format("New Color - {0} - for LOC: {1}", c, node.LOC));
        }
        rend.sharedMaterial = mat;

        block = new MaterialPropertyBlock();
        colorID = Shader.PropertyToID("_Color");

        block.SetColor(colorID, color);
        rend.SetPropertyBlock(block);
    }
}
