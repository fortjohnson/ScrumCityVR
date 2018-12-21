using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using ScrumCity;

public class ScrumModelTest {

    [Test]
    public void LoadingScrumModelPasses() {

        string scrumXmlFile = "C:\\Users\\david\\Documents\\projects\\Testing ScrumCity\\2-ScrumCity v2.0\\XmlScrums\\CassandraFeatures.xml";
        Release release = ScrumModelLoader.LoadReleaseFromXML(scrumXmlFile);

        Assert.AreEqual(1, release.Sprints.Count);
        Assert.AreEqual(48, release.Sprints[0].Features.Count);

        Feature f7 = release.Sprints[0].Features[6];
        Assert.AreEqual("F7", f7.ID, "Invalid ID");  //F7
        Assert.AreEqual("Ted", f7.Owner, "Invalid Owner"); //Ted
        Assert.AreEqual(4, f7.ClassRefs.Count, "Incorrect Class Refs Count"); //4
        Assert.AreEqual(1, f7.MethodRefs.Count, "Incorrect Method Refs Count"); //1
        Assert.AreEqual(f7.ClassRefs.Count + f7.MethodRefs.Count, f7.AllRefs.Count, "Incorrect Total Refs Count");
        Assert.AreEqual(2, f7.WorkEntries.Count, "Incorrect Work Entries Count");
        Assert.AreEqual("org.apache.cassandra.concurrent.JMXEnabledThreadPoolExecutor", f7.ClassRefs[2], "Incorrect Reference Name");

        WorkEntry we = f7.WorkEntries[1];
        Assert.AreEqual("org.apache.cassandra.concurrent.JMXConfigurableThreadPoolExecutor", we.QName, "Incorrect QName");
        Assert.AreEqual(new DateTime(2012, 6, 20, 12, 0, 0), we.Date, "Incorrect Date");
        Assert.AreEqual(6, we.Hours, "Incorrect Hours");
    }
}