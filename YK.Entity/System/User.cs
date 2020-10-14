using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YK.Entity
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Table("sys_users")]
    public class User : BaseEntity
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Required]
        [Column("user_name")]
        [MaxLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 显示昵名
        /// </summary>
        [Column("nick_name")]
        [MaxLength(50)]
        public string NickName { get; set; }

        /// <summary>
        /// 登录密码哈希值
        /// </summary>
        [Column("pwd_hash")]
        [MaxLength(100)]
        public string PasswordHash { get; set; }

        /// <summary>
        /// 显示头像
        /// </summary>
        [Column("avatar")]
        [MaxLength(255)]
        public string Avatar { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Column("user_type")]
        public int? UserType { get; set; }

        /// <summary>
        /// 用户描述
        /// </summary>
        [Column("user_desc")]
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// 是否已锁定
        /// </summary>
        /// <value></value>
        [Column("is_locked")]
        public bool? IsLocked { get; set; }
    }
}
