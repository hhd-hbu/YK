using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace YK.Entity
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("sys_roles")]
    public class Role : BaseEntity
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        [Required]
        [Key]
        [Column("role_code")]
        [MaxLength(20)]
        public string Code { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required]
        [Column("role_name")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [Column("role_desc")]
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// 是否是超级管理员(超级管理员拥有系统的所有权限)
        /// </summary>
        [Column("is_super_admin")]
        public bool IsSuperAdmin { get; set; }

        /// <summary>
        /// 是否是系统内置角色(系统内置角色不允许删除,修改操作)
        /// </summary>
        [Column("is_builtin")]
        public bool IsBuiltin { get; set; }
    }
}
