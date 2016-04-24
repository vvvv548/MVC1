namespace MVC1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        protected 客戶聯絡人Repository repoCustContact = RepositoryHelper.Get客戶聯絡人Repository();
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Id == 0)
            {
                // Create 
                if (repoCustContact.All(false)
                    .Where(p => 客戶Id == this.客戶Id && p.Email==this.Email)
                    .Any())
                {
                    yield return new ValidationResult("相同一個客戶下的聯絡人Email不可重複！請重新輸入！", new string[]{ "Email"});
                }
            }
            else
            {
                //Edit
                if (repoCustContact.All(false)
                    .Where(p => 客戶Id == this.客戶Id && p.Email == this.Email && p.Id != this.Id)
                    .Any())
                {
                    yield return new ValidationResult("相同一個客戶下的聯絡人Email不可重複！請重新輸入！", new string[] { "Email" });
                }
            }
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "客戶名稱", Description = "FK.客戶資料.客戶名稱")]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }
        
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        [EmailAddress(ErrorMessage = "請輸入符合規範的Email")]
        public string Email { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [RegularExpression(@"^\d{4}-\d{6}$", ErrorMessage = "請輸入正確的手機號碼 EX: 09XX-XXXXXX")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
        public Nullable<bool> 是否已刪除 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
