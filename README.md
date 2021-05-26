# SeleniumParser

SeleniumParser é um interpretador de arquivos .SIDE gerados pela IDE do Selenium.

O objetivo é utilizar as gravações dos testes de usabilidade gerados pela própria IDE do Selenium nos testes automatizados, de forma bem simples e de fácil manutenção.

O desenvolvedor utiliza estes arquivos apenas como referência ao criar testes unitários.

## Como Utilizar ##

### Instalar ###

Disponível via **nuget**.

```
PM> Install-Package SeleniumParser.Parser -Version 1.0.2
```

### Criação dos Testes ###

Basta criar um teste de unidade, indicar o endereço completo do arquivo .SIDE e qual o WebDriver que será utilizado junto ao Selenium:
  
```
[TestMethod]
public void AoCadastrarAlterarEExcluirBairrosDeveDarTudoCerto()
{
	var sideFile = @"D:\Projetos\Selenium\AoCadastrarAlterarEExcluirBairrosDeveDarTudoCerto.side";
	new Parser().ParseOneTestByBrowserInstance(sideFile, () => new ChromeDriver(@"D:\Projetos\Selenium\bin\Debug"));
}
```

### Instancia dos Drivers ###

O Parser possui 3 métodos, sendo:
- **ParseTests** : Você é responsável por gerenciar todo o ciclo de vida da instância do Browser;
- **ParseOneTestByBrowserInstance** : Será instanciado um Browser para cada teste existente no arquivo .SIDE;
- **ParseAllTestsOnSameBrowserInstance** : Será utilizado apenas uma instância do Browser para a execução de todos os testes existentes no arquivo .SIDE;

### Gravação dos Arquivos SIDE ###

Os arquivos SIDE interpretados pelo parser são gerados pela seguinte extensão do Google Chrome:

https://chrome.google.com/webstore/detail/selenium-ide/mooikfkahbdckldjjndioackbalphokd

Basta realizar a gravação e pedir para salvar, automaticamente a ferramenta irá sugerir um arquivo .SIDE para ser salvo.

Os testes foram realizados com a versão 3.17 da extensão.

## Detalhes das Últimas Versões ##

v1.0.2
- Correção dos **comandos ChooseOkOnNextConfirmationCommand e ChooseCancelOnNextConfirmationCommand** para validarem após o próximo comando existente;
- Inutilização momentânea dos comandos *WebDriverChooseOkOnVisibleConfirmationCommand e WebDriverChooseCancelOnVisibleConfirmationCommand*;
