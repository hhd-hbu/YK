using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YK.Entity
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class BaseEntity
    {

        /// <summary>
        /// 惟一标识
        /// </summary>
        [Key]
        [Column("id",  Order = 1)]
        public string Uid { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Column("status", Order = 101)]
        public int? Status { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        [Column("del_flag", Order = 102)]
        public int? IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column("create_time", Order = 103)]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者ID
        /// </summary>
        [Column("create_user", Order = 104)]
        public string CreateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Column("update_time", Order = 105)]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新者ID
        /// </summary>
        [Column("update_user", Order = 106)]
        public string UpdateUser { get; set; }
    }
}
