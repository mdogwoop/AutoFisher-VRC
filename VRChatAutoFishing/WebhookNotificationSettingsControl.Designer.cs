namespace VRChatAutoFishing
{
    partial class WebhookNotificationSettingsControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            txtWebHookBodyTemplate = new TextBox();
            lblWebHookBodyTemplate = new Label();
            lblWebHookURL = new Label();
            txtWebhookURL = new TextBox();
            chbEnableNotification = new CheckBox();
            btnTest = new Button();
            SuspendLayout();
            // 
            // txtWebHookBodyTemplate
            // 
            txtWebHookBodyTemplate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtWebHookBodyTemplate.Enabled = false;
            txtWebHookBodyTemplate.Location = new Point(8, 99);
            txtWebHookBodyTemplate.Multiline = true;
            txtWebHookBodyTemplate.Name = "txtWebHookBodyTemplate";
            txtWebHookBodyTemplate.Size = new Size(342, 207);
            txtWebHookBodyTemplate.TabIndex = 15;
            txtWebHookBodyTemplate.Text = "{\"msg_type\":\"text\",\"content\":{\"text\":\"{{message}}\"}}";
            // 
            // lblWebHookBodyTemplate
            // 
            lblWebHookBodyTemplate.AutoSize = true;
            lblWebHookBodyTemplate.Location = new Point(5, 75);
            lblWebHookBodyTemplate.Name = "lblWebHookBodyTemplate";
            lblWebHookBodyTemplate.Size = new Size(80, 17);
            lblWebHookBodyTemplate.TabIndex = 14;
            lblWebHookBodyTemplate.Text = "请求体模板";
            // 
            // lblWebHookURL
            // 
            lblWebHookURL.AutoSize = true;
            lblWebHookURL.Location = new Point(8, 43);
            lblWebHookURL.Name = "lblWebHookURL";
            lblWebHookURL.Size = new Size(34, 17);
            lblWebHookURL.TabIndex = 13;
            lblWebHookURL.Text = "URL:";
            // 
            // txtWebhookURL
            // 
            txtWebhookURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtWebhookURL.Enabled = false;
            txtWebhookURL.Location = new Point(48, 40);
            txtWebhookURL.Name = "txtWebhookURL";
            txtWebhookURL.Size = new Size(302, 23);
            txtWebhookURL.TabIndex = 12;
            // 
            // chbEnableNotification
            // 
            chbEnableNotification.AutoSize = true;
            chbEnableNotification.Location = new Point(8, 10);
            chbEnableNotification.Name = "chbEnableNotification";
            chbEnableNotification.Size = new Size(178, 21);
            chbEnableNotification.TabIndex = 11;
            chbEnableNotification.Text = "启用错误时 WebHook 通知";
            chbEnableNotification.UseVisualStyleBackColor = true;
            chbEnableNotification.CheckedChanged += chbEnableNotification_CheckedChanged;
            // 
            // btnTest
            // 
            btnTest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnTest.Enabled = false;
            btnTest.Location = new Point(275, 10);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(75, 23);
            btnTest.TabIndex = 16;
            btnTest.Text = "测试";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // WebhookNotificationSettingsControl
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnTest);
            Controls.Add(txtWebHookBodyTemplate);
            Controls.Add(lblWebHookBodyTemplate);
            Controls.Add(lblWebHookURL);
            Controls.Add(txtWebhookURL);
            Controls.Add(chbEnableNotification);
            Name = "WebhookNotificationSettingsControl";
            Size = new Size(357, 312);
            Load += WebhookNotificationSettingsControl_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtWebHookBodyTemplate;
        private Label lblWebHookBodyTemplate;
        private Label lblWebHookURL;
        private TextBox txtWebhookURL;
        private CheckBox chbEnableNotification;
        private Button btnTest;
    }
}
