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
    
    public partial class CHITIETKHOAHOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CHITIETKHOAHOC()
        {
            this.BANGDIEMs = new HashSet<BANGDIEM>();
            this.THOIKHOABIEUx = new HashSet<THOIKHOABIEU>();
        }
    
        public int ChiTietID { get; set; }
        public Nullable<int> GiangVienID { get; set; }
        public string PhongHocId { get; set; }
        public Nullable<int> KhoaHocID { get; set; }
        public string Thu { get; set; }
        public Nullable<System.DateTime> NgayBatDauKhoaHoc { get; set; }
        public Nullable<System.DateTime> NgayKetThucKhoaHoc { get; set; }
        public Nullable<byte> SoTiet { get; set; }
        public Nullable<byte> TietBatDau { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BANGDIEM> BANGDIEMs { get; set; }
        public virtual GIANGVIEN GIANGVIEN { get; set; }
        public virtual KHOAHOC KHOAHOC { get; set; }
        public virtual PHONGHOC PHONGHOC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THOIKHOABIEU> THOIKHOABIEUx { get; set; }
    }
}
