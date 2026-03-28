using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {

        string _descricao;
        double _quantidade;
        double _preco;
        public enum CategoriaProduto { Alimentacao, Limpeza, Higiene, Bebidas, Outros };
        CategoriaProduto _categoria;
        DateTime _data;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { 
            get => _descricao;

            set 
            {
                if (string.IsNullOrWhiteSpace(value)) 
                {
                    throw new Exception("Por favor, preencha a descrição");
                }
            
                _descricao = value;

            }
        }
        public double Quantidade {
            get => _quantidade;

            set
            {
                if (value <= 0)
                {
                    throw new Exception("Por favor, preencha a quantidade");
                }

                _quantidade = value;

            }
        }
        public double Preco {
            get => _preco;

            set
            {
                if (value <= 0)
                {
                    throw new Exception("Por favor, preencha o preço");
                }

                _preco = value;

            }
        }
        public double Total { get => Quantidade * Preco; }
        public CategoriaProduto Categoria
        {
            get => _categoria;

            set
            {
                 _categoria = value;
            }
        }

        public DateTime Data
        {
            get => _data;

            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new Exception("Por favor, escolha uma data válida");
                }

                _data = value;

            }
        }

    }
}
