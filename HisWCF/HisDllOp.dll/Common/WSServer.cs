using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Services.Description;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

namespace MEDI.SIIM.SelfServiceWeb
{
    public class WSServer
    {
        static Dictionary<string, KeyValuePair<Type, object>> Motheds = new Dictionary<string, KeyValuePair<Type, object>>();
        public static object Call(string url, string mothedName, params object[] parms)
        {
            if (!Motheds.ContainsKey(url))
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
                    Motheds.Add(url, new KeyValuePair<Type,object>(t, o));
                }
                else
                {
                    throw new Exception(result.Errors[0].ErrorNumber + ":" + result.Errors[0].ErrorText);
                }
            }
            MethodInfo method = Motheds[url].Key.GetMethod(mothedName);
            return method.Invoke(Motheds[url].Value, parms);
        }
    }
}