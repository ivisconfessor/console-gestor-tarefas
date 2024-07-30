using ArtigoTech.GestorTarefas.App.Interfaces;
using ArtigoTech.GestorTarefas.App.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtigoTech.GestorTarefas.App.DataAccess
{
    public class TarefaDAO : ITarefaDAO
    {
        private readonly DatabaseContext _databaseContext;

        public TarefaDAO()
        {
            _databaseContext = new DatabaseContext();
        }

        public void AdicionarTarefa(Tarefa tarefa)
        {
            tarefa.DataCriacao = DateTime.Now;
            _databaseContext.Tarefas.Add(tarefa);
            _databaseContext.SaveChanges();
        }

        public List<Tarefa> ObterTarefas()
        {
            var tarefas = _databaseContext.Tarefas.ToList();
            return tarefas;
        }

        public Tarefa ObterTarefaPorId(int id)
        {
            var tarefa = _databaseContext.Tarefas.FirstOrDefault(t => t.Id == id);
            return tarefa;
        }

        public void AtualizarTarefa(Tarefa tarefa)
        {
            var tarefaAtualizar = _databaseContext.Tarefas.FirstOrDefault(t=> t.Id == tarefa.Id);
            tarefaAtualizar.Nome = tarefa.Nome;
            tarefaAtualizar.Descricao = tarefa.Descricao;
            _databaseContext.SaveChanges();
        }

        public void DeletarTarefa(int id)
        {
            var tarefaDeletar = _databaseContext.Tarefas.FirstOrDefault(t => t.Id == id);
            _databaseContext.Tarefas.Remove(tarefaDeletar);
            _databaseContext.SaveChanges();
        }
    }
}
