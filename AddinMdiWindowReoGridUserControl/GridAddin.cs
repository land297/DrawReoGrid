using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;

namespace SIKABReoGridWindow
{
    public class GridAddin : IAddin
    {
        string _addinDescription = "AddinDescription";
        string _addinName = "AddinName";

        public string Description { get { return _addinDescription; } }

        public string Name { get {return _addinName; }}

        public void Start(ServiceManager serviceManager)
        {
            //implement
            IWindowManager windowManager = DependencyResolver.GetImplementationOf<IWindowManager>();
            ICommandManager commandManager = DependencyResolver.GetImplementationOf<ICommandManager>();
            
            //WindowManager windowManager = (WindowManager)serviceManager.GetService(typeof(WindowManager));
            //windowManager.CreateMdiWindow("SIKABReoGridWindow","GridAddin",)
            GridWindowCmd mdidWindowCmd = new GridWindowCmd(windowManager);

            //CommandManager commandManager = (CommandManager)serviceManager.GetService(typeof(CommandManager));

            //commandManager.Commands.Add(dockedWindowCmd);
            commandManager.Commands.Add(mdidWindowCmd);
        }

        public void Stop()
        { 
            //implement
        }

    }


}
