using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace VRChatAutoFishing
{
    public static class ThemeUtils
    {
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

        public static bool IsSystemDarkTheme()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        object val = key.GetValue("AppsUseLightTheme");
                        if (val is int i)
                        {
                            return i == 0;
                        }
                    }
                }
            }
            catch { }
            return true; // Default to Dark
        }

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
            bool isDark = IsSystemDarkTheme();
            UseImmersiveDarkMode(form.Handle, isDark);
            ApplyMica(form.Handle);
            
            // Fluent Design Typography
            form.Font = new Font("Segoe UI Variable Display", 9F, FontStyle.Regular, GraphicsUnit.Point);
            if (form.Font.Name != "Segoe UI Variable Display") // Fallback
            {
                form.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            }

            if (isDark)
            {
                // Dark Theme Colors
                form.BackColor = Color.FromArgb(32, 32, 32);
                form.ForeColor = Color.White;
            }
            else
            {
                // Light Theme Colors
                form.BackColor = Color.FromArgb(243, 243, 243); // Mica-like fallback for Light
                form.ForeColor = Color.Black;
            }

            foreach (Control c in form.Controls)
            {
                ApplyThemeToControl(c, isDark);
            }
        }

        private static void ApplyThemeToControl(Control c, bool isDark)
        {
            if (c is Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                if (isDark)
                {
                    btn.BackColor = Color.FromArgb(45, 45, 45);
                    btn.ForeColor = Color.White;
                }
                else
                {
                    btn.BackColor = Color.FromArgb(251, 251, 251);
                    btn.ForeColor = Color.Black;
                    // Add a subtle border for light mode buttons if needed, or keep flat
                }
            }
            else if (c is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                if (isDark)
                {
                    txt.BackColor = Color.FromArgb(40, 40, 40);
                    txt.ForeColor = Color.White;
                }
                else
                {
                    txt.BackColor = Color.White;
                    txt.ForeColor = Color.Black;
                }
            }
            else if (c is Label lbl)
            {
                lbl.ForeColor = isDark ? Color.White : Color.Black;
            }
            else if (c is CheckBox chb)
            {
                chb.ForeColor = isDark ? Color.White : Color.Black;
            }
            else if (c is TrackBar trk)
            {
                trk.BackColor = isDark ? Color.FromArgb(32, 32, 32) : Color.FromArgb(243, 243, 243);
            }
            else if (c is Panel pnl)
            {
                 pnl.BackColor = isDark ? Color.FromArgb(32, 32, 32) : Color.FromArgb(243, 243, 243);
                 pnl.ForeColor = isDark ? Color.White : Color.Black;
            }
            else if (c is LinkLabel lnk)
            {
                if (isDark)
                {
                    lnk.LinkColor = Color.FromArgb(100, 149, 237); // CornflowerBlue
                    lnk.ActiveLinkColor = Color.FromArgb(30, 144, 255);
                    lnk.VisitedLinkColor = Color.FromArgb(100, 149, 237);
                }
                else
                {
                    lnk.LinkColor = Color.FromArgb(0, 102, 204);
                    lnk.ActiveLinkColor = Color.FromArgb(0, 51, 153);
                    lnk.VisitedLinkColor = Color.FromArgb(0, 102, 204);
                }
            }

            // Recursive for containers
            if (c.HasChildren)
            {
                foreach (Control child in c.Controls)
                {
                    ApplyThemeToControl(child, isDark);
                }
            }
        }
    }
}
