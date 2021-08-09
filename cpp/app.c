#include <windows.h>

int WINAPI WinMain(
  HINSTANCE hInst,     // 
  HINSTANCE hPrevInst, // 
  LPSTR ipCmdLine,     // 
  int nCmdShow         // 
){

  // application name
  TCHAR szAppName[] = TEXT("bffedit");
  WNDCLASS wc;
  HWND hwnd;

  // set the attributes of the window class
  wc.style         = CS_HREDRAW | CS_VREDRAW;
  wc.lpfnWndProc   = DefWindowProc;
  wc.cbClsExtra    = 0;
  wc.cbWndExtra    = 0;
  wc.hInstance     = hInst;
  wc.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
  wc.hCursor       = LoadCursor(NULL, IDC_ARROW);
  wc.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
  wc.lpszMenuName  = NULL;
  wc.lpszClassName = szAppName;

  // register the window class
  if(!RegisterClass(&wc)) return 0;

  // create window
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
    hInst,
    NULL
  );
  if(!hwnd) return 0;

  // show the window
  ShowWindow(hwnd, nCmdShow);

  // redraw the window
  UpdateWindow(hwnd);

  // msgbox
  MessageBox(hwnd, TEXT("create window"), TEXT(""), MB_OK);

  return 0;
}
