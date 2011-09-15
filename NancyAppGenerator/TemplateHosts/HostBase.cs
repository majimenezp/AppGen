using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;
using System.CodeDom.Compiler;
using Mono.TextTemplating;

namespace NancyAppGenerator.TemplateHosts
{
    public class HostBase : ITextTemplatingEngineHost
    {
        TemplatingEngine engine;
        Encoding encoding;

        CompilerErrorCollection errors = new CompilerErrorCollection();
        List<string> refs = new List<string>();
        List<string> imports = new List<string>();
        List<string> includePaths = new List<string>();
        List<string> referencePaths = new List<string>();

        //host properties for consumers to access
        public CompilerErrorCollection Errors { get { return errors; } }
        public List<string> Refs { get { return refs; } }
        public List<string> Imports { get { return imports; } }
        public List<string> IncludePaths { get { return includePaths; } }
        public List<string> ReferencePaths { get { return referencePaths; } }
        
        public HostBase()
        {
            refs.Add(typeof(TextTransformation).Assembly.Location);
            refs.Add(typeof(HostBase).Assembly.Location);
            encoding = Encoding.UTF8;
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = "";
            location = null;

            if (Path.IsPathRooted(requestFileName))
            {
                location = requestFileName;
            }
            else
            {
                foreach (string path in includePaths)
                {
                    string f = Path.Combine(path, requestFileName);
                    if (File.Exists(f))
                    {
                        location = f;
                        break;
                    }
                }
            }

            if (location == null)
                return false;

            try
            {
                content = System.IO.File.ReadAllText(location);
                return true;
            }
            catch (IOException ex)
            {
                AddError("Could not read included file '" + location + "':\n" + ex.ToString());
            }
            return false;
        }

        CompilerError AddError(string error)
        {
            CompilerError err = new CompilerError();
            err.ErrorText = error;
            Errors.Add(err);
            return err;
        }

        public virtual object GetHostOption(string optionName)
        {
            return null;
        }

        public virtual AppDomain ProvideTemplatingAppDomain(string content)
        {
            return null;
        }

        public void LogErrors(System.CodeDom.Compiler.CompilerErrorCollection errors)
        {
            this.errors.AddRange(errors);
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            return assemblyReference;
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            KeyValuePair<string, string> value;
            if (!directiveProcessors.TryGetValue(processorName, out value))
                throw new Exception(string.Format("No directive processor registered as '{0}'", processorName));
            var asmPath = ResolveAssemblyReference(value.Value);
            if (asmPath == null)
                throw new Exception(string.Format("Could not resolve assembly '{0}' for directive processor '{1}'", value.Value, processorName));
            var asm = System.Reflection.Assembly.LoadFrom(asmPath);
            return asm.GetType(value.Key, true);
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            var key = new ParameterKey(processorName, directiveId, parameterName);
            string value;
            if (parameters.TryGetValue(key, out value))
                return value;
            if (processorName != null || directiveId != null)
                return ResolveParameterValue(null, null, parameterName);
            return null;
        }

        Dictionary<ParameterKey, string> parameters = new Dictionary<ParameterKey, string>();

        Dictionary<string, KeyValuePair<string, string>> directiveProcessors = new Dictionary<string, KeyValuePair<string, string>>();

        public void AddDirectiveProcessor(string name, string klass, string assembly)
        {
            directiveProcessors.Add(name, new KeyValuePair<string, string>(klass, assembly));
        }

        public void AddParameter(string processorName, string directiveName, string parameterName, string value)
        {
            parameters.Add(new ParameterKey(processorName, directiveName, parameterName), value);
        }

        public string ResolvePath(string path)
        {

            return "";
        }

        public void SetFileExtension(string extension)
        {
            extension = extension.TrimStart('.');

        }

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            this.encoding = encoding;
        }

        public IList<string> StandardAssemblyReferences
        {
            get { return refs; }
        }

        public IList<string> StandardImports
        {
            get { return imports; }
        }

        public virtual string TemplateFile()
        {
            throw new Exception("Must Override the method for custom Template file calling");
        }
        

        protected TemplatingEngine Engine
        {
            get
            {
                if (engine == null)
                {
                    engine = new TemplatingEngine();
                }
                return engine;
            }
        }

        public  string ProcessTemplate()
        {
            string content = File.ReadAllText(TemplateFile());
            string textOutPut = Engine.ProcessTemplate(content, this);
            return textOutPut;
        }




        string ITextTemplatingEngineHost.TemplateFile
        {
            get { return TemplateFile(); }
        }
    }

}
