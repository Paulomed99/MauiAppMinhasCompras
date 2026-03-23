using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try 
		{

            grid_carregamento.IsVisible = true;
            carregador.IsRunning = true;
            await Task.Delay(3000);

            Produto p = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text)
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
        }
    }
}