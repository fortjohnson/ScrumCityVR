using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScrumCity
{
    public class LayoutNotBuiltException : System.Exception
    {
        public LayoutNotBuiltException() { }
        public LayoutNotBuiltException(string message) : base(message) { }
        public LayoutNotBuiltException(string message, System.Exception inner) : base(message, inner) { }
    }

    public enum ObjectType { Package, Class, Interface, Method }

    public abstract class CityLayout
    {
        protected readonly Dictionary<ObjectType, GameObject> prefabs;

        abstract public Vector3 Margin { get; }
        abstract public GameObject Build();

        public GameObject GameObj { get; protected set; }
        public CityNode Node { get; protected set; }

        public Vector3 LayoutSize
        {
            get
            {
                if (GameObj == null) throw new LayoutNotBuiltException("Layout hasn't been built yet");
                return GameObj.transform.localScale + Margin;
            }
        }

        public virtual Vector3 Position
        {
            get
            {
                if (GameObj == null) throw new LayoutNotBuiltException("Layout hasn't been built yet");
                return GameObj.transform.localPosition;
            }
            set
            {
                if (GameObj == null) throw new LayoutNotBuiltException("Layout hasn't been built yet");
                GameObj.transform.localPosition = value;
            }
        }

        public CityLayout(CityNode node, Dictionary<ObjectType, GameObject> prefabs)
        {
            Node = node;
            this.prefabs = prefabs;
        }
        
    }
}