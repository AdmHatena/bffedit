#include <windows.h>
#include <windowsx.h>
#include <commctrl.h>

#define ID_EDIT 101

// ----- global ----- //

// インターフェイス
HINSTANCE hInst;

// アプリ名
TCHAR szAppName[] = TEXT("bffedit");

// ----- functions ----- //

// ウィンドプロシージャ
LRESULT CALLBACK WndProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam){

  // エディタボックス
  static HWND hEditBox;

  // buf
  wchar_t buffer[1024];

  // メッセージ処理
  switch(uMsg){
    case WM_CREATE:
      InitCommonControls();
      hEditBox = CreateWindowEx(
        0,
        "EDIT",
        "",
        WS_CHILD | WS_VISIBLE | WS_BORDER | ES_AUTOHSCROLL,
        10,
        10,
        200,
        200,
        hwnd,
        (HMENU)ID_EDIT,
        hInst,
        NULL
      );
      SendMessage(hEditBox, EM_SETLIMITTEXT, (WPARAM)63, 0);
      SendMessage(hEditBox, WM_CTLCOLORSTATIC, (WPARAM)_countof(buffer), (LPARAM)buffer);
    break;
    case WM_DESTROY:
      PostQuitMessage(0);
    break;
    default:
      return DefWindowProc(hwnd, uMsg, wParam, lParam);
    break;
  }

  return 0;
};

// main
int WINAPI WinMain(
  HINSTANCE hInstance,
  HINSTANCE hPrevInst,
  LPSTR lpszCmdLine,
  int nCmdShow
){
  WNDCLASS wc;
  HWND hwnd;

  // グローバル変数に格納
  hInst = hInstance;

  // ウィンドウクラスの属性設定
  wc.style         = CS_HREDRAW | CS_VREDRAW;
  wc.lpfnWndProc   = WndProc;
  wc.cbClsExtra    = 0;
  wc.cbWndExtra    = 0;
  wc.hInstance     = hInstance;
  wc.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
  wc.hCursor       = LoadCursor(NULL, IDC_ARROW);
  wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
  wc.lpszMenuName  = NULL;
  wc.lpszClassName = szAppName;

  // ウィンドウクラス登録
  if(!RegisterClass(&wc)) return 0;

  // ウィンドウ作成
  hwnd = CreateWindow(
    szAppName,
    TEXT("bffedit"),
    WS_OVERLAPPEDWINDOW,
    CW_USEDEFAULT,
    CW_USEDEFAULT,
    CW_USEDEFAULT,
    CW_USEDEFAULT,
    NULL,
    NULL,
    hInstance,
    NULL);
  
  if(!hwnd) return 0;

  // ウィンドウ表示
  ShowWindow(hwnd, nCmdShow);

  // デフォルトウィンドウサイズへ変更
  WINDOWINFO windowInfo;
  GetWindowInfo(hwnd, &windowInfo);
  SetWindowPos(
    hwnd,
    NULL,
    windowInfo.rcWindow.left,
    windowInfo.rcWindow.top,
    1000,
    600,
    SWP_SHOWWINDOW
  );

  // ウィンドウ再描画
  UpdateWindow(hwnd);
  
  // メッセージループ
  MSG msg;
  while(GetMessage(&msg, NULL, 0, 0) > 0){
    TranslateMessage(&msg);
    DispatchMessage(&msg);
  }

  return msg.wParam;
}
