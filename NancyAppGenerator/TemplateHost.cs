using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;

namespace NancyAppGenerator
{
    public class TemplateHost : ITextTemplatingEngineHost
    {

        public object GetHostOption(string optionName)
        {
            throw new NotImplementedException();
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            throw new NotImplementedException();
        }

        public void LogErrors(CompilerErrorCollection errors)
        {
            throw new NotImplementedException();
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            throw new NotImplementedException();
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            throw new NotImplementedException();
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            throw new NotImplementedException();
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            throw new NotImplementedException();
        }

        public string ResolvePath(string path)
        {
            throw new NotImplementedException();
        }

        public void SetFileExtension(string extension)
        {
            throw new NotImplementedException();
        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            throw new NotImplementedException();
        }

        public IList<string> StandardAssemblyReferences
        {
            get { throw new NotImplementedException(); }
        }

        public IList<string> StandardImports
        {
            get { throw new NotImplementedException(); }
        }

        public string TemplateFile
        {
            get { throw new NotImplementedException(); }
        }
    }
}
