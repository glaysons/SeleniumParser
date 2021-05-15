# SeleniumParser

SeleniumParser é um interpretador de arquivos .SIDE gerados pela IDE do Selenium.

O objetivo é utilizar as gravações dos testes de usabilidade gerados pela própria IDE do Selenium nos testes automatizados, de forma bem simples e de fácil manutenção.

O desenvolvedor utiliza estes arquivos apenas como referência ao criar testes unitários.

## Como Utilizar ##

### Instalar ###

Disponível via **nuget**. ***Ainda em desenvolvimento***

```
PM> Install-Package SeleniumParser.Parser
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
