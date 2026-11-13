# Mestre da Brasa — portfólio e solicitações de eventos

Adaptação do frontend React enviado para o sistema ASP.NET Core Razor Pages, usando o namespace `Churrascaria` e a rota existente `/Agendamento/Novo`.

A paleta original foi mantida: carvão `#1a1512`, creme `#f4ede2` e vermelho brasa. O vermelho dos botões foi escurecido para melhorar o contraste. A tipografia usa fontes do computador, sem carregar fontes, frameworks CSS ou scripts de terceiros.

## O que está incluído

- Página inicial com apresentação, tipos de evento, galeria, etapas do atendimento e perguntas frequentes.
- Formulário para solicitar serviço no local de um evento, em vez de reservar mesa em restaurante.
- Cadastro de nome, telefone, data/horário, convidados, ocasião, cidade, local e observações.
- Validação no servidor e campos obrigatórios no navegador.
- Persistência com EF Core e SQLite; migration inicial incluída.
- Mensagem com protocolo após a gravação. O redirecionamento após POST evita reenviar o formulário ao atualizar a página.
- Área da equipe protegida por login, com filtros de data e status e paginação.
- Status: solicitado → confirmado → concluído; pedidos solicitados/confirmados podem ser cancelados.
- Navegação mobile, foco visível, rótulos nos campos, preferências de movimento reduzido e galeria ampliável.

O sistema recebe **solicitações de orçamento**, que só são confirmadas pela equipe após combinar o serviço. Não há confirmação automática de disponibilidade, cobrança ou integração automática com WhatsApp. O botão de WhatsApp, quando configurado, abre uma conversa.

## 1. Executar no VS Code

Esta versão é destinada ao **.NET 8**. Confirme a instalação do SDK e do runtime antes de executar:

```bash
dotnet --list-sdks
dotnet --list-runtimes
```

Abra a pasta que contém `Churrascaria.csproj` no VS Code:

```bash
cd Churrascaria
dotnet restore
dotnet build
dotnet run
```

Acesse `http://localhost:5080`. O banco `churrascaria-eventos.db` é criado na pasta do projeto na primeira execução. Não é necessário criar outra migration inicial. Em novas alterações do modelo, use migrations do EF Core normalmente.

Se o seu projeto atual usa outra versão de .NET, mantenha o framework que já utiliza e alinhe a versão principal dos pacotes EF Core no `.csproj`. O pacote `8.0.*` deste projeto resolve a versão de manutenção disponível da série 8. Depois de validar uma publicação, fixe a versão restaurada ou utilize arquivo de lock para reproduzir as dependências.

## 2. Configurar o acesso da equipe

Na pasta do projeto, defina uma senha própria nos segredos de desenvolvimento:

```bash
dotnet user-secrets set "Admin:Usuario" "equipe"
dotnet user-secrets set "Admin:Senha" "SUBSTITUA_POR_UMA_SENHA_LONGA_E_UNICA"
```

Troque o texto de exemplo pela sua senha no seu terminal. A senha não tem valor padrão e não é gravada no repositório. Acesse `/Conta/Entrar` ou o link **Área da equipe** no rodapé. Após entrar, os pedidos ficam em `/Admin`.

Na hospedagem, defina os valores como segredos/variáveis do servidor: `Admin__Usuario` e `Admin__Senha`. `user-secrets` só é usado no ambiente de desenvolvimento. Esta versão tem um único acesso para a equipe; contas individuais podem ser adicionadas depois com ASP.NET Core Identity.

## 3. Ajustar a identidade e os contatos

Edite a seção `Empresa` de `appsettings.json`:

- `Nome`: nome comercial real; “Mestre da Brasa” veio do frontend enviado.
- `Descricao`: descrição curta do serviço para o site.
- `WhatsApp`: número com DDI e DDD, por exemplo `5521999999999`; o exemplo não é um contato do cliente.
- `Instagram`: URL completa do perfil real, iniciando com `https://www.instagram.com/`.
- `Regiao`: área que o cliente realmente atende.
- `FusoHorario`: fuso usado para comparar a data do evento com o horário atual. Padrão: `America/Sao_Paulo`.

Os contatos vazios ficam ocultos. Não há telefones, perfis ou e-mails fictícios no site.

## 4. Inserir as fotos reais

**O ZIP original não contém as fotos do churrasqueiro.** Contém somente o ícone da chama, ícones de ferramentas e URLs de imagens ilustrativas. Por isso, esta versão usa a identidade da marca na capa e um estado vazio na galeria; não apresenta imagens genéricas como eventos realizados pelo cliente.

Coloque fotos autorizadas em `wwwroot/images/`. Use nomes simples e arquivos JPG/WebP otimizados, preferencialmente entre 1.200 e 1.600 pixels de largura. O site apenas exibe arquivos; não inclui envio de fotos pelo painel.

Exemplo de configuração, após incluir os arquivos correspondentes:

```json
"FotoPrincipal": "~/images/churrasqueiro-no-evento.jpg",
"FotoPrincipalAlt": "Churrasqueiro preparando os cortes durante um evento",
"Portfolio": [
  {
    "Caminho": "~/images/preparo-01.jpg",
    "Titulo": "Na churrasqueira",
    "Categoria": "Preparo",
    "Descricao": "Cortes sendo preparados na churrasqueira"
  },
  {
    "Caminho": "~/images/mesa-01.jpg",
    "Titulo": "Pronto para servir",
    "Categoria": "À mesa",
    "Descricao": "Mesa montada com os itens servidos no evento"
  },
  {
    "Caminho": "~/images/evento-01.jpg",
    "Titulo": "Um dia para reunir",
    "Categoria": "Eventos",
    "Descricao": "Estrutura montada pela equipe para uma confraternização"
  }
]
```

