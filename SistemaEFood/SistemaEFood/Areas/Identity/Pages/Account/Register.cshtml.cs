// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SistemaEFood.Modelos;
using SistemaEFood.Utilidades;

namespace SistemaEFood.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "La {0} contraseña debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = "El {0}  debe tener al menos {2} y máximo {1} caracteres.", MinimumLength = 6)]
            
            [Display(Name = "Usuario")]
            public string Usuario { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "La clave y confirmacion no con iguales.")]
            public string ConfirmPassword { get; set; }

            // tamanno 60
            [Required]
            [StringLength(60, ErrorMessage = "La {0} debe tener minimo {2} y maximo {1} caracteres.", MinimumLength = 20)]
            public string PreguntaSeguridad { get; set; }
            //tamanno 50
            [Required]
            [StringLength(50, ErrorMessage = "La {0} debe tener minimo {2} y maximo {1} caracteres.", MinimumLength = 4)]
            public string RespuestaSeguridad { get; set; }


            public string Role { get; set; }
            public IEnumerable<SelectListItem> ListaRol { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input = new InputModel()
            {
                ListaRol= _roleManager.Roles.Where(r => r.Name != DS.Role_Usuario).Select(n => n.Name).Select( L => new SelectListItem{

                    Text= L,
                    Value = L
                })
            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user =  new Usuario { UserName= Input.Usuario,
                   
                    Email= Input.Email,
                    PreguntaSeguridad=Input.PreguntaSeguridad,
                    RespuestaSeguridad= Input.RespuestaSeguridad,
                    Role = Input.Role,
                    Estado= true
                };
               



                await _userStore.SetUserNameAsync(user, Input.Usuario, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Admin)) {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Consulta))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Consulta));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Seguridad))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Seguridad));
                    }
                    if (!await _roleManager.RoleExistsAsync(DS.Role_Mantenimiento))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Mantenimiento));
                    }

                    if (!await _roleManager.RoleExistsAsync(DS.Role_Usuario))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(DS.Role_Usuario));
                    }

                    if (user.Role ==null )// es un cliente
                    {
                        await _userManager.AddToRoleAsync(user, DS.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, user.Role);
                    }


                    var userId = await _userManager.GetUserIdAsync(user);
                   var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirmar tu email",
                        $"Por favor confirma tu correo <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clickeando aca</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Role==null)
                        {
                            //Cliente se registra, entonces lo loggea de una
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            // El admin esta registrando a alguien
                            //Se conserva su login como administrador
                            //Lo envia al modulo de administracion de usuarios.
                            return RedirectToAction("Index", "Usuario", new { Area = "Admin" });
                        }
                       
                    }
                }

                ReturnUrl = returnUrl;
                Input = new InputModel()
                {
                    ListaRol = _roleManager.Roles.Where(r => r.Name != DS.Role_Usuario).Select(n => n.Name).Select(L => new SelectListItem
                    {

                        Text = L,
                        Value = L
                    })
                };
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
