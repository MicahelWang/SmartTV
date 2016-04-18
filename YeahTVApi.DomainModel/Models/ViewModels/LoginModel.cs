using System.ComponentModel.DataAnnotations;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class LoginModel
    {
        [Display(Name = "姓名")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "确认密码")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }
}