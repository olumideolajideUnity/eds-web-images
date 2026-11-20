"""Draw a Unity 7 Windows editor context menu from a Unity menu item dump.
Since trunk PR #137579 (Editor/Platform/Windows/MenuThemeWin.cpp) Unity owner-draws native Windows menus in the editor
skin colours; draw() follows that code's colours and metrics (draw_win11_light() keeps the old stock-Windows look).
Unity shows the Scene view's right-click menu as a native OS menu, which Unity's capture can't see. The items,
order, enabled state and shortcuts come from Unity 7 itself (GameObjectToolContext.PopulateMenu, dumped by
unity/ct08_items.cs); only the OS chrome is drawn here.
Usage (as module): draw(items_txt, scale) -> RGBA image
"""
from PIL import Image, ImageDraw, ImageFilter, ImageFont
def _efds_font(n):
    # Windows fonts when present; elsewhere the bundled Inter (capture/fonts) or the system monospace font
    import os as _o
    w = "C:/Windows/Fonts/" + n
    if _o.path.exists(w): return w
    d = _o.path.dirname(_o.path.abspath(__file__))
    while not _o.path.isdir(_o.path.join(d, "fonts")) and _o.path.dirname(d) != d: d = _o.path.dirname(d)
    mono = [p for p in ("/System/Library/Fonts/Menlo.ttc", "/usr/share/fonts/truetype/dejavu/DejaVuSansMono-Bold.ttf") if _o.path.exists(p)]
    if n.startswith("consola") and mono: return mono[0]
    return _o.path.join(d, "fonts", "Inter-SemiBold.ttf" if n in ("segoeuib.ttf", "seguisb.ttf", "consolab.ttf") else "Inter-Regular.ttf")

def shortcut(code):
    """Unity menu shortcut codes -> Windows labels: % Ctrl, # Shift, & Alt, _ = no modifier."""
    if not code: return ""
    keys, mods = code, []
    while keys and keys[0] in "%#&_":
        c = keys[0]; keys = keys[1:]
        if c == "%": mods.append("Ctrl")
        elif c == "&": mods.append("Alt")
        elif c == "#": mods.append("Shift")
    order = [m for m in ("Ctrl", "Alt", "Shift") if m in mods]
    k = {"DEL": "Del", "BACKSPACE": "Backspace"}.get(keys.upper(), keys.upper())
    return "+".join(order + [k])

def top_level(path):
    rows, seen = [], set()
    for line in open(path, encoding="utf-8").read().splitlines():
        name, status = line.rsplit("|", 1)
        if name == "---":
            if status == "" and rows and rows[-1][0] != "---": rows.append(("---", "", "", False, False))
            continue
        if "/" in name:
            head = name.split("/")[0]
            if head not in seen: seen.add(head); rows.append((head, "", ">", True, False))
            continue
        last = name.rsplit(" ", 1)
        if len(last) == 2 and last[1] and last[1][0] in "%#&_": label, sc = last
        else: label, sc = name, ""
        rows.append((label, shortcut(sc), "", status != "Disabled", status == "Checked"))
    while rows and rows[-1][0] == "---": rows.pop()
    return rows

