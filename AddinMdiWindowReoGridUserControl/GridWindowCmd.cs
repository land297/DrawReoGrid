using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;

using System.IO;

namespace SIKABReoGridWindow
{
    class GridWindowCmd:Command
    {
        
        MdiWindow _mdiWindow;
        string _baseKey = "SIKABReoGridWindow.GridWindowCmd";
        string _windowKey = "GridWindowCmd";
        string _windowTitle = "GridWindowCmdTitle";
        UIUserControl _uiUserControl;
        public GridWindowCmd(IWindowManager windowManager)
        {
            base.Key = _baseKey;

            _uiUserControl = new UIUserControl();
            _uiUserControl._reoGridHost.Saved += _reoGridHost_Saved;
            _mdiWindow = windowManager.CreateMdiWindow(_windowKey, _windowTitle, _uiUserControl);

            _mdiWindow.Closing += _mdiWindow_Closing;

            this.ExecuteOnCheckedChange = false;

            _mdiWindow.Form.TopMost = false;

            //Console.WriteLine("MdiWindowCmd created for Gridx");
            
        }

        void _reoGridHost_Saved(ReoGridUserControl.EventData ed, EventArgs e)
        {
           //Console.WriteLine(ed.String);
           List<string> doc =  TranslateRGF.Translate(ed.String);
            //string sssss = System.IO.Path.GetTempPath();

           try
           {
               File.WriteAllLines(System.IO.Path.GetTempPath() + "grid.txt", doc.ToArray());

               string pmlVariable = "!!SIKABgridPath = '" + System.IO.Path.GetTempPath() + "grid.txt'";

               Aveva.Core.Utilities.CommandLine.Command.CreateCommand(pmlVariable).RunInPdms();
               Aveva.Core.Utilities.CommandLine.Command.CreateCommand("!!SIKABcreateGridNote()").RunInPdms();

               _uiUserControl._reoGridHost.SetStatusMessage("Status message");
           }
           catch
           { 
                string pmlVariable = "!!SIKABgridPath = 'fail'";
                Aveva.Core.Utilities.CommandLine.Command.CreateCommand(pmlVariable).RunInPdms();
           }


        }

        void _mdiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Checked = false;

            _mdiWindow.Hide();
            e.Cancel = true;
        }

        public override void Execute()
        {
            try
            {
                if (_mdiWindow.Visible)
                {
                    _mdiWindow.Hide();
                }
                else
                {
                    _mdiWindow.Show();
                    _mdiWindow.Float();
                }
                base.Execute();

            }
            catch (Exception)
            {                
                throw;
            }
        }
    }
}
