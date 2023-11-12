namespace Academico.Models
{
    public class ErrorPartialModel
    {
        public ErrorPartialModel(string erro, string mensagem)
        {
            Erro = erro;
            Mensagem = mensagem;
        }

        public string Erro { get; set; }

        public string Mensagem { get; set; }
    }
}
