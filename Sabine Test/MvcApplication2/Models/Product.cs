//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcApplication2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.ProductCategoryMapping = new HashSet<ProductCategoryMapping>();
        }
    
        public int ProductId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<ProductCategoryMapping> ProductCategoryMapping { get; set; }
    }
}