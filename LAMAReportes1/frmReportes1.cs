using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LAMAReportes1
{
    public partial class frmReportes1 : Form
    {
        public frmReportes1()
        {
            InitializeComponent();
        }

        private ReportClass rpt;

        public void SetReporte(ReportClass rpt)
        {
            this.rpt = rpt;
        }
        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.Show();
        }
    }
}
