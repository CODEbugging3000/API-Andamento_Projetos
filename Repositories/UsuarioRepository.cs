using Exo.WebApi.Contexts;
using Exo.WebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace Exo.WebApi.Repositories
{
    public class UsuarioRepository
    {
        //Constructor
        private readonly ExoContext _context;
        public UsuarioRepository(ExoContext context)
        {
            _context = context;
        }


        public Usuario Login(string email,  string senha)
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
        }
        //GET all de usuarios
        public List<Usuario> Listar()
        {
            return _context.Usuarios.ToList();
        }
        //POST de usuarios
        public void Cadastrar(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        //GET by Id de usuarios
        public Usuario BuscaPorId(int id)
        {
            return _context.Usuarios.Find(id);
        }
        //PUT de usuarios
        public void Atualizar(int id,  Usuario usuario)
        {
            Usuario usuarioBuscado = _context.Usuarios.Find(id);
            if (usuarioBuscado != null)
            {
                usuarioBuscado.Email = usuario.Email;
                usuarioBuscado.Senha = usuario.Senha;
            }
            _context.Usuarios.Update(usuarioBuscado);
            _context.SaveChanges();
        }
        //DELETE de usuarios
        public void Deletar(int id)
        {
            Usuario usuarioBuscado = _context.Usuarios.Find(id);
            _context.Usuarios.Remove(usuarioBuscado);
            _context.SaveChanges();
        }
    }
}