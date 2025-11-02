namespace VRChatAutoFishing
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbcSettings = new TabControl();
            tabWebhookNotification = new TabPage();
            webhookNotificationSettings = new WebhookNotificationSettingsControl();
            tbcSettings.SuspendLayout();
            tabWebhookNotification.SuspendLayout();
            SuspendLayout();
            // 
            // tbcSettings
            // 
            tbcSettings.Controls.Add(tabWebhookNotification);
            tbcSettings.Location = new Point(12, 12);
            tbcSettings.Name = "tbcSettings";
            tbcSettings.SelectedIndex = 0;
            tbcSettings.Size = new Size(384, 298);
            tbcSettings.TabIndex = 0;
            // 
            // tabWebhookNotification
            // 
            tabWebhookNotification.Controls.Add(webhookNotificationSettings);
            tabWebhookNotification.Location = new Point(4, 26);
            tabWebhookNotification.Name = "tabWebhookNotification";
            tabWebhookNotification.Padding = new Padding(3);
            tabWebhookNotification.Size = new Size(376, 268);
            tabWebhookNotification.TabIndex = 0;
            tabWebhookNotification.Text = "Webhook 通知设置";
            tabWebhookNotification.UseVisualStyleBackColor = true;
            // 
            // webhookNotificationSettings
            // 
            webhookNotificationSettings.Location = new Point(9, 6);
            webhookNotificationSettings.Name = "webhookNotificationSettings";
            webhookNotificationSettings.Size = new Size(359, 247);
            webhookNotificationSettings.TabIndex = 0;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(408, 320);
            Controls.Add(tbcSettings);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "自动钓鱼 - 高级设置";
            tbcSettings.ResumeLayout(false);
            tabWebhookNotification.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tbcSettings;
        private TabPage tabWebhookNotification;
        private WebhookNotificationSettingsControl webhookNotificationSettings;
    }
}