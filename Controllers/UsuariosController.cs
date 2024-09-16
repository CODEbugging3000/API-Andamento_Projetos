using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Exo.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        public UsuariosController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        
        // GET all usuarios
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_usuarioRepository.Listar());
        }

        // POST usuarios
        /*[HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            _usuarioRepository.Cadastrar(usuario);
            return StatusCode(201);
        }*/
        
        //POST with Login
        public IActionResult Post(Usuario usuario)
        {
            Usuario usuarioBuscado = _usuarioRepository.Login(usuario.Email, usuario.Senha);
            if (usuarioBuscado == null)
            {
                return NotFound("E-mail ou Senha Inválidos!");
            }
            else{
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.Id.ToString())
                };
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: "exoapi.webapi", // Emissor do token.
                    audience: "exoapi.webapi", // Destinatário do token.
                    claims: claims, // Dados definidos acima.
                    expires: DateTime.Now.AddMinutes(30), // Tempo de expiração.
                    signingCredentials: creds // Credenciais do token.
                );
                return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
            }
        }

        //GET By Id usuarios
        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            Usuario usuario = _usuarioRepository.BuscaPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        
        //PUT usuarios (Somente usuarios autenticados)
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Usuario usuario)
        {
            _usuarioRepository.Atualizar(id,usuario);
            return StatusCode(204);
        }

        //DELETE usuarios (Somente usuarios autenticados)
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _usuarioRepository.Deletar(id);
                return StatusCode(204);
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}