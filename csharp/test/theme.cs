using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public class App : Form {
  [DllImport("dwmapi.dll")]
  private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
  protected override void OnHandleCreated(EventArgs e){
    if(DwmSetWindowAttribute(Handle, 19, new[]{1}, 4) != 0) DwmSetWindowAttribute(Handle, 20, new[]{1}, 4);
  }
  App(){
    // constructor
    this.init();
  }
  private void init(){
    this.ClientSize = new Size(800, 400);
    this.Text = "dark theme test";
    this.ResumeLayout(false);
    this.PerformLayout();
  }
  private void quit(){
    Close();
  }
  
  [STAThread]
  static void Main(){
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new App());
  }
};
