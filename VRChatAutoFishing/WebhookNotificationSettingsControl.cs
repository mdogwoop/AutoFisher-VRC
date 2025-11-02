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
    public partial class WebhookNotificationSettingsControl : UserControl
    {

        public WebhookNotificationHandler? GetNotificationHandler()
        {
            if (!chbEnableNotification.Checked)
                return null;
            try
            {
                string url = txtWebhookURL.Text.Trim();
                string template = txtWebHookBodyTemplate.Text;
                return new WebhookNotificationHandler(url, template);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Webhook 配置有误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        public WebhookNotificationSettingsControl()
        {
            InitializeComponent();
        }

        private void DoEnableWebHookConfigurationUI(bool enabled)
        {
            txtWebhookURL.Enabled = enabled;
            txtWebHookBodyTemplate.Enabled = enabled;
            btnTest.Enabled = enabled;
        }

        private void chbEnableNotification_CheckedChanged(object sender, EventArgs e)
        {
            DoEnableWebHookConfigurationUI(chbEnableNotification.Checked);
        }

        private void WebhookNotificationSettingsControl_Load(object sender, EventArgs e)
        {

        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            var handler = GetNotificationHandler();
            if (handler == null)
            {
                return;
            }

            INotificationHandler general_handler = handler;
            var result = general_handler.Notify("这是一条测试消息，来自 VRChat Auto Fishing 的 Webhook 通知功能。");
            if (result.success)
            {
                MessageBox.Show("Webhook 测试消息发送成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Webhook 测试消息发送失败：" + result.message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
