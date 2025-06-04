using AutoMapper;
using DoDo.Data.Entities;
using DoDo.DTOs;
using DoDo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DoDo.Features.Login
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly JwtService _jwtService;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IMapper _mapper;

        public AuthController(
            IUsuarioRepository usuarioRepository,
            JwtService jwtService,
            IPasswordHasher<Usuario> passwordHasher,
            IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existingUser = await _usuarioRepository.ObtenerPorCorreoAsync(dto.CorreoElectronico);
            if (existingUser != null)
                return BadRequest("El correo ya está registrado.");

            var usuario = _mapper.Map<Usuario>(dto);
            usuario.Contrasena = _passwordHasher.HashPassword(usuario, dto.Contrasena);
            usuario.FechaRegistro = DateTime.UtcNow;
            usuario.Activo = true;

            await _usuarioRepository.CrearAsync(usuario);

            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
               new Claim(ClaimTypes.Name, usuario.Nombre ?? string.Empty),
               new Claim(ClaimTypes.Email, usuario.CorreoElectronico ?? string.Empty)
           };

            var token = _jwtService.GenerateToken(claims);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var usuario = await _usuarioRepository.ObtenerPorCorreoAsync(dto.CorreoElectronico);
            if (usuario == null)
                return Unauthorized("Correo o contraseña incorrectos.");

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Contrasena, dto.Contrasena);
            if (resultado != PasswordVerificationResult.Success)
                return Unauthorized("Correo o contraseña incorrectos.");

            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
               new Claim(ClaimTypes.Name, usuario.Nombre ?? string.Empty),
               new Claim(ClaimTypes.Email, usuario.CorreoElectronico ?? string.Empty)
           };

            var token = _jwtService.GenerateToken(claims);

            return Ok(new { token });
        }
    }
}
