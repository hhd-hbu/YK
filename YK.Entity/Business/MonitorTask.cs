using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YK.Entity
{
    /// <summary>
    /// 监控任务
    /// </summary>
    [Table("biz_monitor_task")]
    public class MonitorTask : BaseEntity
    {
        /// <summary>
        /// 租户ID：6000=绿地，8000=星动力，9000=怡佳仁
        /// </summary>
        /// <value></value>
        [Required, Column("tenant_num_id")]
        public int? Tenant { get; set; }

        /// <summary>
        /// 监控任务名称
        /// </summary>
        /// <value></value>
        [Required, Column("task_name")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 钉钉机器人ID，以逗号分隔
        /// </summary>
        /// <value></value>
        [Required, Column("ding_talks")]
        [MaxLength(100)]
        public string DingTalks { get; set; }

        /// <summary>
        /// 监控方式 0-通用查询; 1-API接口
        /// </summary>
        /// <value></value>
        [Required, Column("type_num_id")]
        [DefaultValue(0)]
        public int? Type { get; set; }

        /// <summary>
        /// sqlid或api接口
        /// </summary>
        /// <value></value>
        [Required, Column("task_content")]
        [MaxLength(100)]
        public string Content { get; set; }

        /// <summary>
        /// 手机号 最多50个
        /// </summary>
        /// <value></value>
        [Column("cell_phones")]
        [MaxLength(800)]
        public string Mobiles { get; set; }

        /// <summary>
        /// 接收邮箱 最多50个
        /// </summary>
        /// <value></value>
        [Column("recv_emails")]
        [MaxLength(2000)]
        public string Emails { get; set; }

        /// <summary>
        /// 有效期开始日期，默认为当前时间
        /// </summary>
        [Column("task_begin_date")]
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 有效期结束日期，默认为永久有效9999-12-31
        /// </summary>
        [Column("task_end_date")]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 执行规则-月
        /// </summary>
        /// <value></value>
        [Required, Column("schedular_month")]
        [DefaultValue(0)]
        public int? SchedularMonth { get; set; }

        /// <summary>
        /// 执行规则-日
        /// </summary>
        /// <value></value>
        [Required, Column("schedular_day")]
        [DefaultValue(0)]
        public int? SchedularDay { get; set; }

        /// <summary>
        /// 执行规则-周（位操作：周一1,周二2,周三4,周四8,周五16,周六32,周日64）
        /// </summary>
        /// <value></value>
        [Column("schedular_week_days")]
        [DefaultValue(0)]
        public int? SchedularWeekDays { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        /// <value></value>
        [Column("schedular_begin_time")]
        [DefaultValue("'00:00'")]
        [MaxLength(10)]
        public string SchedularBeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        /// <value></value>
        [Column("schedular_end_time")]
        [DefaultValue("'23:59'")]
        [MaxLength(10)]
        public string SchedularEndTime { get; set; }

        /// <summary>
        /// 执行规则-间隔时间（单位：秒，0表示只执行一次）
        /// </summary>
        /// <value></value>
        [Required, Column("schedular_loop_seconds")]
        [DefaultValue(0)]
        public int? LoopSeconds { get; set; }

        /// <summary>
        /// 执行规则描述
        /// </summary>
        /// <value></value>
        [Required, Column("schedular_rule_desc")]
        [MaxLength(500)]
        public string SchedularRule { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        /// <value></value>
        [Required, Column("msg_content")]
        [MaxLength(500)]
        public string Message { get; set; }

        /// <summary>
        /// 显示条数：0表示显示全部
        /// </summary>
        /// <value></value>
        [Column("show_count")]
        public int? ShowCount { get; set; }

        /// <summary>
        /// 最近执行时间
        /// </summary>
        /// <value></value>
        [Column("last_run_time")]
        public DateTime? LastRunTime { get; set; }
    }
}
