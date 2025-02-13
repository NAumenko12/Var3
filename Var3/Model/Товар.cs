//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Var3.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Товар
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Товар()
        {
            this.Избранные_товары = new HashSet<Избранные_товары>();
            this.Состав_заказа = new HashSet<Состав_заказа>();
        }
    
        public int Id_Товар { get; set; }
        public string Артикул { get; set; }
        public string Название { get; set; }
        public Nullable<int> Категория { get; set; }
        public string Бренд { get; set; }
        public string Животное { get; set; }
        public string Описание { get; set; }
        public string Состав { get; set; }
        public Nullable<int> Колличество_за_ед { get; set; }
        public string Единица { get; set; }
        public Nullable<decimal> Стоимость { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Избранные_товары> Избранные_товары { get; set; }
        public virtual Категории Категории { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Состав_заказа> Состав_заказа { get; set; }
    }
}
