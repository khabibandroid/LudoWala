using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System;

namespace Toast.Kit.Manager.Constant
{
    public enum ServiceStatus
    {
        NONE,
        [XmlEnum("publish")]
        PUBLISH,
        [XmlEnum("prepare")]
        PREPARE
    }

    public enum ServiceInstall
    {
        [XmlEnum("auto")]
        AUTO,
        [XmlEnum("manual")]
        MANUAL
    }

    [XmlRoot("info")]
    public class ServiceInfo
    {
        public class Package
        {
            public string path;
            public string installPath;
        }

        public class Link
        {
            public string name;
            public string url;
        }

        public class Image
        {
            public string title;
            public string path;
        }

        public class DependencyInfo
        {
            public string version;
            public ServiceInstall install;
        }

        [XmlAttribute("version")]
        public string infoVersion;

        public string title;
        public ServiceStatus status;
        public string description;
        public string path;
        public string version;

        [XmlArrayItem("link")]
        public List<Link> linkList;

        [XmlArrayItem("image")]
        public List<Image> imageList;

        [XmlArrayItem("package")]
        public List<Package> packageList;

        [XmlIgnore]
        public Dictionary<string, DependencyInfo> dependencies;
        
        [XmlAnyElement("dependencies")]
        public XmlElement XmlDependencies
        {
            get
            {
                return null;
            }
            set
            {
                if (value == null)
                {
                    dependencies = null;
                }
                else
                {
                    var dependenciesElements = XElement.Parse(value.OuterXml);

                    dependencies = dependenciesElements.Elements().ToDictionary(
                        e => e.Name.LocalName,
                        e =>
                        {
                            var serializer = new XmlSerializer(typeof(DependencyInfo), new XmlRootAttribute(e.Name.LocalName));
                            var reader = e.CreateReader();

                            return (DependencyInfo)serializer.Deserialize(reader);
                        },
                        StringComparer.OrdinalIgnoreCase);
                }
            }
        }
    }
}