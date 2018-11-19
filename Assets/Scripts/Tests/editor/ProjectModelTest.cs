using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using ScrumCity;

public class ProjectModelTest {

    [Test]
    public void GenerateProjectModelPasses() {
        // Use the Assert class to test conditions.

        //Create a couple methods
        MethodNode meth1 = new MethodNode("Method1", 10, 1);
        MethodNode meth2 = new MethodNode("Method2", 35, 2);
        MethodNode meth3 = new MethodNode("Method3", 10, 1);
        MethodNode meth4 = new MethodNode("Method4", 35, 2);

        //Create a couple classes
        ClassNode class1 = new ClassNode("Class1", 65, 5);
        ClassNode class2 = new ClassNode("Class2", 250, 3);

        //Add some methods to the classes
        class1.addChild(meth1);
        class1.addChild(meth2);
        class1.addChild(meth3);
        class2.addChild(meth4);

        //Create a couple packages
        PackageNode package1 = new PackageNode("org");
        PackageNode package2 = new PackageNode("org.package");
        PackageNode package3 = new PackageNode("org.package.tests");
        PackageNode package4 = new PackageNode("org.audit");

        //Add packages to packages
        package1.addChild(package2);
        package1.addChild(package4);
        package2.addChild(package3);

        //Add classes to packages
        package2.addChild(class1);
        package4.addChild(class2);

        //Create Root Project
        ProjectNode project = new ProjectNode("Example Source");
        project.addChild(package1);

        Assert.AreEqual(project.Children.Count, 1);
        Assert.AreEqual(project.Children[0], package1);
        Assert.AreEqual(project.Children[0].Children[0], package2);
        Assert.AreEqual(project.Children[0].Children[1], package4);
        Assert.AreEqual(project.Children[0].Children[0].Children[0], package3);

        Assert.AreEqual(package1.Children.Count, 2);
        Assert.AreEqual(package2.Children.Count, 2);
        Assert.AreEqual(package3.Children.Count, 0);
        Assert.AreEqual(package4.Children.Count, 1);

        Assert.AreEqual(package1.Classes.Count, 0);
        Assert.AreEqual(package1.Packages.Count, 2);
        Assert.AreEqual(package2.Classes.Count, 1);
        Assert.AreEqual(package2.Packages.Count, 1);

        Assert.AreEqual(package1.Children[0], package2);
        Assert.AreEqual(package1.Children[1], package4);
        Assert.AreEqual(package2.Children[0], package3);
        Assert.AreEqual(package2.Children[1], class1);
        Assert.AreEqual(package4.Children[0], class2);

        Assert.AreEqual(class1.Children.Count, 3);
        Assert.AreEqual(class2.Children.Count, 1);
        Assert.AreEqual(class1.Methods.Count, 3);
        Assert.AreEqual(class2.Methods.Count, 1);

        Assert.AreEqual(class1.Children[0], meth1);
        Assert.AreEqual(class1.Children[1], meth2);
        Assert.AreEqual(class1.Children[2], meth3);
        Assert.AreEqual(class2.Children[0], meth4);

        Assert.AreEqual(class1.NOA, 5);
        Assert.AreEqual(class1.NOM, 3);
        Assert.AreEqual(class1.LOC, 65);

        Assert.Throws<System.NotImplementedException>(delegate { meth1.addChild(meth2); });
    }

    [Test]
    public void XmlLoadPasses()
    {

        ProjectXMLReader reader = new ProjectXMLReader();
        ProjectNode project = reader.LoadProject("C:/Users/david/Documents/projects/ScrumCity/project.xml");

        Assert.AreEqual(project.Name, "Cassandra TestCase");
        Assert.AreEqual(project.Children[0].Name, "org");

        PackageNode cassandra = project.Packages[0].Packages[0].Packages[0];
        Assert.AreEqual("org.apache.cassandra", cassandra.Name);
        Assert.AreEqual(29, cassandra.Packages.Count);
        Assert.AreEqual(0, cassandra.Classes.Count);
        Assert.AreEqual(29, cassandra.Children.Count);


        Assert.AreEqual("org.apache.cassandra.auth", cassandra.Packages[0].Name);
        PackageNode auth = cassandra.Packages[0];
        Assert.AreEqual(0, auth.Packages.Count);
        Assert.AreEqual(43, auth.Classes.Count);

        string dbName = "org.apache.cassandra.db";
        PackageNode db = (PackageNode) cassandra.Children.First(s => s.Name == dbName);
        Assert.AreEqual(dbName, db.Name);
        Assert.AreEqual(9, db.Packages.Count);
        Assert.AreEqual(186, db.Classes.Count);
        Assert.AreEqual(9 + 186, db.Children.Count);

        string cellName = "Cell";
        ClassNode cell = (ClassNode)db.Children.First(s => s.Name == cellName);
        Assert.AreEqual(cellName, cell.Name);
        Assert.AreEqual(cell.LOC, 1);
        Assert.AreEqual(cell.NOM, 13);
        Assert.AreEqual(cell.NOA, 1);
        Assert.IsTrue(cell.IsInterface);
        Assert.IsFalse(cell.IsAbstract);
        Assert.IsFalse(cell.IsEnum);
        Assert.AreEqual(13, cell.Methods.Count);
        Assert.AreEqual(13, cell.Children.Count);

        string methodName = "reconcile";
        MethodNode rec = (MethodNode)cell.Children.First(s => s.Name == methodName);
        Assert.AreEqual(methodName, rec.Name);
    }
}
