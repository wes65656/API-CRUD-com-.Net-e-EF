
using Microsoft.EntityFrameworkCore;
using MinhaWebAPI.Estudantes.Data;

namespace MinhaWebAPI.Estudantes;



public static class EstudantesRotas
{
    public static void AddRotasEstudantes(this WebApplication app)
    {
        // Criando os usuarios com map post
        var RotasEstudantes = app.MapGroup ("estudantes");
        //app.MapGet("estudantes", () => new Estudante("Weslley"));
        RotasEstudantes.MapPost("", async (AddEstudanteRequest request, AppDbContext context, CancellationToken ct) => 
        {

            var jaExiste = await context.Estudantes.AnyAsync(Estudante => Estudante.Nome == request.Nome, ct);
            
            if (jaExiste)
            {
                return Results.Conflict("Já existe");
            }


            var novoEstudante = new Estudante(request.Nome);
            await context.Estudantes.AddAsync(novoEstudante);
            await context.SaveChangesAsync(ct);

            var estudanteRetorno = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);

            return Results.Ok(estudanteRetorno);
        });
    
        // retornar os estudantes cadastrados
        RotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) =>
        {
            var estudante = await context
            .Estudantes
            .Where(estudantes=> estudantes.Ativo)
            .Select(estudante => new EstudanteDto(estudante.Id, estudante.Nome))
            .ToListAsync(ct);
            return estudante;

        });
        // atualizar nome do estudante
        RotasEstudantes.MapPut ("{id:guid}", async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct )=>
        {
            var estudante = await context
            .Estudantes
            .SingleOrDefaultAsync(estudante => estudante.Id == id, ct);

            if (estudante == null)
            {
                return Results.NotFound();
            }
            estudante.AtualizarNome(request.Nome);
            await context.SaveChangesAsync();
            return Results.Ok(new EstudanteDto(estudante.Id, estudante.Nome));
        });

        // deletar

        RotasEstudantes.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken ct )=> 
        {
            var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id, ct);

            if(estudante == null)
            {
                return Results.NotFound();
            }
            estudante.Desativar();
            await context.SaveChangesAsync(ct);
            return Results.Ok();
        });
    }
}