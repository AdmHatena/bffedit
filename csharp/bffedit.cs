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
    private ToolStripMenuItem menuFileSave; // ファイル -> 保存
    private ToolStripMenuItem menuFileOpen; // ファイル -> 開く

    // 内容が変更されたかどうか
    bool isTextEdited = false;

    // 現在開いているファイルのパス
    // string filePath = null;

    private bool saveContents(bool overwrite = false){
      string content = this.textBox1.Text;
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.FileName = "untitled.txt";
      sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
      sfd.Filter = "テキストファイル (*.txt)|*.txt|すべてのファイル (*.*)|*.*";
      sfd.FilterIndex = 1;
      sfd.Title = "名前を付けて保存";
      sfd.RestoreDirectory = true;
      sfd.OverwritePrompt = true;
      sfd.CheckPathExists = true;
      if(sfd.ShowDialog() != DialogResult.OK) return false;
      System.IO.Stream stream = sfd.OpenFile();
      if(stream == null) return false;
      System.IO.StreamWriter sw = new System.IO.StreamWriter(stream);
      sw.Write(content);
      sw.Close();
      stream.Close();
      this.isTextEdited = false;
      return true;
    }

    private bool openContents(){
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.FileName = "";
      ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
      ofd.Filter = "テキストファイル (*.txt)|*.txt|すべてのファイル (*.*)|*.*";
      ofd.FilterIndex = 1;
      ofd.Title = "開く";
      ofd.RestoreDirectory = true;
      ofd.CheckFileExists = true;
      ofd.CheckPathExists = true;
      if(ofd.ShowDialog() != DialogResult.OK) return false;
      System.IO.Stream stream = ofd.OpenFile();
      if(stream == null) return false;
      System.IO.StreamReader sr = new System.IO.StreamReader(stream);
      string content = sr.ReadToEnd();
      sr.Close();
      stream.Close();
      this.textBox1.Text = content;
      return true;
    }

    private void quitApplication(){
      // 終了前に、保存されていない変更があった時に保存ダイアログが開くようにしたい
      Close(); // アプリケーションの終了
    }

    public bffeditMain() {
      InitializeComponent();
    }
    private void InitializeComponent() {
      // add menuBar
      this.menu = new MenuStrip();
      this.menuFile = new ToolStripMenuItem{ Text = "ファイル" }; // [ファイル]タブの追加
      this.menuFileSave = new ToolStripMenuItem{ Text = "保存" }; // [保存]項目の追加
      this.menuFileSave.Click += (s, e) => { this.saveContents(); }; // 内容を保存する
      this.menuFileOpen = new ToolStripMenuItem{ Text = "開く" }; // [保存]項目の追加
      this.menuFileOpen.Click += (s, e) => { this.openContents(); }; // 内容を保存する
      this.menuFileQuit = new ToolStripMenuItem{ Text = "終了" }; // [終了]項目の追加
      this.menuFileQuit.Click += (s, e) => { this.quitApplication(); }; // アプリケーションを終了する
      this.menuFile.DropDownItems.AddRange(new ToolStripMenuItem[]{this.menuFileSave, this.menuFileOpen, this.menuFileQuit});
      this.menu.Items.AddRange(new ToolStripMenuItem[]{this.menuFile});

      // add textBox
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox1.TextChanged += (s, e) => { this.isTextEdited = true; };
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
