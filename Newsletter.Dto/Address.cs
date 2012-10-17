using System;

namespace Newsletter.Dto
{
    public class Address
    {
        public Address(string name, string email)
        {
            this.Name = name;

            this.Email = email;
        }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
