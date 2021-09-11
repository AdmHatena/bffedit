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

    // ----- menu bar ----- //
    private MenuStrip menu;
    private ToolStripMenuItem menuFile; // ファイル
    private ToolStripMenuItem menuFileQuit; // ファイル -> 終了

    public bffeditMain() {
      InitializeComponent();
    }
    private void InitializeComponent() {
      // add menuBar
      this.menu = new MenuStrip();
      this.menuFile = new ToolStripMenuItem{ Text = "ファイル" }; // [ファイル]タブの追加
      this.menuFileQuit = new ToolStripMenuItem{ Text = "終了" }; // [終了]項目の追加
      this.menuFileQuit.Click += (o, e) => Close(); // 終了を押したときにアプリケーションを終了する
      this.menuFile.DropDownItems.AddRange(new ToolStripMenuItem[]{menuFileQuit});
      this.menu.Items.AddRange(new ToolStripMenuItem[]{menuFile});

      // add textBox
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
      this.Controls.Add(this.menu);
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
