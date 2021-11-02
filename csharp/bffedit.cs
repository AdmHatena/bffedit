using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace bffedit {
  public class FontSettingDialog : Form {
    private FontFamily[] fonts;
    private Label fontLabel1, fontLabel2, fontLabel3;
    private ComboBox fontSelector;
    private NumericUpDown fontSizeSelector;
    private GroupBox fontSample;
    public FontSettingDialog(){
      this.Text = "フォントの変更";

      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.StartPosition = FormStartPosition.CenterParent;

      // ラベル - フォント名
      this.fontLabel1 = new Label();
      this.fontLabel1.Text = "フォント名";
      this.fontLabel1.Location = new Point(10, 10);
      this.fontLabel1.AutoSize = true;

      // インストール済フォントリストの取得
      InstalledFontCollection fts = new InstalledFontCollection();
      this.fonts = fts.Families;

      // フォント選択肢の作成
      this.fontSelector = new ComboBox();
      this.fontSelector.Location = new Point(10, 25);
      foreach(FontFamily ff in this.fonts){
        this.fontSelector.Items.Add(ff.Name);
      }

      // ラベル - フォントサイズ
      this.fontLabel2 = new Label();
      this.fontLabel2.Text = "サイズ";
      this.fontLabel2.Location = new Point(10, 55);
      this.fontLabel2.AutoSize = true;

      // フォントサイズ選択肢の作成
      this.fontSizeSelector = new NumericUpDown();
      this.fontSizeSelector.Location = new Point(10, 70);

      // フォントのサンプル表示
      this.fontSample = new GroupBox();
      this.fontSample.Width = 100;
      this.fontSample.Height = 100;
      this.fontSample.Location = new Point(170, 10);
      this.fontSample.Text = "サンプル";
      this.fontLabel3 = new Label();
      this.fontLabel3.AutoSize = true; // 複数行ラベル
      this.fontLabel3.Text = "BFFEdit\n\nBrain F*ck'n\nFast Editor";
      this.fontLabel3.Top = 25;
      this.fontLabel3.Left = 10;
      this.fontSample.Controls.Add(this.fontLabel3);

      this.SuspendLayout();
      this.Controls.AddRange(new Control[] {
        this.fontLabel1, this.fontSelector,     // フォント選択
        this.fontLabel2, this.fontSizeSelector, // フォントサイズ
        this.fontSample                         // フォントのサンプル表示
      });
      // this.Controls.Add(this.fontSelector);
      this.PerformLayout();
    }
  }
  public class bffeditMain : Form {
    private RichTextBox textBox1;

    // ----- menu bar ----- //
    private MenuStrip menu;
    private ToolStripMenuItem menuFile; // ファイル
    private ToolStripMenuItem menuFileOverwrite; // ファイル -> 上書き保存
    private ToolStripMenuItem menuFileSave; // ファイル -> 名前を付けて保存
    private ToolStripMenuItem menuFileOpen; // ファイル -> 開く
    private ToolStripMenuItem menuFileQuit; // ファイル -> 終了
    private ToolStripMenuItem menuFont; // フォント
    private ToolStripMenuItem menuFontType; // フォント -> フォント変更
    private ToolStripMenuItem menuHelp; // ヘルプ
    private ToolStripMenuItem menuHelpInfo; // ヘルプ -> バージョン情報

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

    private void callFontSettingsWindow(){
      FontSettingDialog dialog= new FontSettingDialog();
      dialog.ShowDialog();
    }
    public void selectedFontCommunication(string fontname){
      //
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

      this.menuFont = new ToolStripMenuItem{ Text = "書式" }; // [フォント]タブの追加
      this.menuFontType = new ToolStripMenuItem{ Text = "フォント変更" }; // [フォント変更]タブの追加
      this.menuFontType.Click += (s, e) => { this.callFontSettingsWindow(); }; // フォント変更ウィンドウ呼び出し
      this.menuFont.DropDownItems.AddRange(new ToolStripMenuItem[]{this.menuFontType});

      this.menu.Items.AddRange(new ToolStripMenuItem[]{this.menuFile, this.menuFont});

      // add textBox
      this.textBox1 = new System.Windows.Forms.RichTextBox();
      this.textBox1.TextChanged += (s, e) => { this.isTextEdited = true; };
      this.SuspendLayout();

      // textBox
      this.textBox1.AcceptsTab = true;
      this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBox1.SelectionFont = new Font(this.textBox1.Font.FontFamily, 10); // RichTextBoxでのフォント設定

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
