using Microsoft.EntityFrameworkCore;
using TravelDoc.Api.Features.Usuarios.Login;
using TravelDoc.Application.Domain.Usuarios.Repositories;
using TravelDoc.Application.Usuarios.Domain;
using TravelDoc.Repository.Contexts;

namespace TravelDoc.Repository.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly TravelDocDbContext _context;

        public UsuarioRepository(TravelDocDbContext context)
        {
            _context = context;
        }

        public async ValueTask InserirAsync(Usuario usuario)
        {
            await _context.UsuarioTb.AddAsync(usuario);

            _context.Entry(usuario).Property("DataInclusao").CurrentValue = DateTime.Now.ToUniversalTime();
        }

        public async ValueTask<Usuario?> ObterAsync(string email)
        {
            return await _context.UsuarioTb.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async ValueTask<bool> ExisteAsync(string emailOrTel)
        {
            return await _context.UsuarioTb.AnyAsync(x => x.Telefone == emailOrTel || x.Email == emailOrTel);
        }

        public async ValueTask<bool> ExisteAsync(Usuario usuario)
        {
            return await _context.UsuarioTb.AnyAsync(x => x.Cpf == usuario.Cpf || x.Email == usuario.Email);
        }
    }
}
