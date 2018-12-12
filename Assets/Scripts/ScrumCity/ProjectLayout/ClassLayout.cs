using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScrumCity;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ClassLayout : CityLayout
{
    private float minWidth = 1.5f;
    private float minHeight = 1.5f;
    private float maxWidth = 100;
    private float maxHeight = 200;

    public override Vector3 Margin { get { return new Vector3(3f, 0f, 3f); } }

    public override Vector3 Position
    {
        // Custom get/set to deal with cylinders 
        get {
            Vector3 pos = GameObj.transform.localPosition;
            if (((ClassNode)Node).IsInterface) pos.y /= 2;
            return pos;
        }
        set
        {
            if (((ClassNode)Node).IsInterface) value.y *= 2;
            GameObj.transform.localPosition = value;
        }
    }

    public  ClassLayout(ClassNode node, Dictionary<ObjectType, GameObject> prefabs) : base(node, prefabs)
    {}

    public override GameObject Build()
    {
        ClassNode node = (ClassNode)Node;
        GameObj = MonoBehaviour.Instantiate(prefabs[node.IsInterface ? ObjectType.Interface : ObjectType.Class], Vector3.zero, Quaternion.identity);
        GameObj.GetComponent<CityObject>().Node = Node;

        // Set initial scale before modifying based on method placement
        float width = Mathf.Clamp(((ClassNode)Node).IsInterface ? ((ClassNode)Node).NOA * 1.5f : ((ClassNode)Node).NOA, minWidth, maxWidth);
        float height = Mathf.Clamp(((ClassNode)Node).NOM, minHeight, maxHeight);
        height = ((ClassNode)Node).IsInterface ? height / 2f : height;       // Cylinders are different than rectangles 
        GameObj.transform.localScale = new Vector3(width, height, width);

        Vector2 size = Vector2.zero;
        List<CityLayout> childLayouts = new List<CityLayout>();
        foreach (MethodNode childMeth in node.Methods)
        {
            MethodLayout mo = new MethodLayout(childMeth, prefabs);
            GameObject childGo = mo.Build();
            childLayouts.Add(mo);

            size += new Vector2(mo.LayoutSize.x, mo.LayoutSize.z);
        }

        LayoutChildren(GameObj, childLayouts, size);

        return GameObj;
    }

    private void LayoutChildren(GameObject parent, List<CityLayout> children, Vector2 size)
    {
        // Algorithm 3.1 in https://wettel.github.io/download/Wettel10b-PhDThesis.pdf
        // TODO: Generalize this function to place in Layout Base Class

        KDNode ptree = new KDNode(0, 0, size.x, size.y);
        Vector2 covrec = Vector2.zero;
        float totalArea = 0f;

        //order children by size
        children = children.OrderByDescending(s => s.LayoutSize.x * s.LayoutSize.z).ToList();
        foreach (CityLayout c in children)
        {
            List<KDNode> pNodes = ptree.AvailableNodes(c);
            Dictionary<KDNode, float> preservers = new Dictionary<KDNode, float>();
            Dictionary<KDNode, float> expanders = new Dictionary<KDNode, float>();

            KDNode targetNode;
            KDNode fitNode;

            // Check if each available node with preserve or expand current coverage
            foreach (var pNode in pNodes)
            {
                // Check if node preserves or expands coverage area
                if (pNode.X + c.LayoutSize.x <= covrec.x && pNode.Y + c.LayoutSize.z <= covrec.y)         // Adding element to this node preserves coverage so find waste
                {
                    // Waste is coverage area minus total occupied area plus new element area
                    float waste = covrec.x * covrec.y - (totalArea + (c.LayoutSize.x * c.LayoutSize.y));
                    preservers[pNode] = waste;
                }
                else                                                                            // Adding element to this node expands coverage so find new ratio
                {
                    // new ratio is new coverage area -> longer/short
                    float newX = pNode.X + c.LayoutSize.x > covrec.x ? pNode.X + c.LayoutSize.x : covrec.x;
                    float newY = pNode.Y + c.LayoutSize.z > covrec.y ? pNode.Y + c.LayoutSize.z : covrec.y;

                    expanders[pNode] = newX > newY ? newX / newY : newY / newX;
                }
            }

            // Find Node to assign to our target node
            if (preservers.Count > 0)   // If we have preservers use the node with the smallest waste
                targetNode = preservers.OrderBy(s => s.Value).First().Key;
            else                        // Otherwise using the aspect ratio closest to a square (or 1)
                targetNode = expanders.OrderBy(s => Mathf.Abs(s.Value - 1)).First().Key;

            // If target is a perfect fit do nothing otherwise split node to get a perfect fit
            if (targetNode.IsPerfectFit(c))
                fitNode = targetNode;
            else
                fitNode = targetNode.Split(c);

            // Update coverage based on added fitnode (but only if it expands either dimension
            covrec.x = fitNode.X + fitNode.Width > covrec.x ? fitNode.X + fitNode.Width : covrec.x;
            covrec.y = fitNode.Y + fitNode.Height > covrec.y ? fitNode.Y + fitNode.Height : covrec.y;


            //Place child on Parent (Unity uses center anchoring but KDtree using top left anchoring)
            float xPos = -ptree.Width / 2 + fitNode.Width / 2 + fitNode.X;
            float yPos = parent.transform.position.x - parent.transform.localScale.y / 2 + c.LayoutSize.y / 2;
            float zPos = ptree.Height / 2 - fitNode.Height / 2 - fitNode.Y;
            c.Position = new Vector3(xPos, yPos, zPos);

            //update total area of added elements
            totalArea += fitNode.Area;
        }

        //Calc Scale of Parent Package
        parent.transform.localScale = new Vector3
        {
            // TODO:  Make decision on Class Size scaling
            x = covrec.x > parent.transform.localScale.x ? covrec.x : parent.transform.localScale.x,
            y = parent.transform.localScale.y,
            z = covrec.y > parent.transform.localScale.z ? covrec.y : parent.transform.localScale.z
            //x = Mathf.Clamp(covrec.x, minWidth, maxWidth),
            //y = parent.transform.localScale.y,
            //z = Mathf.Clamp(covrec.y, minWidth, maxWidth)
        };

        //Move Children back scaled package and attach as children game objects
        foreach (CityLayout c in children)
        {
            float xDiff = (ptree.Width - covrec.x) / 2;
            float zDiff = (ptree.Height - covrec.y) / 2;

            Vector3 pos = c.GameObj.transform.localPosition;
            pos.x += xDiff;
            pos.z -= zDiff;
            c.GameObj.transform.localPosition = pos;

            c.GameObj.transform.parent = parent.transform;
        }
    }
}
