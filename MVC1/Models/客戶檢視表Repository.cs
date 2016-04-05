using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC1.Models
{   
	public  class 客戶檢視表Repository : EFRepository<客戶檢視表>, I客戶檢視表Repository
	{

	}

	public  interface I客戶檢視表Repository : IRepository<客戶檢視表>
	{

	}
}