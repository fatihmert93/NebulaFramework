using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nebula.CoreLibrary.Shared
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {

        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
