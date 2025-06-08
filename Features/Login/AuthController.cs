using AutoMapper;
using DoDo.Data.Entities;
using DoDo.DTOs;
using DoDo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;

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
        private readonly IEmailService _emailService;

        public AuthController(
            IUsuarioRepository usuarioRepository,
            JwtService jwtService,
            IPasswordHasher<Usuario> passwordHasher,
            IMapper mapper,
            IEmailService emailService)
        {
            _usuarioRepository = usuarioRepository;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _emailService = emailService;
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] Microsoft.AspNetCore.Identity.Data.ForgotPasswordRequest request)
        {
            var user = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (user == null) return Ok(); // no revelar si existe o no

            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.ResetPasswordTokenExpiration = DateTime.UtcNow.AddMinutes(30);

            await _usuarioRepository.UpdateAsync(user);

            // Aquí normalmente enviarías el correo
            Console.WriteLine($"Token para {request.Email}: {user.ResetPasswordToken}");

            return Ok(new { message = "Si el correo existe, se envió un token de recuperación." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] DoDo.DTOs.ResetPasswordRequest request)
        {
            var user = await _usuarioRepository.GetByResetTokenAsync(request.Token);
            if (user == null || user.ResetPasswordTokenExpiration < DateTime.UtcNow)
                return BadRequest("Token inválido o expirado.");

            user.Contrasena = _passwordHasher.HashPassword(user, request.NewPassword);
            user.ResetPasswordToken = null;
            user.ResetPasswordTokenExpiration = null;

            await _usuarioRepository.UpdateAsync(user);

            return Ok(new { message = "Contraseña restablecida exitosamente." });
        }

        [HttpPost("send-reset-link")]
        public async Task<IActionResult> SendResetLink([FromBody] DTOs.SendResetPasswordRequest request)
        {
            var user = await _usuarioRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return NotFound("Usuario no encontrado.");

            user.ResetPasswordToken = Guid.NewGuid().ToString();
            user.ResetPasswordTokenExpiration = DateTime.UtcNow.AddHours(1);
            await _usuarioRepository.UpdateAsync(user);

            var link = $" http://localhost:5173/reset-password?token={user.ResetPasswordToken}";
            var body = $@"<!DOCTYPE html>
                <html>
                <head>
                    <meta charset='utf-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Restablecer de Contraseña **** </title>
                </head>
                <body style='margin: 0; padding: 0; background-color: #f4f4f4;'>
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; background-color: #f8f9fa; padding: 20px; border-radius: 10px;'>
                    <div style='background-color: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);'>
        
                        <!-- Header -->
                        <div style='text-align: center; margin-bottom: 30px;'>
                            <h1 style='color: #2c3e50; margin: 0; font-size: 24px;'>🔐 Restablecimiento de Contraseña</h1>
                            <div style='width: 50px; height: 3px; background: linear-gradient(90deg, #3498db, #2ecc71); margin: 10px auto;'></div>
                        </div>

                        <!-- Greeting -->
                        <div style='margin-bottom: 25px;'>
                            <p style='font-size: 18px; color: #2c3e50; margin: 0;'>
                                👋 ¡Hola <strong style='color: #3498db;'>{user.Nombre}</strong>!
                            </p>
                        </div>

                        <!-- Main message -->
                        <div style='background-color: #f1f8ff; padding: 20px; border-radius: 6px; border-left: 4px solid #3498db; margin-bottom: 25px;'>
                            <p style='color: #2c3e50; margin: 0 0 15px 0; line-height: 1.6;'>
                                🔄 Hemos recibido una solicitud para restablecer la contraseña de tu cuenta.
                            </p>
                            <p style='color: #2c3e50; margin: 0; line-height: 1.6;'>
                                Para continuar con el proceso, haz clic en el botón de abajo:
                            </p>
                        </div>

                        <!-- CTA Button -->
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{link}' style='display: inline-block; background: linear-gradient(135deg, #3498db, #2ecc71); color: white; text-decoration: none; padding: 15px 30px; border-radius: 25px; font-weight: bold; font-size: 16px; box-shadow: 0 4px 15px rgba(52, 152, 219, 0.3); transition: transform 0.2s;'>
                                🔓 Restablecer mi contraseña
                            </a>
                        </div>

                        <!-- Important info -->
                        <div style='background-color: #fff3cd; padding: 15px; border-radius: 6px; border-left: 4px solid #ffc107; margin-bottom: 25px;'>
                            <p style='color: #856404; margin: 0; font-size: 14px;'>
                                ⏰ <strong>Importante:</strong> Este enlace expirará en <strong>1 hora</strong> por motivos de seguridad.
                            </p>
                        </div>

                        <!-- Security note -->
                        <div style='background-color: #d1ecf1; padding: 15px; border-radius: 6px; border-left: 4px solid #17a2b8; margin-bottom: 25px;'>
                            <p style='color: #0c5460; margin: 0; font-size: 14px; line-height: 1.5;'>
                                🛡️ <strong>¿No fuiste tú?</strong><br>
                                Si no solicitaste este cambio, puedes ignorar este mensaje de forma segura. Tu cuenta permanecerá protegida.
                            </p>
                        </div>

                        <!-- Footer -->
                        <div style='text-align: center; margin-top: 40px; padding-top: 20px; border-top: 1px solid #e9ecef;'>
                            <p style='color: #6c757d; margin: 0 0 10px 0; font-size: 16px;'>
                                ¡Saludos cordiales! 😊
                            </p>
                            <p style='color: #3498db; font-weight: bold; font-size: 18px; margin: 0;'>
                                El equipo de <span style='color: #2ecc71;'>DoDo</span> 🦤
                            </p>
                        </div>

                        <!-- Alternative link -->
                        <div style='margin-top: 30px; padding: 15px; background-color: #f8f9fa; border-radius: 6px; text-align: center;'>
                            <p style='color: #6c757d; font-size: 12px; margin: 0 0 5px 0;'>
                                ¿Problemas con el botón? Copia y pega este enlace en tu navegador:
                            </p>
                            <p style='color: #3498db; font-size: 12px; margin: 0; word-break: break-all;'>
                                {link}
                            </p>
                        </div>

                    </div>
                </div>
                </body>
                </html>";
            
            try
            {
                await _emailService.SendEmailAsync(user.CorreoElectronico, "Restablecer contraseña", body);

                return Ok(new { message = "Correo enviado con el enlace de recuperación." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al enviar el correo: {ex.Message}");

            }

        }
    }
}