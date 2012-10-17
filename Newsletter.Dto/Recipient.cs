using System;

namespace Newsletter.Dto
{
    public class Recipient
    {
        public Recipient(long detailId, string name, string email, int type)
        {
            this.DetailId = detailId;

            this.Name = name;

            this.Email = email;

            this.Type = type;
        }

        public long DetailId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Type { get; set; }
    }
}
