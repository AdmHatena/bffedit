using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bffedit {
  public class bffeditMain : Form {
    private TextBox textBox1;
    public bffeditMain() {
        InitializeComponent();
    }
    private void InitializeComponent() {
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.SuspendLayout();
        // textBox
        this.textBox1.AcceptsReturn = true;
        this.textBox1.AcceptsTab = true;
        this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.textBox1.Multiline = true;
        this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        // Form
        this.ClientSize = new System.Drawing.Size(800, 480);
        this.Controls.Add(this.textBox1);
        this.Text = "bffedit";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    [STAThread]
    static void Main() {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new bffeditMain());
    }
  }
}
