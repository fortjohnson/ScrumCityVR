using System;
using System.Collections.Generic;
using System.Linq;

namespace ScrumCity { 

    public abstract class CityNode
    {
        public string Name { get; private set; }
        public Dictionary<string, List<CityNode>> children = new Dictionary<string, List<CityNode>>();
        public List<CityNode> Children
        {
            get
            {
                return children.Values.SelectMany(x => x).ToList<CityNode>();
            }
        }

        public CityNode(string name)
        {
            Name = name;
        }

        public abstract void addChild(CityNode node);

    }


    public class ProjectNode : CityNode
    {
        public List<PackageNode> Packages
        {
            get
            {
                return children["packages"].Select(x => (PackageNode)x).ToList();
            }
        }

        public ProjectNode(string name) : base(name)
        {
            children["packages"] = new List<CityNode>();
        }

        public override void addChild(CityNode node)
        {
            if (node.GetType() != typeof(PackageNode))
                throw new ArgumentException("Invalid Model Type; Projects only contain Packages as children ");

            children["packages"].Add(node);
        }
    }


    public class PackageNode : CityNode
    {

        public List<PackageNode> Packages
        {
            get
            {
                return children["packages"].Select(x => (PackageNode)x).ToList();
            }
        }

        public List<ClassNode> Classes
        {
            get
            {
                return children["classes"].Select(x => (ClassNode)x).ToList();
            }
        }

        public PackageNode(string name) : base(name)
        {
            children["packages"] = new List<CityNode>();
            children["classes"] = new List<CityNode>();
        }

        public override void addChild(CityNode node)
        {
            if (node.GetType() != typeof(PackageNode) && node.GetType() != typeof(ClassNode))
                throw new ArgumentException("Invalid Model Type; Packages only contain Classes and other Packages as children");

            children[node.GetType() == typeof(PackageNode) ? "packages" : "classes"].Add(node);
        }
    }


    public class ClassNode : CityNode
    {
        public bool IsAbstract { get; private set; }
        public bool IsInterface { get; private set; }
        public bool IsEnum { get; set; }
        public int LOC { get; private set; }
        public int NOA { get; private set; }
        public int NOM
        {
            get
            {
                return children["methods"].Count;
            }
        }

        public List<MethodNode> Methods
        {
            get
            {
                return children["methods"].Select(x => (MethodNode)x).ToList();
            }
        }

        public ClassNode(string name, int loc, int noa, bool isAbstract=false, bool isInterface=false) : base(name)
        {
            IsAbstract = isAbstract;
            IsInterface = isInterface;
            LOC = loc;
            NOA = noa;
            children["methods"] = new List<CityNode>();


        }

        public override void addChild(CityNode node)
        {
            if (node.GetType() != typeof(MethodNode))
                throw new ArgumentException("Invalid Model Type; Project only have Packages as children");

            children["methods"].Add(node);
        }
    }


    public class MethodNode : CityNode
    {
        public int LOC { get; private set; }
        public int NOP { get; private set; }

        public MethodNode(string name, int loc, int nop) : base(name)
        {
            LOC = loc;
            NOP = nop;
        }

        public override void addChild(CityNode node)
        {
            throw new NotImplementedException("Methods have no children");
        }
    }
}

