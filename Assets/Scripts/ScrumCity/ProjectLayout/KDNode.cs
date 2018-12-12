using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ScrumCity
{
    public class KDNode
    {
        private Vector4 rect;

        public float X
        {
            get { return rect[0]; }
            set { rect[0] = value; }
        }

        public float Y
        {
            get { return rect[1]; }
            set { rect[1] = value; }
        }

        public float Width
        {
            get { return rect[2]; }
            set { rect[2] = value; }
        }

        public float Height
        {
            get { return rect[3]; }
            set { rect[3] = value; }
        }

        public float Area
        {
            get { return rect[2] * rect[3]; }   // area = width * height
        }

        public KDNode(float x, float y, float width, float height)
        {
            this.rect = new Vector4(x, y, width, height);
            //UnityEngine.Debug.Log(string.Format("New Node: x: {0}, y: {1}, width: {2}, height: {3} ", x, y, width, height));
        }

        public CityLayout Element { get; set; }

        public KDNode Left { get; set; }
        public KDNode Right { get; set; }

        public bool WillFit(CityLayout element)
        {
            //An element will fit the node if it's X and Z sizes are less than width and height
            return element.LayoutSize.x <= Width && element.LayoutSize.z <= Height;
        }

        public bool Available(CityLayout element)
        {
            // KDnode is available only if it will fit the element and it doesn't already have an occupying element or any child elements
            return WillFit(element) && Element == null && Left == null && Right == null;
        }

        public bool IsPerfectFit(CityLayout element)
        {
            // Is a perfect fit if element scale.x equals width and scale.y equals height
            return element.LayoutSize.x == Width && element.LayoutSize.z == Height;
        }

        public KDNode Split(CityLayout element)
        {
            // Create the node that is a perfect fit for the element and place at top right of current node
            KDNode fitnode = new KDNode(X, Y, element.LayoutSize.x, element.LayoutSize.z);
            fitnode.Element = element;

            // Split current Node Horizontally then verically based on fitnode
            Left = new KDNode(X, Y, Width, fitnode.Height);
            Right = new KDNode(X, Y + fitnode.Height, Width, Height - fitnode.Height);

            // Add fitnode as child to Left and split vertically
            Left.Left = fitnode;
            Left.Right = new KDNode(X + fitnode.Width, Y, Width - fitnode.Width, fitnode.Height);

            return fitnode;
        }

        public List<KDNode> AvailableNodes(CityLayout child)
        {
            List<KDNode> available = new List<KDNode>();
            if (Left != null) available.AddRange(Left.AvailableNodes(child));
            if (Right != null) available.AddRange(Right.AvailableNodes(child));

            if (Available(child)) available.Add(this);

            return available;
        }
    }
}
