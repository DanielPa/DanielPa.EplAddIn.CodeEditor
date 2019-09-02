using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplSDK.WPF;

namespace DanielPa.EplAddIn.CodeEditor
{
  public class AddInModule : IEplAddIn
  {
    public bool OnRegister(ref bool bLoadOnStart)
    {
      bLoadOnStart = true;
      return true;
    }

    public bool OnUnregister()
    {
      return true;
    }

    public bool OnInit()
    {
      return true;
    }

    public bool OnInitGui()
    {
      DialogBarFactory dialogBarFactory = new DialogBarFactory("CodeEditor", typeof(CodeEditorControl), DialogDockingOptions.Any, 0);

      return true;
    }

    public bool OnExit()
    {
      return true;
    }
  }
}