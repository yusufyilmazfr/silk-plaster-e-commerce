using SilkPlaster.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilkPlaster.Entities
{
    [Table("Members")]
    public class Member : EntityBase, IEntity
    {
        [Required,
        DisplayName("İsim"),
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string FirstName { get; set; }

        [Required,
        DisplayName("Soyisim"),
        StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string LastName { get; set; }

        [Required,
        DisplayName("E-posta"),
        StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [Required,
        DisplayName("Parola"),
        StringLength(64, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }


        public List<WishList> WishLists { get; set; }
        public List<Order> Orders { get; set; }
        public List<Basket> Baskets { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
