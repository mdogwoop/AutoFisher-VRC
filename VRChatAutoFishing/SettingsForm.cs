using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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

    public class Settings
    {
        public INotificationHandler? webhookNotificationHandler;

    }

    public class Managers
    {
        public NotificationManager notificationManager = new();
    }
}
