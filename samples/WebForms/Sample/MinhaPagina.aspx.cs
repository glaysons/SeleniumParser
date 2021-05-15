using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sample.Models;

namespace Sample
{
	public partial class MinhaPagina : Page
	{

		private IList<Fulano> Fulanos
		{
			get
			{
				if (ViewState["Fulanos"] == null)
					ViewState["Fulanos"] = new List<Fulano>();
				return (IList<Fulano>)ViewState["Fulanos"];
			}
		}

		private string Nome { get { return NomeText.Text; } set { NomeText.Text = value; } }
		private string Sobrenome { get { return SobrenomeText.Text; } set { SobrenomeText.Text = value; } }
		private int Idade { get { return int.TryParse(IdadeText.Text, out int idade) ? idade : 0; } set { IdadeText.Text = (value > 0) ? value.ToString() : string.Empty; } }
		private string Mensagem { get { return MensagemText.Text; } set { MensagemText.Text = value; } }
		private int SomaDasIdades { get { return int.TryParse(SomaDasIdadesLabel.Text, out int soma) ? soma : 0; } set { SomaDasIdadesLabel.Text = value.ToString(); } }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				PreencherGrade();
		}

		private void PreencherGrade()
		{
			FulanosGrid.DataSource = Fulanos;
			FulanosGrid.DataBind();
			SomaDasIdades = Fulanos.Sum(f => f.Idade);
		}

		protected void InserirButton_Click(object sender, EventArgs e)
		{
			Fulanos.Add(new Fulano()
			{
				Nome = Nome,
				Sobrenome = Sobrenome,
				Idade = Idade,
				Mensagem = Mensagem
			});

			Nome = string.Empty;
			Sobrenome = string.Empty;
			Idade = 0;
			Mensagem = string.Empty;

			System.Threading.Thread.Sleep(1000 * 5);

			PreencherGrade();
		}
	}
}