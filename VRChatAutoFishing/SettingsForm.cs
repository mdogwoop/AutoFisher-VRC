using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRChatAutoFishing
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public void SaveSettingsToFile(AppSettings overrides)
        {
            var appSettings = new AppSettings
            {
                castingTime = overrides.castingTime ?? AppSettings.DefaultCastingTime,
                WebhookSettings = overrides.WebhookSettings ?? webhookNotificationSettings.SaveSettings()
            };
            string settingsJson = JsonSerializer.Serialize(appSettings);
            var localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoFisherVRC.json");
            var userProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AutoFisherVRC.json");

            try
            {
                File.WriteAllText(localPath, settingsJson);
                return;
            }
            catch (Exception)
            {
                // 流下去，尝试写入用户目录
            }

            try
            {
                File.WriteAllText(userProfilePath, settingsJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法保存设置（这不会影响当前设置，但是设置项将在下次启动程序时还原）: {ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public AppSettings InitializeSavedValues()
        {
            var localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AutoFisherVRC.json");
            var userProfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AutoFisherVRC.json");
            string? settingsJson = null;

            try
            {
                if (File.Exists(localPath))
                {
                    settingsJson = File.ReadAllText(localPath);
                    return DoLoadAppSettings(settingsJson) ?? new AppSettings();
                }
            }
            catch (Exception) { }

            try
            {
                if (File.Exists(userProfilePath))
                {
                    settingsJson = File.ReadAllText(userProfilePath);
                    return DoLoadAppSettings(settingsJson) ?? new AppSettings();
                }
            }
            catch (Exception) { }
            return new AppSettings();
        }

        private AppSettings? DoLoadAppSettings(string settingsJson)
        {
            AppSettings? appSettings = JsonSerializer.Deserialize<AppSettings>(settingsJson ?? "");
            if (appSettings == null)
                return null;

            webhookNotificationSettings.LoadSettings(appSettings.WebhookSettings);
            return appSettings;
        }

        public Settings GetSettings()
        {
            Settings result = new();
            result.webhookNotificationHandler = webhookNotificationSettings.GetNotificationHandler();
            return result;
        }

        public Managers GetManagers()
        {
            Settings settings = GetSettings();
            return SetupManagersFromSettings(settings);
        }

        static public Managers SetupManagersFromSettings(Settings settings)
        {
            Managers managers = new Managers();
            managers.notificationManager = SetupNotifications(settings);
            return managers;
        }

        static private NotificationManager SetupNotifications(Settings settings)
        {
            NotificationManager result = new();

            if (settings.webhookNotificationHandler != null)
                result.AddHandler(settings.webhookNotificationHandler);

            if (!result.HasHandlers())
                return result;

            // Test Notifications
            var rc = result.NotifyAll("自动钓鱼程序已启动！");
            if (!rc.success)
            {
                MessageBox.Show($"通知功能测试失败（这不会阻塞钓鱼控制启动）：{rc.message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return result;
        }

    }

    public class AppSettings
    {
        public const double DefaultCastingTime = 1.7;

        public double? castingTime { get; set; }
        public WebhookNotificationSettingsControl.WebhookSettings? WebhookSettings { get; set; }
    }

    public class Settings
    {
        public INotificationHandler? webhookNotificationHandler;
    }

    public class Managers
    {
        public NotificationManager notificationManager = new();
    }
}
