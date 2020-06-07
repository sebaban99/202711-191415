using IMMRequest.Domain;
using System;

namespace IMMRequest.WebApi
{
    public class AdminDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public AdminDTO() { }

        public AdminDTO(Admin admin)
        {
            Id = admin.Id;
            Name = admin.Name;
            Email = admin.Email;
            Password = admin.Password;
        }

        public Admin ToEntity()
        {
            Admin adminAsEntity = new Admin()
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Password = this.Password
            };
            return adminAsEntity;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                AdminDTO adminDTO = (AdminDTO)obj;
                return this.Id == adminDTO.Id;
            }
        }
    }
}