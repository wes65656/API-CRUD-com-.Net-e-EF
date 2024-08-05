namespace MinhaWebAPI.Estudantes;

public class Estudante
{
    public Guid Id {get; init;} // O quê é o init ? ele diz que depois que o ID for estanciado nada poderá alterar o ID
    public string Nome {get; private set;}
    public bool Ativo {get; private set;}

    public Estudante(string nome)
    {
        Nome = nome;
        Id = Guid.NewGuid();
        Ativo = true;
    }
    public void AtualizarNome(string nome)
    {
        Nome = nome;
    }
    public void Desativar()
    {
        Ativo = false;
    }
}