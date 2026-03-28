using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public Array ListaCategorias { get; set; }
    public NovoProduto()
	{
		InitializeComponent();

        ListaCategorias = Enum.GetValues(typeof(Models.Produto.CategoriaProduto));

        this.BindingContext = this;
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try 
		{

            grid_carregamento.IsVisible = true;
            carregador.IsRunning = true;
            btn_salvar.IsEnabled = false;
            await Task.Delay(3000);

            Produto p = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = (Produto.CategoriaProduto)pck_categoria.SelectedItem,
                Data = dt_dataCompra.Date,
                
			};

			await App.Db.Insert(p);
			
            txt_descricao.Text = string.Empty;
            txt_quantidade.Text = string.Empty;
            txt_preco.Text = string.Empty;
            
            await DisplayAlert("Sucesso!", "Registro inserido", "OK");

        } 
		catch(Exception ex) 
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
        finally
        {
            carregador.IsRunning = false;
            grid_carregamento.IsVisible = false;
            btn_salvar.IsEnabled = true;
        }
    }
}