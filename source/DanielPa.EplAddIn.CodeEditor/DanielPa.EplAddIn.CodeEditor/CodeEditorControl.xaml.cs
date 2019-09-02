using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Eplan.EplApi.Base;
using Microsoft.CSharp;

namespace DanielPa.EplAddIn.CodeEditor
{
  /// <summary>
  /// Interaction logic for CodeEditorControl.xaml
  /// </summary>
  public partial class CodeEditorControl : UserControl
  {
    public CodeEditorControl()
    {
      InitializeComponent();
      List<Uri> references = new List<Uri>();
      string binPath = PathMap.SubstitutePath("$(BIN)");
      references.Add(new Uri($@"{binPath}\Eplan.EplApi.AFu.dll", UriKind.Absolute));
      references.Add(new Uri($@"{binPath}\Eplan.EplApi.Baseu.dll", UriKind.Absolute));
      references.Add(new Uri($@"{binPath}\Eplan.EplApi.DataModelu.dll", UriKind.Absolute));
      references.Add(new Uri($@"{binPath}\Eplan.EplApi.EServicesu.dll", UriKind.Absolute));
      references.Add(new Uri($@"{binPath}\Eplan.EplApi.Guiu.dll", UriKind.Absolute));
      references.Add(new Uri($@"{binPath}\Eplan.EplApi.HEServicesu.dll", UriKind.Absolute));
      references.Add(new Uri($@"{binPath}\Eplan.EplApi.MasterDatau.dll", UriKind.Absolute));
      references.Add(new Uri($@"{binPath}\Eplan.EplApi.Starteru.dll", UriKind.Absolute));

      editControl1.AssemblyReferences = references;
      editControl1.Text = SetCodeFrame();
    }

    private void Execute_OnClick(object sender, RoutedEventArgs e)
    {
      var results = CompileAssembly(editControl1.Text);

      if (!results.Errors.HasErrors)
      {
        InvokeMethod(results.CompiledAssembly);
      }
      else
      {
        foreach (CompilerError compilerError in results.Errors)
        {
          var baseException = new BaseException(compilerError.ErrorText, MessageLevel.Error);
          baseException.FixMessage();
        }
      }
    }

    private CompilerResults CompileAssembly(string source)
    {
      CSharpCodeProvider codeProvider = new CSharpCodeProvider();
      CompilerParameters parameters = new CompilerParameters();
      parameters.GenerateExecutable = false;
      parameters.ReferencedAssemblies.Add("System.dll");
      parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
      parameters.ReferencedAssemblies.Add("mscorlib.dll");
      parameters.ReferencedAssemblies.Add("Eplan.EplApi.Starteru.dll");
      parameters.ReferencedAssemblies.Add("Eplan.EplApi.Baseu.dll");
      parameters.ReferencedAssemblies.Add("Eplan.EplApi.AFu.dll");
      parameters.ReferencedAssemblies.Add("Eplan.EplApi.DataModelu.dll");
      parameters.ReferencedAssemblies.Add("Eplan.EplApi.HEServicesu.dll");
      CompilerResults compilerResults = codeProvider.CompileAssemblyFromSource(parameters, source);
      return compilerResults;
    }

    private void InvokeMethod(Assembly assembly)
    {
      Object obj = assembly.CreateInstance("Foo.Bar");
      MethodInfo execute = assembly.GetType("Foo.Bar").GetMethod("Execute");
      try
      {
        execute.Invoke(obj, null);
      }
      catch (Exception exception)
      {
        if (exception.InnerException != null)
        {
          var baseException = new BaseException(exception.InnerException.Message, MessageLevel.Error);
          baseException.FixMessage();
        }
        else
        {
          var baseException = new BaseException(exception.Message, MessageLevel.Error);
          baseException.FixMessage();
        }
      }
    }

    private string SetCodeFrame()
    {
      return "using System;\n" +
             "using System.Windows.Forms;\n" +
             "using Eplan.EplApi.DataModel;\n" +
             "using Eplan.EplApi.HEServices;\n" +
             "\n" +
             "namespace Foo\n" +
             "{\n" +
             "  public class Bar\n" +
             "  {\n" +
             "    public void Execute()\n" +
             "    {\n" +
             "      using (LockingStep oLS = new LockingStep())\n" +
             "      {\n" +
             "\n" +
             "        //START YOUR CODE HERE\n" +
             "\n" +
             "        //EXAMPLE:\n" +
             "        //var prj = new SelectionSet().GetCurrentProject(true);\n" +
             "        //var pageName = prj.Pages[0].Name;\n" +
             "        //MessageBox.Show(pageName);\n" +
             "      }\n" +
             "    }\n" + 
             "  }\n" +
             "}";
    }
  }
}