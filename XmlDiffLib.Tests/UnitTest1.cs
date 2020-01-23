using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XmlDiffLib.Tests
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestXmlDiff()
    {
      
      XmlDiff diff = new XmlDiff(TestResources.HAAR01000, TestResources.HAAR01001, "HAAR01000", "HAAR01001");
      XmlDiffOptions options = new XmlDiffOptions();
      options.IgnoreAttributeOrder = true;
      options.IgnoreAttributes = false;
      options.MaxAttributesToDisplay = 3;
      diff.CompareDocuments(options);
      Assert.AreEqual(diff.DiffNodeList.Count, 0);
      XmlDocument xdoc = new XmlDocument();
      xdoc.LoadXml(TestResources.HAAR01001);
      XmlNode node = xdoc.SelectSingleNode("//Product");
      node.Attributes["EffDate"].Value = "01/01/3000";
      diff = new XmlDiff(TestResources.HAAR01000, xdoc.InnerXml, "HAAR01000", "HAAR01001");
      diff.CompareDocuments(options);
      Assert.AreEqual(diff.DiffNodeList.Count, 2);
      Assert.IsTrue(diff.DiffNodeList[1].Description.Contains("EffDate"));
      xdoc.LoadXml(TestResources.HAAR01001);
      node = xdoc.SelectSingleNode("//Company");
      node.Attributes["CompanyID"].Value = "1000";
      diff = new XmlDiff(TestResources.HAAR01000, xdoc.InnerXml);
      diff.CompareDocuments(options);
      Assert.AreEqual(diff.DiffNodeList.Count, 2);
      Assert.IsTrue(diff.DiffNodeList[1].Description.Contains("CompanyID"));
    }

    /// <summary>
    /// GIVEN   an xml structure with a node to be ignored
    /// WHEN    the xpath is specified that needs to be ignored
    /// THEN    comparedocument should return true
    /// </summary>
    [TestMethod]
    public void TestXmlIgnoreXPathWithValue()
    {
        // Arrange
        string fromXml = "<Root><Node><SubNode>Foo</SubNode><IgnoreNode>Bar</IgnoreNode></Node><Node><SubNode><Element>Foo</Element></SubNode></Node></Root>";
        string toXml = "<Root><Node><SubNode>Foo</SubNode></Node><Node><SubNode><Element>Foo</Element></SubNode></Node></Root>";

        XmlDiff xmlDiff = new XmlDiff(fromXml, toXml);

        XmlDiffOptions xmlDiffOptions = new XmlDiffOptions();
        xmlDiffOptions.IgnoreXPaths.Add("Root/Node/IgnoreNode");

        // Act
        bool isSame = xmlDiff.CompareDocuments(xmlDiffOptions);

        // Assert
        Assert.IsTrue(isSame);
    }

    /// <summary>
    /// GIVEN   an xml structure with a node missing
    /// WHEN    no xpath to be ignored is not provided
    /// THEN    comparedocument should return false
    /// </summary>
    [TestMethod]
    public void TestXmlIgnoreXPathWithoutValue()
    {
        // Arrange
        string fromXml = "<Root><Node><SubNode>Foo</SubNode><IgnoreNode>Bar</IgnoreNode></Node><Node><SubNode><Element>Foo</Element></SubNode></Node></Root>";
        string toXml = "<Root><Node><SubNode>Foo</SubNode></Node><Node><SubNode><Element>Foo</Element></SubNode></Node></Root>";

        XmlDiff xmlDiff = new XmlDiff(fromXml, toXml);

        XmlDiffOptions xmlDiffOptions = new XmlDiffOptions();

        // Act
        bool isSame = xmlDiff.CompareDocuments(xmlDiffOptions);

        // Assert
        Assert.IsFalse(isSame);
    }
  }
}
