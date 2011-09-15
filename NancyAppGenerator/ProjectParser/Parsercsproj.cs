using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
namespace NancyAppGenerator.ProjectParser
{
    class Parsercsproj
    {
        string FilePath;
        static XDocument projDefinition;
        XNamespace msbuild;
        int changes = 0;
        public Parsercsproj(string currentPath)
        {
            string[] files = Directory.GetFiles(currentPath, "*.csproj");
            this.FilePath = files[0];
            msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";
            projDefinition = XDocument.Load(FilePath);

            IEnumerable<string> references = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "ItemGroup")
                .Elements(msbuild + "Reference")
                .Select(refElem => refElem.Value);
            RootNameSpace = projDefinition
                .Element(msbuild + "Project")
                .Elements(msbuild + "PropertyGroup")
                .Elements(msbuild + "RootNamespace")
                .Select(refElem => refElem.Value).FirstOrDefault();
        }

        public string RootNameSpace { get; set; }

        public void AddCompileFile(string fileName)
        {
            var Exist = projDefinition
               .Element(msbuild + "Project")
               .Elements(msbuild + "ItemGroup")
               .Elements(msbuild + "Compile")
               .Attributes("Include")
               .Where(x => x.Value == fileName).Count() > 0;
            var test1 = projDefinition
                    .Element(msbuild + "Project")
                    .Elements(msbuild + "ItemGroup")
                    .Elements(msbuild + "Compile")
                    .Last();
            var test2 = projDefinition
                    .Element(msbuild + "Project")
                    .Elements(msbuild + "ItemGroup")
                    .Elements(msbuild + "Compile");
            if (!Exist)
            {
                XElement elem = new XElement(msbuild + "Compile");
                elem.SetAttributeValue("Include", fileName);
                projDefinition
                    .Element(msbuild + "Project")
                    .Elements(msbuild + "ItemGroup")
                    .Elements(msbuild + "Compile")
                    .Last().AddAfterSelf(elem);
                ++changes;
            }
        }
        public void AddContentFile(string fileName, CopyOutPutOptions option)
        {
            var Exist = projDefinition
              .Element(msbuild + "Project")
              .Elements(msbuild + "ItemGroup")
              .Elements(msbuild + "Content")
              .Attributes("Include")
              .Where(x => x.Value == fileName).Count() > 0;
            if (!Exist)
            {
                XElement elem = new XElement(msbuild + "Content");
                elem.SetAttributeValue("Include", fileName);
                if (option == CopyOutPutOptions.PreserveNewest || option == CopyOutPutOptions.Always)
                {
                    XElement copyoutput=new XElement(msbuild +"CopyToOutputDirectory");
                    copyoutput.Value = option.ToString();
                    elem.Add(copyoutput);
                }
                var itemGroup=projDefinition
                    .Element(msbuild + "Project")
                    .Elements(msbuild + "ItemGroup")
                    .Where(x =>( x.Elements(msbuild + "None").Count() + x.Elements(msbuild + "Content").Count() )>0).FirstOrDefault();
                var itemgroups = projDefinition
                    .Element(msbuild + "Project")
                    .Elements(msbuild + "ItemGroup");
                    itemGroup.Elements().Last().AddAfterSelf(elem);
                ++changes;
            }
        }
        public void Save()
        {
            if (changes > 0)
            {
                projDefinition.Save(FilePath);
                projDefinition = null;
                FileStream arch = new FileStream(FilePath, FileMode.Open );
                projDefinition = XDocument.Load(arch);
                arch.Close();
            }
            changes = 0;            
        }

    }
}
