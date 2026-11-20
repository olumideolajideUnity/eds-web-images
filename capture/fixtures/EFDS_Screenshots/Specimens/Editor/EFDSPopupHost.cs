using UnityEditor;
using UnityEngine;

namespace EFDS
{
    // Hosts any PopupWindowContent in a normal utility window so it stays open while it's captured
    // (real popups close as soon as Unity loses focus). The content draws exactly as it would in its popup.
    public class EFDSPopupHost : EditorWindow
    {
        public PopupWindowContent content;

        public static EFDSPopupHost Open(PopupWindowContent c)
        {
            foreach (var w in Resources.FindObjectsOfTypeAll<EFDSPopupHost>()) w.Close();
            var host = CreateInstance<EFDSPopupHost>();
            host.titleContent = new GUIContent("EFDSPopupHost");
            host.content = c;
            var size = c.GetWindowSize();
            host.ShowUtility();
            host.position = new Rect(900, 300, size.x, size.y);
            c.OnOpen();
            return host;
        }

        bool m_HasUITK;

        // UI Toolkit popup content (PopupWindowContent.CreateGUI) is hosted as the window's visual tree.
        void CreateGUI()
        {
            if (content == null) return;
            var ve = content.CreateGUI();
            if (ve == null) return;
            m_HasUITK = true;
            ve.style.flexGrow = 1;
            rootVisualElement.Add(ve);
        }

        void OnGUI()
        {
            if (content == null || m_HasUITK) return;
            content.OnGUI(new Rect(0, 0, position.width, position.height));
            Repaint();
        }
    }
}
