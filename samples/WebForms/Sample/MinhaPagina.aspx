<%@ Page Title="Minha Página" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MinhaPagina.aspx.cs" Inherits="Sample.MinhaPagina" %>

<asp:Content ContentPlaceHolderID="AreaDeScripts" runat="server">
	<script>
		function MensagemDeBoasVindas(mensagem) {
			alert(mensagem);
		}
	</script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Let's go to the playground.</h3>
    <p>Use this area to provide additional information.</p>
	<p>Campos para Preenchimento:</p>
	<p>Nome:
		<asp:TextBox ID="NomeText" runat="server"></asp:TextBox>
	</p>
	<p>Sobrenome:
		<asp:TextBox ID="SobrenomeText" runat="server"></asp:TextBox>
	</p>
	<p>Idade:
		<asp:TextBox ID="IdadeText" runat="server"></asp:TextBox>
	</p>
	<p>Mensagem do dia:
		<asp:TextBox ID="MensagemText" runat="server"></asp:TextBox>
	</p>
	<p>
		<asp:Button ID="InserirButton" runat="server" Text="Button" OnClick="InserirButton_Click" />
	</p>
	<p>&nbsp;</p>
	<p>Soma das Idades:
		<asp:Label ID="SomaDasIdadesLabel" runat="server" Text="0"></asp:Label>
	</p>
	<p>&nbsp;</p>
	<p>Registros Existentes:</p>
	<p>
		<asp:DataGrid ID="FulanosGrid" runat="server" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" Width="100%">
			<FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
			<HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
			<ItemStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
			<PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" Mode="NumericPages" />
			<SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
		</asp:DataGrid>
	</p>
</asp:Content>
