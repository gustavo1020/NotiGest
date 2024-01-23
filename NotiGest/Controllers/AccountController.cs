using Application.Contracts;
using Application.DTO;
using Application.Page;
using Core.Entityes;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace NotiGest.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwt;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtService jwt)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._jwt = jwt;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustom([FromBody] UserInsertDto usuarioDto)
        {
            try
            {
                var entityUser = usuarioDto.Adapt<User>();

                var usercreate = await _userManager.CreateAsync(entityUser, usuarioDto.Password ?? string.Empty);

                if (!usercreate.Succeeded) return BadRequest($"Error al crear el usuario {usercreate.Errors.First()}");

                await _userManager.UpdateSecurityStampAsync(entityUser);

                var userRol = await _userManager.AddToRoleAsync(entityUser, "UserDefault");

                if (!userRol.Succeeded) return BadRequest($"Error al relacionarel usuarioRol {userRol.Errors}");

                return Ok("Registro Exitoso");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoginCustom([FromBody] UserInsertDto usuarioDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(usuarioDto.Email ?? string.Empty) ?? new User { };

                if (user == null) return BadRequest("Credenciales inválidas");

                var userRol = await _userManager.GetRolesAsync(user);

                if (userRol == null) return BadRequest("Error al generar el token del usuario");

                var result = await _signInManager.PasswordSignInAsync(user, usuarioDto.Password ?? string.Empty, false, lockoutOnFailure: false);

                if (result.Succeeded) return Ok(_jwt.GenerateToken(user.Email ?? string.Empty, userRol, user.Id, user.UserName));

                if (result.IsLockedOut) return BadRequest("La cuenta está bloqueada");

                return BadRequest("Credenciales inválidas");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
        
            return Ok(users.Adapt<List<UserGet>>());
        }
    }
}
