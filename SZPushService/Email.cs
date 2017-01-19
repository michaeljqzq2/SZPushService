using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace SZPushService
{
    public static class Email
    {
        private static readonly MailMessage mMailMessage;   //主要处理发送邮件的内容（如：收发人地址、标题、主体、图片等等）
        private static readonly SmtpClient mSmtpClient; //主要处理用smtp方式发送此邮件的配置信息（如：邮件服务器、发送端口号、验证方式等等）
        private const int mSenderPort=25;   //发送邮件所用的端口号（htmp协议默认为25）
        private const string mSenderServerHost= "smtp.sina.com";    //发件箱的邮件服务器地址（IP形式或字符串形式均可）
        private const string mSenderPassword = "sina2mlw381109";    //发件箱的密码
        private const string mSenderUsername = "michaeljqzq";   //发件箱的用户名（即@符号前面的字符串，例如：hello@163.com，用户名为：hello）
        private const bool mEnableSsl = true;    //是否对邮件内容进行socket层加密传输
        private const bool mEnablePwdAuthentication = true;  //是否对发件人邮箱进行密码验证
        private const string toMail = "michaeljqzq@139.com";
        private const string fromMail = "michaeljqzq@sina.com";
        private static object olock = new object();
        static Email()
        {
            mMailMessage = new MailMessage();
            mMailMessage.To.Add(toMail);
            mMailMessage.From = new MailAddress(fromMail);

            mSmtpClient = new SmtpClient();
            //mSmtpClient.Host = "smtp." + mMailMessage.From.Host;
            mSmtpClient.Host = mSenderServerHost;
            mSmtpClient.Port = mSenderPort;
            mSmtpClient.UseDefaultCredentials = false;
            mSmtpClient.EnableSsl = mEnableSsl;
        }
        ///<summary>
        /// 添加附件
        ///</summary>
        ///<param name="attachmentsPath">附件的路径集合，以分号分隔</param>
        public static void AddAttachments(string attachmentsPath)
        {
            try
            {
                string[] path = attachmentsPath.Split(';'); //以什么符号分隔可以自定义
                Attachment data;
                ContentDisposition disposition;
                for (int i = 0; i < path.Length; i++)
                {
                    data = new Attachment(path[i], MediaTypeNames.Application.Octet);
                    disposition = data.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(path[i]);
                    disposition.ModificationDate = File.GetLastWriteTime(path[i]);
                    disposition.ReadDate = File.GetLastAccessTime(path[i]);
                    mMailMessage.Attachments.Add(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        ///<summary>
        /// 邮件的发送
        ///</summary>
        public static void Send(string subject,string emailBody)
        {
            try
            {
                if (mMailMessage != null)
                {
                    lock (olock)
                    {
                        mMailMessage.Subject = subject;
                        mMailMessage.Body = emailBody;
                        mMailMessage.IsBodyHtml = true;
                        mMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                        mMailMessage.Priority = MailPriority.Normal;

                        if (mEnablePwdAuthentication)
                        {
                            System.Net.NetworkCredential nc = new System.Net.NetworkCredential(mSenderUsername, mSenderPassword);
                            //mSmtpClient.Credentials = new System.Net.NetworkCredential(mSenderUsername, mSenderPassword);
                            //NTLM: Secure Password Authentication in Microsoft Outlook Express
                            mSmtpClient.Credentials = nc.GetCredential(mSmtpClient.Host, mSmtpClient.Port, "NTLM");
                        }
                        else
                        {
                            mSmtpClient.Credentials = new System.Net.NetworkCredential(mSenderUsername, mSenderPassword);
                        }
                        mSmtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        mSmtpClient.Send(mMailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sent failed"+ex.ToString());
            }
        }
    }



}
