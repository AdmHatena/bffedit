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
    private ToolStripMenuItem menuFileOverwrite; // ファイル -> 上書き保存
    private ToolStripMenuItem menuFileSave; // ファイル -> 名前を付けて保存
    private ToolStripMenuItem menuFileOpen; // ファイル -> 開く
    private ToolStripMenuItem menuFileQuit; // ファイル -> 終了

    // 内容が変更されたかどうか
    bool isTextEdited = false;

    // 現在開いているファイルのパス
    string filePath = null;

    private bool saveContents(bool overwrite = false){
      // 「上書き保存」モードのときに開いてるファイルが存在しない場合、「名前を付けて保存」モードに変更
      if(overwrite & this.filePath == null) overwrite = false;

      if(!overwrite){
        // 「名前を付けて保存」ダイアログの作成
        SaveFileDialog sfd = new SaveFileDialog();
        sfd.FileName = "untitled.txt";
        sfd.InitialDirectory = this.filePath == null ? System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) : System.IO.Path.GetDirectoryName(this.filePath);
        sfd.Filter = "テキストファイル (*.txt)|*.txt|すべてのファイル (*.*)|*.*";
        sfd.FilterIndex = 1;
        sfd.Title = "名前を付けて保存";
        sfd.RestoreDirectory = true;
        sfd.OverwritePrompt = true;
        sfd.CheckPathExists = true;

        // 実際にダイアログを出す
        if(sfd.ShowDialog() != DialogResult.OK) return false;
        // ファイルストリームの作成
        System.IO.Stream stream = sfd.OpenFile();
        if(stream == null) return false;
        System.IO.StreamWriter sw = new System.IO.StreamWriter(stream);

        // 内容の書き込み
        sw.Write(this.textBox1.Text);

        // ファイルストリームを閉じる
        sw.Close();
        stream.Close();

        // 現在開いているファイル名を記録
        this.filePath = sfd.FileName;
      } else{
        System.IO.File.WriteAllText(this.filePath, this.textBox1.Text);
      }

      // 内容変更フラグをリセット
      this.isTextEdited = false;
      return true;
    }

    private bool openContents(){
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.FileName = "";
      ofd.InitialDirectory = this.filePath == null ? System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) : System.IO.Path.GetDirectoryName(this.filePath);
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
      this.filePath = ofd.FileName;
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
      this.menuFileOpen = new ToolStripMenuItem{ Text = "開く" }; // [開く]項目の追加
      this.menuFileOpen.Click += (s, e) => { this.openContents(); }; // 内容を保存する
      this.menuFileOpen.ShortcutKeys = Keys.Control | Keys.O; // Ctrl + O
      this.menuFileOverwrite = new ToolStripMenuItem{ Text = "上書き保存" }; // [上書き保存]項目の追加
      this.menuFileOverwrite.Click += (s, e) => { this.saveContents(true); }; // 内容を上書き保存する
      this.menuFileOverwrite.ShortcutKeys = Keys.Control | Keys.S; // Ctrl + S
      this.menuFileSave = new ToolStripMenuItem{ Text = "名前を付けて保存" }; // [名前を付けて保存]項目の追加
      this.menuFileSave.Click += (s, e) => { this.saveContents(); }; // 内容を保存する
      this.menuFileSave.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S; // Ctrl + Shift + S
      this.menuFileQuit = new ToolStripMenuItem{ Text = "終了" }; // [終了]項目の追加
      this.menuFileQuit.Click += (s, e) => { this.quitApplication(); }; // アプリケーションを終了する
      this.menuFile.DropDownItems.AddRange(new ToolStripMenuItem[]{this.menuFileOpen, this.menuFileOverwrite, this.menuFileSave, this.menuFileQuit});
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
