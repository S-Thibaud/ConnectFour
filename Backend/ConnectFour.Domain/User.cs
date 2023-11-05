using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Identity;

namespace ConnectFour.Domain;

public class User : IdentityUser<Guid>
{
    [Required]
    public string NickName { get; set; }

    //public override bool Equals(object obj)
    //{
    //    if (this == obj) return true;
    //    if (obj == null) return false;

    //    User user = (User)obj;
    //    return NickName.Equals(user.NickName);
    //}

    //public override int GetHashCode()
    //{
    //    return base.GetHashCode();
    //}
}