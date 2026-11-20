"""Render Windows visual-style parts (e.g. DarkMode_Explorer::Button push buttons / checkboxes) to PNGs with uxtheme's own
DrawThemeBackground, so themed controls match what Windows draws. Usage: render(cls, part, state, w, h, bg) -> PIL RGB image."""
import ctypes, ctypes.wintypes as wt
from PIL import Image
ux, gdi, user = ctypes.windll.uxtheme, ctypes.windll.gdi32, ctypes.windll.user32
user.SetProcessDpiAwarenessContext(ctypes.c_void_p(-4))
ux.OpenThemeDataForDpi.restype = ctypes.c_void_p; ux.OpenThemeDataForDpi.argtypes = [wt.HWND, wt.LPCWSTR, wt.UINT]
ux.DrawThemeBackground.argtypes = [ctypes.c_void_p, ctypes.c_void_p, ctypes.c_int, ctypes.c_int, ctypes.POINTER(wt.RECT), ctypes.c_void_p]
ux.GetThemeColor.argtypes = [ctypes.c_void_p, ctypes.c_int, ctypes.c_int, ctypes.c_int, ctypes.POINTER(wt.DWORD)]
for f in ('CreateCompatibleDC', 'CreateCompatibleBitmap', 'CreateSolidBrush'): getattr(gdi, f).restype = ctypes.c_void_p
gdi.CreateCompatibleDC.argtypes = [ctypes.c_void_p]; gdi.CreateCompatibleBitmap.argtypes = [ctypes.c_void_p, ctypes.c_int, ctypes.c_int]
gdi.GetDIBits.argtypes = [ctypes.c_void_p, ctypes.c_void_p, wt.UINT, wt.UINT, ctypes.c_void_p, ctypes.c_void_p, wt.UINT]
gdi.SelectObject.argtypes = [ctypes.c_void_p, ctypes.c_void_p]
user.GetDC.restype = ctypes.c_void_p; user.FillRect.argtypes = [ctypes.c_void_p, ctypes.POINTER(wt.RECT), ctypes.c_void_p]
class BMI(ctypes.Structure):
    _fields_ = [("biSize", wt.DWORD), ("biWidth", ctypes.c_long), ("biHeight", ctypes.c_long), ("biPlanes", wt.WORD), ("biBitCount", wt.WORD),
                ("biCompression", wt.DWORD), ("biSizeImage", wt.DWORD), ("a", ctypes.c_long), ("b", ctypes.c_long), ("c", wt.DWORD), ("d", wt.DWORD)]
def render(cls, part, state, w, h, bg=(20, 20, 20), dpi=96):
    th = ux.OpenThemeDataForDpi(None, cls, dpi)
    if not th: raise RuntimeError("no theme " + cls)
    sdc = user.GetDC(None); dc = gdi.CreateCompatibleDC(sdc); bmp = gdi.CreateCompatibleBitmap(sdc, w, h); gdi.SelectObject(dc, bmp)
    r = wt.RECT(0, 0, w, h); br = gdi.CreateSolidBrush(bg[0] | bg[1] << 8 | bg[2] << 16); user.FillRect(dc, ctypes.byref(r), br)
    ux.DrawThemeBackground(th, dc, part, state, ctypes.byref(r), None)
    bi = BMI(ctypes.sizeof(BMI), w, -h, 1, 32, 0, 0, 0, 0, 0, 0); buf = ctypes.create_string_buffer(w * h * 4)
    gdi.GetDIBits(dc, bmp, 0, h, buf, ctypes.byref(bi), 0)
    return Image.frombuffer("RGBA", (w, h), buf.raw, "raw", "BGRA", 0, 1).convert("RGB")
def color(cls, part, state, prop=3803):   # TMT_TEXTCOLOR
    th = ux.OpenThemeDataForDpi(None, cls, 96); c = wt.DWORD()
    ok = ux.GetThemeColor(th, part, state, prop, ctypes.byref(c)) == 0
    return (c.value & 255, c.value >> 8 & 255, c.value >> 16 & 255) if ok else None
