using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoapi.Data;
using todoapi.Models;
using todoapi.Repositories;
using todoapi.Shared;

namespace todoapi.Services
{
    public class ToDoListService: IToDoListService
    {

        private readonly IToDoListRepository repository;
        public ToDoListService(IToDoListRepository repository)
        {
            this.repository = repository;
            
        }
        
        public async Task<List<ToDo>> GetToDosAsync()
        {
             return await repository.GetAllAsync();
        }

        public async Task<List<ToDo>> GetAllAsyncWithDeletedToDos()
        {
             return await repository.GetAllAsyncWithDeletedToDos();
        }

        public async Task<ToDo> GetToDoByIdAsync(int id)
        {
            var getToDo = await repository.GetByIdAsync(id);

            if(getToDo == null)
            {
                throw new Exception($"ToDo item with ID {id} not found");
            }


            return getToDo;
        }

        public async Task<ToDo> PostToDoAsync(ToDo newToDo)
        {
            await repository.PostAsync(newToDo);
            return newToDo;
        }

        //Put        
        public async Task<ToDo> UpdateToDoAsync(int id, ToDo updatedToDo)
        {


            
            if(id != updatedToDo.ToDoID) throw new Exception($"ToDo item with ID {id} do not match the item that wants to update");

            var ExistsToDo = await repository.GetByIdAsync(id);

            if(ExistsToDo == null) throw new Exception($"ToDo item with ID {id} do not exists");


            ExistsToDo.IstoDoDone = updatedToDo.IstoDoDone;
            ExistsToDo.ToDoContent = updatedToDo.ToDoContent;

            try
            {
                await repository.UpdateAsync(ExistsToDo);
            }
            catch (DbUpdateConcurrencyException)
            {               
                    throw;
            }

            return updatedToDo;          

        }

        public async Task<ToDo> DeleteToDoAsync(int id)
        {
            
            var deletedTodo = await repository.GetByIdAsync(id);

            if(deletedTodo == null)
            {
                throw new Exception("ToDo item not found");
            }

            deletedTodo.IsDeleted = true;

            await repository.UpdateAsync(deletedTodo);
            
            return deletedTodo;
        }

    }
}