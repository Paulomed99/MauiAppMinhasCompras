using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path) 
        {
            _conn = new SQLiteAsyncConnection(path);

            _conn.CreateTableAsync<Produto>().Wait();
        }

        // Método para inserir um novo produto, como entrada temos o produto "p".
        // Retorna a quantidade de linhas inseridas.
        public Task<int> Insert(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        // Método para alterar um registro já existente, como entrada temos o produto "p".
        // Aqui utilizamos o método de Prepared Statements, para evitar SQL Injection.
        public Task<List<Produto>> Update(Produto p)
        {
            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=?, Data=? WHERE Id=?";

            return _conn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Data, p.Id);
        }

        // Método para apagar um produto.
        // Em vez de query manual, usa o jeito do ORM com expressão lambda (i => i.Id == Id).
        public Task<int> Delete(int Id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == Id);
        }

        // Traz a tabela completa.
        // Equivalente a um "SELECT * FROM Produto", mas usando o método do próprio pacote do SQLite
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        // Método de busca para filtrar produtos pela descrição.
        // Aqui nao está sendo usado método de Prepared Statements, risco de SQL Injection.
        public Task<List<Produto>> Search(string q) 
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";

            return _conn.QueryAsync<Produto>(sql);
        }

        // Método de busca para filtrar produtos pela categoria.
        public Task<List<Produto>> SearchCategoria(Produto.CategoriaProduto q)
        {
            return _conn.Table<Produto>()
                         .Where(i => i.Categoria == q)
                         .ToListAsync();
        }

        // Método de busca para filtrar produtos pela data de compra.
        public Task<List<Produto>> SearchData(DateTime dataInicio, DateTime dataFim)
        {
            return _conn.Table<Produto>()
                         .Where(i => i.Data >= dataInicio && i.Data <= dataFim)
                         .ToListAsync();
        }

    }
}
