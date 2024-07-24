using ArtigoTech.GestorTarefas.App.DataAccess;
using ArtigoTech.GestorTarefas.App.Models;
using System;
using System.Threading;

namespace ArtigoTech.GestorTarefas.App
{
    public class Program
    {
        static void Main(string[] args)
        {
            Database.CriarTabelaTarefas(); // Cria a tabela se não existir

            var dao = new TarefaDAO();
            while (true)
            {
                MostrarMenuPrincipal();

                var opcoesPrincipal = Console.ReadLine();

                switch (opcoesPrincipal)
                {
                    case "1":
                        GerenciarTarefas(dao);
                        break;
                    case "2":
                        FecharAplicacao();
                        return;
                    default:
                        EscreverMensagemOpcaoInvalida();
                        break;
                }
            }
        }

        static void AdicionarTarefa(TarefaDAO dao)
        {
            EscreverTitulo("Adicionar Nova Tarefa");

            Console.Write("Nome da tarefa (ou 'V' para voltar): ");
            var nome = Console.ReadLine();

            if (nome.ToLower() == "v")
                return;

            Console.Write("Descrição da tarefa: ");
            var descricao = Console.ReadLine();

            var tarefa = new Tarefa 
            { 
                Nome = nome, 
                Descricao = descricao 
            };
            dao.AdicionarTarefa(tarefa);

            EscreverMensagemAviso($"Tarefa [{tarefa.Nome}] adicionada com sucesso!");
            VoltarParaMenuTarefas();
        }

        static void ListarTarefas(TarefaDAO dao)
        {
            EscreverTitulo("Listagem de Tarefas");

            var tarefas = dao.ObterTarefas();
            if (tarefas.Count == 0)
            {
                EscreverMensagemAviso("Nenhuma tarefa encontrada!");
            }
            else
            {
                foreach (var tarefa in tarefas)
                {
                    Console.WriteLine($"Id: {tarefa.Id}, Nome: {tarefa.Nome}, Descrição: {tarefa.Descricao}, Data de Criação: {tarefa.DataCriacao}");
                }
            }

            VoltarParaMenuTarefas();
        }

        static void AtualizarTarefa(TarefaDAO dao)
        {
            EscreverTitulo("Atualizar Tarefa");
            
            int id = LerIdTarefa();
            if (id == -1)
            {
                return;
            }

            var tarefaExistente = dao.ObterTarefaPorId(id);
            if (tarefaExistente == null)
            {
                EscreverMensagemAviso($"Tarefa com ID {id} não encontrada");
                VoltarParaMenuTarefas();
                return;
            }

            Console.Write("Novo nome da tarefa (deixe em branco para manter o mesmo): ");
            var novoNome = Console.ReadLine();

            Console.Write("Nova descrição da tarefa (deixe em branco para manter a mesma): ");
            var novaDescricao = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(novoNome))
                tarefaExistente.Nome = novoNome;
            if (!string.IsNullOrWhiteSpace(novaDescricao))
                tarefaExistente.Descricao = novaDescricao;

            dao.AtualizarTarefa(tarefaExistente);

            EscreverMensagemAviso($"Tarefa [{tarefaExistente.Nome}] atualizada com sucesso.");
            VoltarParaMenuTarefas();
        }

        static void DeletarTarefa(TarefaDAO dao)
        {
            EscreverTitulo("Deletar Tarefa");

            int id = LerIdTarefa();
            if (id == -1)
                return;

            var tarefaExistente = dao.ObterTarefaPorId(id);
            if (tarefaExistente == null)
            {
                EscreverMensagemAviso($"Tarefa com ID {id} não encontrada!");
                VoltarParaMenuTarefas();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($">>> Tem certeza que deseja deletar a tarefa [{tarefaExistente.Nome}]? (S/N): ");
            var confirmacao = Console.ReadLine();
            if (confirmacao.ToLower() == "s")
            {
                dao.DeletarTarefa(id);
                EscreverMensagemAviso("Tarefa deletada com sucesso!");
            }
            else
            {
                EscreverMensagemAviso("Operação cancelada!");
            }

            VoltarParaMenuTarefas();
        }

        static void GerenciarTarefas(TarefaDAO dao)
        {
            while (true)
            {
                Console.Clear();
                MostrarMenuTarefas();

                var opcaoTarefas = Console.ReadLine();

                switch (opcaoTarefas)
                {
                    case "1":
                        AdicionarTarefa(dao);
                        break;
                    case "2":
                        ListarTarefas(dao);
                        break;
                    case "3":
                        AtualizarTarefa(dao);
                        break;
                    case "4":
                        DeletarTarefa(dao);
                        break;
                    case "5":
                        Console.Clear();
                        return;
                    case "6":
                        FecharAplicacao();
                        return;
                    default:
                        EscreverMensagemOpcaoInvalida();
                        break;
                }
            }
        }

        static void MostrarMenuPrincipal()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================================");
            Console.WriteLine("Gestão de Tarefas - Menu Principal");
            Console.WriteLine("==================================");
            Console.ResetColor();
            Console.WriteLine("1. Gerenciar Tarefas");
            Console.WriteLine("2. Sair");
            Console.Write("Escolha uma opção: ");
        }

        static void MostrarMenuTarefas()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(">>> Menu de Tarefas <<<");
            Console.ResetColor();
            Console.WriteLine("===============================");
            Console.WriteLine("1. Adicionar Tarefa");
            Console.WriteLine("2. Listar Tarefas");
            Console.WriteLine("3. Atualizar Tarefa");
            Console.WriteLine("4. Deletar Tarefa");
            Console.WriteLine("5. Voltar ao Menu Principal");
            Console.WriteLine("6. Sair");
            Console.Write("Escolha uma opção: ");
        }

        static int LerIdTarefa()
        {
            string entrada;
            int id;
            while (true)
            {
                Console.Write("Digite o ID da tarefa (ou 'V' para voltar): ");
                entrada = Console.ReadLine().Trim();

                if (entrada.ToLower() == "v")
                {
                    return -1;
                }

                if (int.TryParse(entrada, out id))
                {
                    return id;
                }
                else
                {
                    EscreverMensagemAviso("Por favor, digite um ID válido.");
                    Console.WriteLine();
                }
            }
        }

        static void FecharAplicacao()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine(">>> Fechando a aplicação em 5 segundos...");
            Console.WriteLine("===============================");

            for (int i = 5; i > 0; i--)
            {
                Console.WriteLine($"Fechando em {i}...");
                Thread.Sleep(1000);
            }

            Environment.Exit(0);
        }

        static void VoltarParaMenuTarefas()
        {
            Console.WriteLine();
            Console.WriteLine($"Pressione qualquer tecla para voltar ao menu de tarefas...");
            Console.ReadKey();
        }

        static void EscreverTitulo(string texto)
        {
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(">>> " + texto);
            Console.ResetColor();
            Console.WriteLine("===============================");
        }

        static void EscreverMensagemAviso(string texto)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(">>> " + texto);
            Console.ResetColor();
        }

        static void EscreverMensagemOpcaoInvalida()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(">>> Opção inválida! Pressione qualquer tecla para voltar...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
