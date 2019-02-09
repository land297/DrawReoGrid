using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ReoGridUserControl;

namespace SIKABReoGridWindow
{
    public partial class UIUserControl : UserControl
    {
        public ReoGridHost _reoGridHost;
        public UIUserControl()
        {
            InitializeComponent();

            _reoGridHost = new ReoGridHost();

            this.Controls.Add(_reoGridHost);

            //trams
           
        }

    }
}
