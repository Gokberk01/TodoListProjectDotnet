using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using todoapi.Data;
using todoapi.Models;
using todoapi.Shared;

namespace todoapi.Repositories
{

    public class ToDoListRepository : IToDoListRepository
    {
            private readonly AppDbContext _ToDoListcontext;

            public  ToDoListRepository (AppDbContext context)
            {
                _ToDoListcontext = context;
            }

            public async Task<List<ToDo>> GetAllAsync()
            {
                var _todoList = _ToDoListcontext.ToDoList;
                var result = await _todoList.OrderBy(x => x.ToDoID).Where(x => x.IsDeleted != true).ToListAsync();
                return result;
            }
            
            public async Task<List<ToDo>> GetAllAsyncWithDeletedToDos()
            {
                var _todoList = _ToDoListcontext.ToDoList;
                var result = await _todoList.OrderBy(x => x.ToDoID).ToListAsync();
                return result;
            }

            public async Task<ToDo> GetByIdAsync(int id)
            {
                var ExistData =  await _ToDoListcontext.ToDoList.FindAsync(id);
                if(ExistData != null)
                {
                    return ExistData;
                }
                else 
                {
                    throw new Exception($"ToDo item with ID {id} not found");
                }

            }
            
            public async Task PostAsync(ToDo todo) 
            {
                await _ToDoListcontext.ToDoList.AddAsync(todo);
                await _ToDoListcontext.SaveChangesAsync();
            }

            public async Task UpdateAsync(ToDo updateToDo) 
            {
                _ToDoListcontext.ToDoList.Update(updateToDo);
                await _ToDoListcontext.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var deletedToDo = await _ToDoListcontext.ToDoList.FindAsync(id);
                if(deletedToDo != null)
                {
                    _ToDoListcontext.ToDoList.Remove(deletedToDo);
                    await _ToDoListcontext.SaveChangesAsync();
                }
            }

    }
}