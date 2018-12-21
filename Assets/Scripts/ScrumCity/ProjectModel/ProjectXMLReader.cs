using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;

namespace ScrumCity
{
    public class ProjectXMLReader
    {
        private ProjectNode Project { get; set; }

        public ProjectNode LoadProject(string xmlfile)
        {

            XmlReader reader = XmlReader.Create(xmlfile);
            reader.MoveToContent();
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            XmlNode root = doc.FirstChild;

            if (root.Name != "project")
                throw new System.Xml.XmlException("XML input doesn't start with project");

            Project = new ProjectNode(root.Attributes["name"].Value);
            XmlNodeList children = root.SelectSingleNode("./packages").ChildNodes;

            foreach (XmlNode node in children)
            {
                LoadModel(Project, node);
            }

            return Project;
        }

        private void LoadModel(CityNode parent, XmlNode node)
        {
            CityNode model;
            List<XmlNode> children = new List<XmlNode>();
            switch (node.Name)
            {
                case "project":
                    {
                        model = new ProjectNode(node.Attributes["name"].Value);
                        parent.addChild(model);

                        XmlNode packages = node.SelectSingleNode("./packages");
                        if (packages != null)
                            children = children.Concat(packages.ChildNodes.Cast<XmlNode>()).ToList();
                        break;
                    }
                case "package":
                    {
                        model = new PackageNode(node.SelectSingleNode("./name").InnerText, node.Attributes["qname"].Value);
                        parent.addChild(model);

                        XmlNode packages = node.SelectSingleNode("./packages");
                        if (packages != null)
                            children = children.Concat(packages.ChildNodes.Cast<XmlNode>()).ToList();

                        XmlNode classes = node.SelectSingleNode("./classes");
                        if (classes != null)
                            children = children.Concat(classes.ChildNodes.Cast<XmlNode>()).ToList();
                        break;
                    }
                case "class":
                    {
                        string name = node.SelectSingleNode("./name").InnerText;
                        int loc = int.Parse(node.SelectSingleNode("./loc").InnerText);
                        int noa = int.Parse(node.SelectSingleNode("./noa").InnerText);
                        bool isAbstract = node.Attributes["abstract"] != null ? true : false;
                        bool isInterface = node.Attributes["interface"] != null ? true : false;

                        model = new ClassNode(name, node.Attributes["qname"].Value, loc, noa, isAbstract, isInterface);
                        parent.addChild(model);

                        XmlNode methods = node.SelectSingleNode("./methods");
                        if (methods != null)
                            children = children.Concat(methods.ChildNodes.Cast<XmlNode>()).ToList();
                    }
                    break;
                case "method":
                    {
                        string name = node.SelectSingleNode("./name").InnerText;
                        int loc = int.Parse(node.SelectSingleNode("./loc").InnerText);
                        int nop = int.Parse(node.SelectSingleNode("./nop").InnerText);

                        ((ClassNode)parent).IsEnum = node.SelectSingleNode("./enum") != null;
                        model = new MethodNode(name, node.Attributes["qname"].Value, loc, nop);
                        parent.addChild(model);

                        // leave children empty since methods have no children
                    }
                    break;
                default:
                    throw new System.Xml.XmlException("Invalid Element Type " + node.Name);
            }

            foreach (XmlNode child in children)
            {
                LoadModel(model, child);
            }
        }
    }
}


