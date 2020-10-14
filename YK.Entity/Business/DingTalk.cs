using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YK.Entity
{
    /// <summary>
    /// 钉钉机器人
    /// </summary>
    [Table("biz_ding_talk")]
    public class DingTalk : BaseEntity
    {
        /// <summary>
        /// 机器人名称
        /// </summary>
        /// <value></value>
        [Required, Column("dingtalk_name")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 钉钉链接
        /// </summary>
        /// <value></value>
        [Required, Column("dingtalk_url")]
        [MaxLength(500)]
        public string Url { get; set; }

        /// <summary>
        /// 是否发送短信
        /// </summary>
        /// <value></value>
        [Column("send_sms_sign")]
        public bool? SendSms { get; set; }

        /// <summary>
        /// 是否发送邮件
        /// </summary>
        /// <value></value>
        [Column("send_mail_sign")]
        public bool? SendMail { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        [Column("remark")]
        [MaxLength(200)]
        public string Remark { get; set; }
    }
}
