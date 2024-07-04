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
                        Console.WriteLine("\u001b[31;1mOpção inválida. Pressione qualquer tecla para voltar ao menu principal...\u001b[0m");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        static void AdicionarTarefa(TarefaDAO dao)
        {
            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\u001b[33;1m>>> Adicionar Nova Tarefa\u001b[0m");
            Console.WriteLine("===============================");

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

            Console.WriteLine($"\u001b[32;1mTarefa [{tarefa.Nome}] adicionada com sucesso.\u001b[0m");
            Console.WriteLine();

            VoltarParaMenuTarefas();
        }

        static void ListarTarefas(TarefaDAO dao)
        {
            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\u001b[33;1m>>> Listagem de Tarefas\u001b[0m");
            Console.WriteLine("===============================");

            var tarefas = dao.ObterTarefas();
            if (tarefas.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa encontrada.");
            }
            else
            {
                foreach (var tarefa in tarefas)
                {
                    Console.WriteLine($"Id: {tarefa.Id}, Nome: {tarefa.Nome}, Descrição: {tarefa.Descricao}, Data de Criação: {tarefa.DataCriacao}");
                }
            }

            Console.WriteLine();

            VoltarParaMenuTarefas();
        }

        static void AtualizarTarefa(TarefaDAO dao)
        {
            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\u001b[33;1m>>> Atualizar Tarefa\u001b[0m");
            Console.WriteLine("===============================");

            int id = LerIdTarefa();
            if (id == -1)
            {
                return;
            }

            var tarefaExistente = dao.ObterTarefaPorId(id);
            if (tarefaExistente == null)
            {
                Console.WriteLine($"\u001b[31;1mTarefa com ID {id} não encontrada.\u001b[0m");
                Console.WriteLine();
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

            Console.WriteLine($"\u001b[32;1mTarefa [{tarefaExistente.Nome}] atualizada com sucesso.\u001b[0m");
            Console.WriteLine();

            VoltarParaMenuTarefas();
        }

        static void DeletarTarefa(TarefaDAO dao)
        {
            Console.WriteLine();

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\u001b[33;1m>>> Deletar Tarefa\u001b[0m");
            Console.WriteLine("===============================");

            int id = LerIdTarefa();
            if (id == -1)
            {
                return;
            }

            var tarefaExistente = dao.ObterTarefaPorId(id);
            if (tarefaExistente == null)
            {
                Console.WriteLine($"\u001b[31;1mTarefa com ID {id} não encontrada.\u001b[0m");
                Console.WriteLine();
                VoltarParaMenuTarefas();
                return;
            }

            Console.Write($"\u001b[31;1mTem certeza que deseja deletar a tarefa [{tarefaExistente.Nome}]? (S/N): \u001b[0m");
            var confirmacao = Console.ReadLine();
            if (confirmacao.ToLower() == "s")
            {
                dao.DeletarTarefa(id);
                Console.WriteLine("\u001b[31;1mTarefa deletada com sucesso.\u001b[0m");
            }
            else
            {
                Console.WriteLine("Operação cancelada.");
            }

            Console.WriteLine();

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
                        Console.WriteLine("\u001b[31;1mOpção inválida. Pressione qualquer tecla para voltar ao menu de tarefas...\u001b[0m");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        static void MostrarMenuPrincipal()
        {
            Console.WriteLine("\u001b[33;1m===============================\u001b[0m");
            Console.WriteLine("\u001b[33;1mGestão de Tarefas - Menu Principal\u001b[0m");
            Console.WriteLine("\u001b[33;1m===============================\u001b[0m");
            Console.WriteLine("1. Gerenciar Tarefas");
            Console.WriteLine("2. Sair");
            Console.Write("Escolha uma opção: ");
        }

        static void MostrarMenuTarefas()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\u001b[33;1m>>> Menu de Tarefas <<<\u001b[0m");
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
                    Console.WriteLine();
                    Console.WriteLine("\u001b[31;1mPor favor, digite um número válido.\u001b[0m");
                }
            }
        }

        static void FecharAplicacao()
        {
            Console.WriteLine();

            Console.WriteLine("\u001b[33;1m>>> Fechando a aplicação em 5 segundos...\u001b[0m");
            Console.WriteLine("===============================");

            for (int i = 5; i > 0; i--)
            {
                Console.WriteLine($"\u001b[33;1mFechando em {i}...\u001b[0m");
                Thread.Sleep(1000);
            }

            Environment.Exit(0);
        }

        static void VoltarParaMenuTarefas()
        {
            Console.WriteLine($"Pressione qualquer tecla para voltar ao menu de tarefas...");
            Console.ReadKey();
        }
    }
}