Os títulos e descrições são exemplos: ajuste-os ao que as fotos realmente mostram. Use somente fotos cuja publicação foi autorizada; o formulário não dá autorização para uso de imagem. As fotos da galeria são carregadas sob demanda e podem ser ampliadas.

## 5. Integrar ao projeto que você já criou

Também é possível executar esta pasta como um projeto completo, sem alterar sua versão anterior. Para integrar manualmente, faça uma cópia do projeto existente e:

1. Copie `Pages`, `wwwroot`, `Models`, `Data` e `Services`, conferindo arquivos de mesmo nome.
2. Compare `Program.cs` e adicione o registro do contexto, configuração da empresa, autenticação e autorização. Mantenha outros registros necessários do seu projeto.
3. Adicione os pacotes EF Core e o `UserSecretsId` do `.csproj`, conforme sua versão de .NET.
4. Incorpore as seções de `appsettings.json` e, se desejar a mesma porta local, `Properties/launchSettings.json`.
5. Para começar com o banco novo desta versão, copie `Migrations` e utilize a conexão `churrascaria-eventos.db`.

O banco novo tem campos específicos de eventos e **não importa automaticamente** registros de um `churrascaria.db` anterior. Se você já usa outro `DbContext`, Identity ou migrations, incorpore os campos ao contexto existente e gere uma nova migration; não misture históricos de migrations diretamente.

O antigo `DadosAgendamento`, que ficava dentro de `NovoModel`, foi movido para `Models/DadosAgendamento.cs`. A propriedade do formulário continua se chamando `Agendamento`, mantendo os vínculos `asp-for="Agendamento.NomeCliente"` e os demais campos.

`Models/Agendamento.cs` representa a entidade no banco. Como existe uma pasta/namespace `Pages.Agendamento`, o PageModel utiliza `Churrascaria.Models.Agendamento` explicitamente para evitar o conflito entre namespace e classe.

## Arquivos principais

| Arquivo | Responsabilidade |
| --- | --- |
| `Pages/Index.cshtml` | Página pública de portfólio |
| `Pages/Shared/_Layout.cshtml` | Cabeçalho, menu, rodapé e arquivos estáticos |
| `Pages/Agendamento/Novo.cshtml` | Formulário de solicitação |
| `Pages/Agendamento/Novo.cshtml.cs` | Recebimento, validação e gravação |
| `Models/DadosAgendamento.cs` | Somente os dados enviados pelo visitante |
| `Models/Agendamento.cs` | Dados persistidos e status controlado pela equipe |
| `Data/ChurrascariaContext.cs` | Contexto do EF Core |
| `Pages/Admin/Index.cshtml` | Consulta e atualização de status |
| `wwwroot/css/site.css` | Cores, tipografia e layout responsivo |
| `wwwroot/js/site.js` | Menu, ampliação das fotos e proteção visual contra duplo envio |

## Teste local antes de usar com o cliente

1. Envie um pedido válido e anote o protocolo.
2. Entre na área da equipe e confira os dados recebidos.
3. Feche e reabra o aplicativo: o registro deve continuar no banco.
4. Envie uma data passada, um telefone inválido e uma quantidade vazia/zero: o servidor deve rejeitar os dados.
5. Atualize a página após um envio bem-sucedido: não deve gerar novo registro.
6. Confirme um pedido e conclua-o. Cancele outro e confira os filtros.
7. Saia da conta e tente abrir `/Admin`: deverá ser direcionado ao login.
8. Confira no celular o menu, o formulário e as fotos reais após adicioná-las.

## Para publicar

Use hospedagem que execute ASP.NET Core. A pasta publicada deve ter acesso de escrita ao arquivo do banco; o banco precisa ficar em disco persistente, com backup e recuperação definidos. Na produção, ajuste `AllowedHosts`, use HTTPS e configure os segredos da equipe no servidor. A migration automática deste exemplo pressupõe uma única instância; em ambientes com várias instâncias, aplique migrations no processo de implantação.

O limite por IP foi pensado para uso inicial. Se a aplicação ficar atrás de proxy, configure encaminhamento de IP somente de proxies conhecidos antes de usar o limitador em produção. Confirme com o cliente a região atendida, itens incluídos, capacidade da equipe e o texto de privacidade. Disponibilidade, alterações de data e cancelamentos com o cliente são combinados manualmente nesta versão.

## Verificação desta entrega

Foram verificados os arquivos do ZIP, nomes de rotas e namespaces, links e rótulos da prévia HTML e a sintaxe do JavaScript. O ambiente de criação não disponibiliza .NET SDK; portanto, **não foi possível executar `dotnet build`, migrations ou testar a gravação/login em execução**. As verificações locais acima devem ser feitas antes da publicação.

A prévia HTML separada reproduz a interface das páginas, mas **não salva agendamentos**. O projeto Razor Pages é a versão que contém o backend.

## Documentação de referência

- Razor Pages: https://learn.microsoft.com/pt-br/aspnet/core/razor-pages/
- EF Core e SQLite: https://learn.microsoft.com/pt-br/ef/core/get-started/overview/first-app
