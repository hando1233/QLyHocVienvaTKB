//------------------------------------------------------------------------------
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
    
    public partial class THOIKHOABIEU
    {
        public int ThoiKhoaBieuID { get; set; }
        public Nullable<int> ChiTietID { get; set; }
        public Nullable<int> HocVienID { get; set; }
        public Nullable<System.DateTime> NgayBatDau { get; set; }
        public Nullable<System.DateTime> NgayKetThuc { get; set; }
        public Nullable<int> HocKy { get; set; }
    
        public virtual CHITIETKHOAHOC CHITIETKHOAHOC { get; set; }
        public virtual HOCVIEN HOCVIEN { get; set; }
    }
}
