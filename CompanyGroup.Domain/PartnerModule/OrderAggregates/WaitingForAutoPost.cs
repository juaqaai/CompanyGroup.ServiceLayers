using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// várakozó sor elem
    /// </summary>
    public class WaitingForAutoPost
    {
        public WaitingForAutoPost(int id, int foreignKey, int foreignKeyType, string content, DateTime createdDate, DateTime postedDate, int status)
        {
            this.Id = id;

            this.ForeignKey = foreignKey;

            this.ForeignKeyType = foreignKeyType;

            this.Content = content;

            this.CreatedDate = createdDate;

            this.PostedDate = postedDate;

            this.Status = status;
        }

        public int Id { get; set; } 

        public int ForeignKey { get; set; }
  
        public int ForeignKeyType { get; set; }  

        public string Content { get; set; }  

        public DateTime CreatedDate { get; set; } 
 
        public DateTime PostedDate { get; set; }  

        public int Status { get; set; } 
    }

    /// <summary>
    /// várakozó sor
    /// </summary>
    public class WaitingForAutoPostList : List<WaitingForAutoPost>
    {
        public WaitingForAutoPostList(List<WaitingForAutoPost> items)
        {
            if (items != null)
            {
                this.AddRange(items);
            }
        }
    }
}
