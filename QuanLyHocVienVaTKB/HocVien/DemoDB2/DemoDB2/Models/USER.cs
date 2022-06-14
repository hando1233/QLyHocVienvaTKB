﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoDB2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            this.RAOVATs = new HashSet<RAOVAT>();
        }
        [Display(Name = "Mã Người Dùng")]
        [Required(ErrorMessage = "Tên tài khoản không được trống")]
        public int MANGUOIDUNG { get; set; }
        [Display(Name = "Tên Người Dùng")]
        [Required(ErrorMessage = "Tên tài khoản không được trống")]
        public string TENDANGNHAP { get; set; }
        [Display(Name = "Mật Khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được trống")]
        public string MATKHAU { get; set; }
        [Display(Name = "Số Điện Thoại")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Số điện thoại không được trống")]
        public string SODIENTHOAI { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email không được trống")]
        public string EMAIL { get; set; }
        [Display(Name = "Địa Chỉ")]
        public string DIACHI { get; set; }
        [Display(Name = "Giới Tính")]
        public Nullable<bool> GIOITINH { get; set; }
        [Display(Name = "Ngày Sinh")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> NGAYSINH { get; set; }
        [Display(Name = "Họ Tên")]
        [Required(ErrorMessage = "Họ tên không được trống")]
        public string HOTEN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RAOVAT> RAOVATs { get; set; }

        [NotMapped]
        [Compare("MATKHAU")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập Lại Mật Khẩu")]
        public string ConfirmPass { get; set; }
        [NotMapped]
        public string ErrorLogin { get; set; }
    }
}