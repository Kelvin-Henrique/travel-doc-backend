using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelDoc.Api.Features.Usuarios.Login;
using TravelDoc.Application.Usuarios.Domain;

namespace TravelDoc.Application.Domain.Usuarios.Repositories
{
    public interface IUsuarioRepository
    {
        ValueTask InserirAsync(Usuario usuario);
        ValueTask<bool> ExisteAsync(Usuario usuario);
        ValueTask<bool> ExisteAsync(string emailOrTel);
        ValueTask<UsuarioViewModel?> ObterAsync(string email);
    }
}
