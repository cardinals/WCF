using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Net;
using System.IO;

namespace Common.WSCall
{
    public class WSServer
    {
        public static object Call<Entity>(string url, params object[] parms)
            where Entity : WSEntity.WSEntityBody
        {
            WebClient web = new WebClient();
            Stream stream = web.OpenRead(url + "?WSDL");
            ServiceDescription description = ServiceDescription.Read(stream);
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();
            importer.ProtocolName = "Soap";
            importer.Style = ServiceDescriptionImportStyle.Client;
            importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
            importer.AddServiceDescription(description, null, null);
            CodeNamespace nmspace = new CodeNamespace();
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(nmspace);
            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameter = new CompilerParameters();
            parameter.GenerateExecutable = false;
            parameter.GenerateInMemory = true;
            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.XML.dll");
            parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameter.ReferencedAssemblies.Add("System.Data.dll");
            CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
            if (!result.Errors.HasErrors)
            {
                Assembly asm = result.CompiledAssembly;
                Type t = asm.GetType("WebService");
                object o = Activator.CreateInstance(t);
                MethodInfo method = t.GetMethod(typeof(Entity).Name);
                return method.Invoke(o, parms);
            }
            return null;
        }
    }
}
