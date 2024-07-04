using ArtigoTech.GestorTarefas.App.Models;
using System.Collections.Generic;

namespace ArtigoTech.GestorTarefas.App.Interfaces
{
    public interface ITarefaDAO
    {
        void AdicionarTarefa(Tarefa tarefa);
        List<Tarefa> ObterTarefas();
        Tarefa ObterTarefaPorId(int id);
        void AtualizarTarefa(Tarefa tarefa);
        void DeletarTarefa(int id);
    }
}
