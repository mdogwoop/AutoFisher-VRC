# AutoFisher-VRC 🎮⚡
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 自动钓鱼(C#重构)

读取日志+OSC输入控制，合法辅助，可完全后台的挂机脚本

演示：https://www.bilibili.com/video/BV1TqotYrEDe

说明：软件里面有

## WebHook 通知 (Since V1.5.2)

启用后，可以使用在以下情况：

1. 程序开始控制VRC
2. 太久没有成功钓到鱼（包括久不上钩、收杆失败）
3. 钓鱼线程抛出异常

向指定的 Webhook 服务器发送一个 POST 请求。其 Content-Type 为 `application/json`，内容为指定的模板渲染后的内容。

模板渲染总是替换以下变量到实际值：

- `{{messaage}}` : 消息内容

### 使用示例 - 飞书Bot

参考[文档](https://open.feishu.cn/document/client-docs/bot-v3/add-custom-bot?lang=zh-CN)配置一个Bot，然后将 Webhook URL 填入程序内，点击启动，则会使用配置的 Bot 发送一条通知以测试。