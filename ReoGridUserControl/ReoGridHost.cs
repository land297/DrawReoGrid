using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using unvell.ReoGrid.Editor;
using unvell.ReoGrid;

namespace ReoGridUserControl
{
    public partial class ReoGridHost : UserControl
    {
        ReoGridEditor _editor;

        public delegate void SaveEvent(EventData ed, EventArgs e);
        public event SaveEvent Saved;
        public string SavePath { get; set; }
        private string _fileName = "gridxmloutput.xml";

        public ReoGridHost()
        {
            InitializeComponent();

            _editor = new ReoGridEditor();
            _editor.TopLevel = false;
            _editor.FormBorderStyle = FormBorderStyle.None;
            _editor.Visible = true;
            _editor.Location = new Point(0, 0);
            

            panel1.Controls.Add(_editor);

            this.Load += ReoGridHost_Load;

            SavePath = System.IO.Path.GetTempPath();


        }

        private void ReoGridHost_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
        }

        public void SetStatusMessage(string message)
        {
            toolStripLabel1.Text = message;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReoGridControl _reoGridControl = _editor.Grid;
            if (_reoGridControl.Save(SavePath + _fileName))
                 {
                if (Saved != null)
                {
                    EventData ed = new EventData() { String = SavePath + _fileName } ;
                    Saved(ed, new EventArgs());
                    
                }
                //send command to e3d/pdms
                }

        }
    }
    public class EventData
    {
        public string String { get; set; }
    }
}
