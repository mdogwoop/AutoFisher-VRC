using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace VRChatAutoFishing
{
    public static class ThemeUtils
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            if (IsWindows10OrGreater(17763))
            {
                int attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                int useImmersiveDarkMode = enabled ? 1 : 0;
                return DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
            }

            return false;
        }

        public static bool ApplyMica(IntPtr handle)
        {
            // 2 = DWMSBT_MAINWINDOW (Mica)
            // 3 = DWMSBT_TRANSIENTWINDOW (Acrylic)
            // 4 = DWMSBT_TABBEDWINDOW (Mica Alt)
            int backdropType = 2; 
            return DwmSetWindowAttribute(handle, DWMWA_SYSTEMBACKDROP_TYPE, ref backdropType, sizeof(int)) == 0;
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }
        
        public static void ApplyTheme(Form form)
        {
            UseImmersiveDarkMode(form.Handle, true);
            ApplyMica(form.Handle);
            
            // Fluent Design Typography
            form.Font = new System.Drawing.Font("Segoe UI Variable Display", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            if (form.Font.Name != "Segoe UI Variable Display") // Fallback
            {
                form.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            }

            // Dark Theme Colors
            form.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
            form.ForeColor = System.Drawing.Color.White;

            foreach (Control c in form.Controls)
            {
                ApplyThemeToControl(c);
            }
        }

        private static void ApplyThemeToControl(Control c)
        {
            if (c is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = System.Drawing.Color.FromArgb(45, 45, 45);
                btn.ForeColor = System.Drawing.Color.White;
                btn.Cursor = Cursors.Hand;
            }
            else if (c is TextBox txt)
            {
                txt.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
                txt.ForeColor = System.Drawing.Color.White;
                txt.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (c is Label lbl)
            {
                lbl.ForeColor = System.Drawing.Color.White;
            }
            else if (c is CheckBox chb)
            {
                chb.ForeColor = System.Drawing.Color.White;
            }
            else if (c is TrackBar trk)
            {
                // TrackBar hard to style in WinForms, assume default is okay or invisible background
                trk.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
            }
            else if (c is Panel pnl)
            {
                 pnl.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
                 pnl.ForeColor = System.Drawing.Color.White;
            }
            else if (c is LinkLabel lnk)
            {
                lnk.LinkColor = System.Drawing.Color.FromArgb(100, 149, 237); // CornflowerBlue
                lnk.ActiveLinkColor = System.Drawing.Color.FromArgb(30, 144, 255);
                lnk.VisitedLinkColor = System.Drawing.Color.FromArgb(100, 149, 237);
            }

            // Recursive for containers
            if (c.HasChildren)
            {
                foreach (Control child in c.Controls)
                {
                    ApplyThemeToControl(child);
                }
            }
        }
    }
}
