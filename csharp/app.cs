using System;
using System.Windows.Forms;

namespace bffedit{
  public class bffeditMain : Form{
    static void Main(){
      Application.Run(new bffeditMain());
    }

    bffeditMain(){
      this.Width = 1280;
      this.Height = 720;
    }
  }
}
