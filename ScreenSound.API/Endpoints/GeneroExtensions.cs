using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;

namespace ScreenSound.API.Endpoints;

public static class GeneroExtensions
{
    public static void AddEndPointGenero(this WebApplication app)
    {
        #region EndPoint Generos

        app.MapGet("/Generos", ([FromServices] DAL<Genero> dal) =>
        {
            return EntityListToResponseList(dal.Listar());
        });

        app.MapGet("/Generos/{nome}", ([FromServices] DAL<Genero> dal, string nome) =>
        {
            var musica = dal.RecuperarPor(m => m.Nome.ToUpper().Equals(nome.ToUpper()));

            if (musica is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(musica);
        });

        app.MapPost("/Generos", ([FromServices] DAL<Genero> dal, [FromBody] GeneroRequest generoRequest) =>
        {
            dal.Adicionar(RequestToEntity(generoRequest));
        });

        app.MapDelete("/Generos/{id}", ([FromServices] DAL<Genero> dal, int id) =>
        {
            var genero = dal.RecuperarPor(g => g.Id == id);
            if (genero is null)
            {
                return Results.NotFound();
            }
            dal.Deletar(genero);
            return Results.NoContent();
        });

        app.MapPut("/Generos", ([FromServices] DAL<Genero> dal, [FromBody] Genero genero) =>
        {
            var GeneroAAtualizar = dal.RecuperarPor(g => g.Id == genero.Id);
            if (GeneroAAtualizar is null)
            {
                return Results.NotFound();
            }
            GeneroAAtualizar.Nome = genero.Nome;
            GeneroAAtualizar.Descricao = genero.Descricao;

            dal.Atualizar(GeneroAAtualizar);
            return Results.Ok();
        });
    }
        #endregion

        #region Request e Response

        private static Genero RequestToEntity(GeneroRequest generoRequest)
        {
            return new Genero() { Nome = generoRequest.Nome, Descricao = generoRequest.Descricao };
        }

        private static ICollection<GeneroResponse> EntityListToResponseList(IEnumerable<Genero> generos)
        {
            return generos.Select(a => EntityToResponse(a)).ToList();
        }

        private static GeneroResponse EntityToResponse(Genero genero)
        {
            return new GeneroResponse(genero.Id, genero.Nome!, genero.Descricao!);
        }

        #endregion
}