def draw_win11_light(items_txt, k=2.5):
    rows = top_level(items_txt)
    f = ImageFont.truetype(_efds_font("segoeui.ttf"), round(12 * k))
    ITEM, SEP, PADV, W = round(30 * k), round(9 * k), round(4 * k), round(330 * k)
    H = PADV * 2 + sum(SEP if r[0] == "---" else ITEM for r in rows)
    pad = round(16 * k)
    img = Image.new("RGBA", (W + 2 * pad, H + 2 * pad), (0, 0, 0, 0))
    sh = Image.new("L", img.size, 0)
    ImageDraw.Draw(sh).rounded_rectangle([pad, pad + round(3 * k), pad + W, pad + H + round(3 * k)], round(8 * k), fill=95)
    img.putalpha(sh.filter(ImageFilter.GaussianBlur(round(7 * k))))
    img = Image.composite(Image.new("RGBA", img.size, (0, 0, 0, 255)), Image.new("RGBA", img.size, (0, 0, 0, 0)), img.getchannel("A"))
    d = ImageDraw.Draw(img)
    d.rounded_rectangle([pad, pad, pad + W, pad + H], round(8 * k), fill=(249, 249, 249, 255), outline=(220, 220, 220, 255), width=max(1, round(k * 0.6)))
    y = pad + PADV
    for label, sc, sub, enabled, *_ in rows:
        if label == "---":
            d.line([pad + round(4 * k), y + SEP // 2, pad + W - round(4 * k), y + SEP // 2], fill=(226, 226, 226, 255), width=max(1, round(k * 0.6)))
            y += SEP; continue
        col = (26, 26, 26, 255) if enabled else (160, 160, 160, 255)
        d.text((pad + round(28 * k), y + ITEM / 2), label, font=f, fill=col, anchor="lm")
        if sc: d.text((pad + W - round(17 * k), y + ITEM / 2), sc, font=f, fill=col, anchor="rm")
        if sub:
            cx, cy, s = pad + W - round(14 * k), y + ITEM / 2, round(3.2 * k)
            d.line([cx - s / 2, cy - s, cx + s / 2, cy, cx - s / 2, cy + s], fill=col, width=max(1, round(k * 0.55)), joint="curve")
        y += ITEM
    img.info["rows"] = rows_at          # top y (image px) of each row, e.g. to line a submenu up with its parent item
    img.info["frame"] = FRAME
    return img, pad


def draw(items_txt, k=2.5, bg=(20, 20, 20), text=(210, 210, 210), hot=None, highlight=(60, 60, 60), min_width=0, hot_outline=False):
    """Unity owner-drawn menu (MenuThemeWin.cpp). Colours come from the editor theme variables
    --unity-colors-app_toolbar-background / --unity-colors-default-text (read live: 20,20,20 / 210,210,210 dark).
    k = device pixels per 96-dpi pixel (2.5 at 250%).
    hot = label of the row under the pointer: filled with --unity-colors-app_toolbar_button-background-hover (#3C3C3C dark, read
    live; ContainerWindow.TryGetNativeThemeColors) as a rounded rect inset kPopupHighlightInset 3 with kPopupHighlightRadius 6.
    min_width = minimum popup width in DIP (a dropdown's menu is at least as wide as the field)."""
    blend = lambda a, b, n: tuple(x + (y - x) * n // 255 for x, y in zip(a, b))
    disabled, sep = blend(text, bg, 128), blend(bg, text, 48)          # kDisabledTextBlend, kSeparatorBlend
    rows = top_level(items_txt)
    f = ImageFont.truetype(_efds_font("segoeui.ttf"), round(12 * k))   # lfMenuFont: Segoe UI 9pt
    S = lambda v: round(v * k)
    ITEM, SEPH, FRAME = S(22), S(9), S(3)                               # check 16 + margins 3+3; separator 5 + 2+2
    INSET, GUTTER, SUBCOL, GAP, ARROW = S(3), S(32), S(20), S(20), S(23)
    avg = round(sum(f.getlength(c) for c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz") / 52)
    lw = max(f.getlength(r[0]) for r in rows if r[0] != "---"); aw = max([f.getlength(r[1]) for r in rows if r[1]] or [0])
    W = max(round(2 * INSET + GUTTER + lw + SUBCOL + GAP * (1 if aw else 0) + aw + 2 * avg) + 2 * FRAME, S(min_width))
    H = sum(SEPH if r[0] == "---" else ITEM for r in rows) + 2 * FRAME
    pad = S(16)
    img = Image.new("RGBA", (W + 2 * pad, H + 2 * pad), (0, 0, 0, 0))
    sh = Image.new("L", img.size, 0)                                   # DWM popup shadow
    ImageDraw.Draw(sh).rounded_rectangle([pad, pad + S(2), pad + W, pad + H + S(2)], S(4), fill=110)
    img = Image.composite(Image.new("RGBA", img.size, (0, 0, 0, 255)), img, sh.filter(ImageFilter.GaussianBlur(S(6))))
    d = ImageDraw.Draw(img)
    # whole window (frame too) in the menu background; DWM small rounded corners + border in the separator colour
    d.rounded_rectangle([pad, pad, pad + W - 1, pad + H - 1], S(4), fill=bg + (255,), outline=sep + (255,), width=max(1, round(k)))
    y = pad + FRAME; x0 = pad + FRAME; x1 = pad + W - FRAME
    rows_at = {}
    for label, sc, sub, enabled, checked in rows:
        if label == "---":
            ly = y + SEPH // 2
            d.rectangle([x0 + S(6), ly, x1 - S(6) - 1, ly + max(1, round(k)) - 1], fill=sep + (255,))
            y += SEPH; continue
        col = (text if enabled else disabled) + (255,)
        rl, rr = x0 + INSET, x1 - INSET
        rows_at[label] = y
        if hot is not None and label == hot:
            if hot_outline:   # selected item whose submenu is open: outline in the text colour instead of a fill (FrameRoundRect)
                d.rounded_rectangle([rl, y, rr - 1, y + ITEM - 1], S(6), outline=tuple(text) + (255,), width=max(1, round(k)))
            else:
                d.rounded_rectangle([rl, y, rr - 1, y + ITEM - 1], S(6), fill=tuple(highlight) + (255,))
        d.text((rl + GUTTER, y + ITEM / 2), label, font=f, fill=col, anchor="lm")
        if sc: d.text((rr - SUBCOL, y + ITEM / 2), sc, font=f, fill=col, anchor="rm")
        if checked:                                                     # kCheckGlyph in the 26 DIP check column
            gx, gy = rl + (S(26) - S(10)) / 2, y + (ITEM - S(10)) / 2
            d.line([(gx + px * k, gy + py * k) for px, py in ((1.5, 5.5), (4.0, 7.5), (8.5, 3.0))], fill=col, width=max(1, round(1.25 * k)), joint="curve")
        if sub:                                                         # kSubmenuGlyph, 10 DIP square, 1.25 stroke
            gx, gy = rr - ARROW + (ARROW - S(10)) / 2, y + (ITEM - S(10)) / 2
            pts = [(gx + px * k, gy + py * k) for px, py in ((3.5, 1.5), (7.0, 5.0), (3.5, 8.5))]
            d.line(pts, fill=col, width=max(1, round(1.25 * k)), joint="curve")
        y += ITEM
    img.info["rows"] = rows_at          # top y (image px) of each row, e.g. to line a submenu up with its parent item
    img.info["frame"] = FRAME
    return img, pad
